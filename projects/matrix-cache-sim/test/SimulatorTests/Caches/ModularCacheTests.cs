/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.Caches;
using Mcs.Simulator.CacheLines;
using Mcs.Simulator.Events;
using Mcs.Simulator.Memory;
using Mcs.Simulator.Policies.Eviction;
using Mcs.Simulator.Policies.Placement;
namespace McsTests.Simulator.Caches;

public class ModularCacheTests
{
	[Fact]
	public void CacheLinesNotPresentInitially()
	{
		const int CACHE_SIZE = 4;
		const int CACHE_LINE_SIZE = 8;
		var placementPolicy = new Mock<IPlacementPolicy>();
		var evictionPolicy = new Mock<IEvictionPolicy>();
		var cache = new ModularCache(
			CACHE_SIZE,
			CACHE_LINE_SIZE,
			placementPolicy.Object,
			evictionPolicy.Object
		);

		// No cache lines should be loaded immediately after construction
		for (var i = 0; i < CACHE_SIZE; i++)
		{
			Assert.False(cache.IsPresent(i * CACHE_LINE_SIZE));
		}
	}

	[Fact]
	public void LoadCacheLineTriggersEvent()
	{
		// Set up the cache
		const int CACHE_SIZE = 4;
		const int CACHE_LINE_SIZE = 8;
		var placementPolicy = new Mock<IPlacementPolicy>();
		var evictionPolicy = new Mock<IEvictionPolicy>();
		var cache = new ModularCache(
			CACHE_SIZE,
			CACHE_LINE_SIZE,
			placementPolicy.Object,
			evictionPolicy.Object
		);

		// Bind to the cache line loaded event
		OnCacheLineLoadedEventArgs? eventArgs = null;
		cache.OnCacheLineLoaded += (_, args) => eventArgs = args;

		// Load a cache line
		const int INDEX = 1;
		const int ADDRESS = INDEX * CACHE_LINE_SIZE;
		var cacheLine = new Mock<ICacheLine>();
		cacheLine.SetupGet(line => line.StartingAddress)
			.Returns(ADDRESS);
		cacheLine.SetupGet(line => line.EndingAddress)
			.Returns(ADDRESS + CACHE_LINE_SIZE);
		cacheLine.SetupGet(line => line.Size)
			.Returns(CACHE_LINE_SIZE);
		placementPolicy.Setup(policy => policy.GetIndices(cacheLine.Object))
			.Returns(new List<int> { INDEX });

		cache.LoadCacheLine(cacheLine.Object);

		// Check that the event was triggered
		Assert.NotNull(eventArgs);
		Assert.Equal(INDEX, eventArgs.Index);
		Assert.Equal(ADDRESS, eventArgs.Address);
		Assert.Equal(CACHE_LINE_SIZE, eventArgs.Size);
	}

	[Fact]
	public void IsPresentReturnsTrueOnceCacheLineLoaded()
	{
		// Set up the cache
		const int CACHE_SIZE = 4;
		const int CACHE_LINE_SIZE = 8;
		var placementPolicy = new Mock<IPlacementPolicy>();
		var evictionPolicy = new Mock<IEvictionPolicy>();
		var cache = new ModularCache(
			CACHE_SIZE,
			CACHE_LINE_SIZE,
			placementPolicy.Object,
			evictionPolicy.Object
		);

		// Load a cache line
		const int INDEX = 1;
		const int ADDRESS = INDEX * CACHE_LINE_SIZE;
		static bool IsInCacheLine(int address) => address switch
		{
			< ADDRESS => false,
			>= ADDRESS + CACHE_LINE_SIZE => false,
			_ => true
		};
		var cacheLine = new Mock<ICacheLine>();
		cacheLine.SetupGet(line => line.StartingAddress)
			.Returns(ADDRESS);
		cacheLine.SetupGet(line => line.EndingAddress)
			.Returns(ADDRESS + CACHE_LINE_SIZE);
		cacheLine.SetupGet(line => line.Size)
			.Returns(CACHE_LINE_SIZE);
		cacheLine.Setup(line => line.Contains(It.IsAny<int>()))
			.Returns<int>(IsInCacheLine);
		placementPolicy.Setup(policy => policy.GetIndices(cacheLine.Object))
			.Returns(new List<int> { INDEX });

		cache.LoadCacheLine(cacheLine.Object);

		// Make sure that addresses outside the cache line are not reported
		//   as loaded and that addresses inside the cache line are reported
		//   as loaded
		for (var i = 0; i < CACHE_SIZE * CACHE_LINE_SIZE * 2; i++)
		{
			Assert.Equal(IsInCacheLine(i), cache.IsPresent(i));
		}
	}

	[Fact]
	public void GetLoadedCacheLine()
	{
		// Set up the cache
		const int CACHE_SIZE = 4;
		const int CACHE_LINE_SIZE = 8;
		var placementPolicy = new Mock<IPlacementPolicy>();
		var evictionPolicy = new Mock<IEvictionPolicy>();
		var cache = new ModularCache(
			CACHE_SIZE,
			CACHE_LINE_SIZE,
			placementPolicy.Object,
			evictionPolicy.Object
		);

		// Load a cache line
		const int INDEX = 1;
		const int ADDRESS = INDEX * CACHE_LINE_SIZE;
		var cacheLine = new Mock<ICacheLine>();
		cacheLine.SetupGet(line => line.StartingAddress)
			.Returns(ADDRESS);
		cacheLine.SetupGet(line => line.EndingAddress)
			.Returns(ADDRESS + CACHE_LINE_SIZE);
		cacheLine.SetupGet(line => line.Size)
			.Returns(CACHE_LINE_SIZE);
		cacheLine.Setup(line => line.Contains(It.IsAny<int>()))
			.Returns<int>(address => address switch
			{
				< ADDRESS => false,
				>= ADDRESS + CACHE_LINE_SIZE => false,
				_ => true
			});
		placementPolicy.Setup(policy => policy.GetIndices(cacheLine.Object))
			.Returns(new List<int> { INDEX });
		cache.LoadCacheLine(cacheLine.Object);

		// Make sure the cache line can be obtained using any address in the
		//   cache line
		for (var i = 0; i < CACHE_LINE_SIZE; i++)
		{
			Assert.Equal(
				cacheLine.Object,
				cache.GetCacheLine(ADDRESS + i)
			);
		}
	}

	[Fact]
	public void CacheLineEvictedIfCacheFull()
	{
		// Set up the cache
		const int CACHE_SIZE = 4;
		const int CACHE_LINE_SIZE = 8;
		var placementPolicy = new Mock<IPlacementPolicy>();
		var evictionPolicy = new Mock<IEvictionPolicy>();
		var cache = new ModularCache(
			CACHE_SIZE,
			CACHE_LINE_SIZE,
			placementPolicy.Object,
			evictionPolicy.Object
		);

		// Bind to the cache line evicted event
		OnCacheLineEvictedEventArgs? eventArgs = null;
		cache.OnCacheLineEvicted += (_, args) => eventArgs = args;

		// Load a cache line
		const int INDEX = 1;
		const int ADDRESS = INDEX * CACHE_LINE_SIZE;
		placementPolicy.Setup(policy => policy.GetIndices(It.IsAny<ICacheLine>()))
			.Returns(new List<int> { INDEX });
		evictionPolicy.Setup(
			policy => policy.GetIndexToEvict(It.IsAny<IReadOnlyList<int>>())
		).Returns(INDEX);

		var evictedCacheLine = new Mock<ICacheLine>();
		evictedCacheLine.SetupGet(line => line.StartingAddress)
			.Returns(ADDRESS);
		evictedCacheLine.SetupGet(line => line.EndingAddress)
			.Returns(ADDRESS + CACHE_LINE_SIZE);
		evictedCacheLine.SetupGet(line => line.Size)
			.Returns(CACHE_LINE_SIZE);
		cache.LoadCacheLine(evictedCacheLine.Object);

		// Load another cache line
		// Since the placement policy will map the new cache line to the same
		//   index as the previous cache line, the previous cache line should
		//   be evicted
		var cacheLine = new Mock<ICacheLine>();
		cacheLine.SetupGet(line => line.StartingAddress)
			.Returns(ADDRESS + CACHE_LINE_SIZE);
		cacheLine.SetupGet(line => line.EndingAddress)
			.Returns(ADDRESS + (CACHE_LINE_SIZE * 2));
		cacheLine.SetupGet(line => line.Size)
			.Returns(CACHE_LINE_SIZE);
		cache.LoadCacheLine(cacheLine.Object);

		// Check that the event was triggered and that the data matches the
		//   data for the first cache line
		Assert.NotNull(eventArgs);
		Assert.Equal(INDEX, eventArgs.Index);
		Assert.Equal(ADDRESS, eventArgs.Address);
		Assert.Equal(CACHE_LINE_SIZE, eventArgs.Size);
	}

	[Fact]
	public void EvictionPolicyNotifiedWhenCacheLineAccessed()
	{
		// Set up the cache
		const int CACHE_SIZE = 4;
		const int CACHE_LINE_SIZE = 8;
		var memory = new Mock<IMemory>();
		var placementPolicy = new Mock<IPlacementPolicy>();
		var evictionPolicy = new Mock<IEvictionPolicy>();
		var cache = new ModularCache(
			CACHE_SIZE,
			CACHE_LINE_SIZE,
			placementPolicy.Object,
			evictionPolicy.Object
		);

		// Load a cache line
		const int INDEX = 1;
		const int ADDRESS = INDEX * CACHE_LINE_SIZE;
		placementPolicy.Setup(policy => policy.GetIndices(It.IsAny<ICacheLine>()))
			.Returns(new List<int> { INDEX });
		cache.LoadCacheLine(
			new WriteThroughCacheLine(
				memory.Object,
				ADDRESS,
				CACHE_LINE_SIZE
			)
		);

		// Access the cache line
		var cacheLine = cache.GetCacheLine(ADDRESS);
		cacheLine.Read(ADDRESS);

		// Check that the eviction policy was notified
		evictionPolicy.Verify(
			policy => policy.OnCacheLineAccessed(INDEX),
			Times.Once
		);
	}

	[Fact]
	public void GetCacheLineThrowsOnInvalidAddress()
	{
		const int CACHE_SIZE = 4;
		const int CACHE_LINE_SIZE = 8;
		var placementPolicy = new Mock<IPlacementPolicy>();
		var evictionPolicy = new Mock<IEvictionPolicy>();
		var cache = new ModularCache(
			CACHE_SIZE,
			CACHE_LINE_SIZE,
			placementPolicy.Object,
			evictionPolicy.Object
		);

		Assert.Throws<ArgumentOutOfRangeException>(
			() => cache.GetCacheLine(-1)
		);
	}
}
