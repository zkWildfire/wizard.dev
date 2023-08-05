/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Common.Simulation;
using Mcs.Simulator.CacheLines;
using Mcs.Simulator.Caches;
using Mcs.Simulator.Events;
using Mcs.Simulator.Memory;
using Mcs.Simulator.Policies.Eviction;
using Mcs.Simulator.Policies.Placement;
using Mcs.Simulator.Simulation;
using Mcs.Simulator.Validators;
using Mcs.Simulator;
namespace McsTests.Simulator;

public class ModularSimulatorTests
{
	[Fact]
	public void ReadValidAddresses()
	{
		const int MATRIX_X = 4;
		const int MATRIX_Y = 4;
		const int CACHE_SIZE = 4;
		const int CACHE_LINE_SIZE = 8;
		const int STARTING_OFFSET = CACHE_LINE_SIZE;

		// Set up the simulator components
		var memory = new Mock<IMemory>();
		memory.SetupGet(m => m.Size)
			.Returns((MATRIX_X * MATRIX_Y) + STARTING_OFFSET);
		memory.Setup(m => m.Read(It.IsAny<int>()))
			.Returns((int address) => address);

		var placementPolicy = new FullyAssociativePlacementPolicy(CACHE_SIZE);
		var evictionPolicy = new LeastRecentlyUsedEvictionPolicy(CACHE_SIZE);
		var validator = new Mock<IMemoryValidator>();
		var cache = new ModularCache(
			CACHE_SIZE,
			CACHE_LINE_SIZE,
			placementPolicy,
			evictionPolicy
		);
		var cacheLineFactory = new WriteThroughCacheLineFactory(CACHE_LINE_SIZE);
		var matrix = new RowMajorMatrix(
			memory.Object,
			MATRIX_X,
			MATRIX_Y,
			STARTING_OFFSET
		);

		// Create the simulator and run the test
		var simulator = new ModularSimulator(
			memory.Object,
			cache,
			cacheLineFactory,
			validator.Object,
			matrix
		);
		for (var i = STARTING_OFFSET; i < memory.Object.Size; i++)
		{
			Assert.Equal(i, simulator.Read(i));
		}
	}

	[Fact]
	public void ReadingInvalidAddressThrows()
	{
		const int MATRIX_X = 4;
		const int MATRIX_Y = 4;
		const int CACHE_SIZE = 4;
		const int CACHE_LINE_SIZE = 8;
		const int STARTING_OFFSET = CACHE_LINE_SIZE;

		// Set up the simulator components
		var memory = new Mock<IMemory>();
		memory.SetupGet(m => m.Size)
			.Returns((MATRIX_X * MATRIX_Y) + STARTING_OFFSET);
		memory.Setup(m => m.Read(It.IsAny<int>()))
			.Returns((int address) => address);

		var placementPolicy = new FullyAssociativePlacementPolicy(CACHE_SIZE);
		var evictionPolicy = new LeastRecentlyUsedEvictionPolicy(CACHE_SIZE);
		var validator = new Mock<IMemoryValidator>();
		var cache = new ModularCache(
			CACHE_SIZE,
			CACHE_LINE_SIZE,
			placementPolicy,
			evictionPolicy
		);
		var cacheLineFactory = new WriteThroughCacheLineFactory(CACHE_LINE_SIZE);
		var matrix = new RowMajorMatrix(
			memory.Object,
			MATRIX_X,
			MATRIX_Y,
			STARTING_OFFSET
		);

		// Create the simulator and run the test
		var simulator = new ModularSimulator(
			memory.Object,
			cache,
			cacheLineFactory,
			validator.Object,
			matrix
		);

		// This should throw because it's not a valid memory address
		Assert.Throws<ArgumentOutOfRangeException>(() => simulator.Read(-1));

		// This should throw because it's outside the memory region that the
		//   matrix occupies
		Assert.Throws<ArgumentOutOfRangeException>(() => simulator.Read(0));
	}

	[Fact]
	public void ReadingUnloadedMemoryAddressTriggersCacheLineLoadEvent()
	{
		const int MATRIX_X = 4;
		const int MATRIX_Y = 4;
		const int CACHE_SIZE = 4;
		const int CACHE_LINE_SIZE = 8;
		const int STARTING_OFFSET = CACHE_LINE_SIZE;

		// Set up the simulator components
		var memory = new Mock<IMemory>();
		memory.SetupGet(m => m.Size)
			.Returns((MATRIX_X * MATRIX_Y) + STARTING_OFFSET);
		memory.Setup(m => m.Read(It.IsAny<int>()))
			.Returns((int address) => address);

		const int INDEX = 1;
		var placementPolicy = new Mock<IPlacementPolicy>();
		placementPolicy.Setup(p => p.GetIndices(It.IsAny<ICacheLine>()))
			.Returns(new List<int> { INDEX });

		var evictionPolicy = new LeastRecentlyUsedEvictionPolicy(CACHE_SIZE);
		var validator = new Mock<IMemoryValidator>();
		var cache = new ModularCache(
			CACHE_SIZE,
			CACHE_LINE_SIZE,
			placementPolicy.Object,
			evictionPolicy
		);
		var cacheLineFactory = new WriteThroughCacheLineFactory(CACHE_LINE_SIZE);
		var matrix = new RowMajorMatrix(
			memory.Object,
			MATRIX_X,
			MATRIX_Y,
			STARTING_OFFSET
		);

		// Create the simulator and run the test
		var simulator = new ModularSimulator(
			memory.Object,
			cache,
			cacheLineFactory,
			validator.Object,
			matrix
		);
		OnCacheLineLoadedEventArgs? args = null;
		simulator.OnCacheLineLoaded += (sender, e) => args = e;
		simulator.Read(STARTING_OFFSET);

		Assert.NotNull(args);
		Assert.Equal(INDEX, args.Index);
		Assert.Equal(STARTING_OFFSET, args.Address);
		Assert.Equal(CACHE_LINE_SIZE, args.Size);
	}

	[Fact]
	public void ReadingUnloadedMemoryAddressTriggersMemoryAccessEvent()
	{
		const int MATRIX_X = 4;
		const int MATRIX_Y = 4;
		const int CACHE_SIZE = 4;
		const int CACHE_LINE_SIZE = 8;
		const int STARTING_OFFSET = CACHE_LINE_SIZE;

		// Set up the simulator components
		var memory = new Mock<IMemory>();
		memory.SetupGet(m => m.Size)
			.Returns((MATRIX_X * MATRIX_Y) + STARTING_OFFSET);
		memory.Setup(m => m.Read(It.IsAny<int>()))
			.Returns((int address) => address);

		const int INDEX = 1;
		var placementPolicy = new Mock<IPlacementPolicy>();
		placementPolicy.Setup(p => p.GetIndices(It.IsAny<ICacheLine>()))
			.Returns(new List<int> { INDEX });

		var evictionPolicy = new LeastRecentlyUsedEvictionPolicy(CACHE_SIZE);
		var validator = new Mock<IMemoryValidator>();
		var cache = new ModularCache(
			CACHE_SIZE,
			CACHE_LINE_SIZE,
			placementPolicy.Object,
			evictionPolicy
		);
		var cacheLineFactory = new WriteThroughCacheLineFactory(CACHE_LINE_SIZE);
		var matrix = new RowMajorMatrix(
			memory.Object,
			MATRIX_X,
			MATRIX_Y,
			STARTING_OFFSET
		);

		// Create the simulator and run the test
		var simulator = new ModularSimulator(
			memory.Object,
			cache,
			cacheLineFactory,
			validator.Object,
			matrix
		);
		OnMemoryAccessedEventArgs? args = null;
		simulator.OnMemoryAccess += (sender, e) => args = e;
		var address = STARTING_OFFSET + 1;
		var (x, y) = matrix.ToMatrixCoordinate(address);
		simulator.Read(address);

		Assert.NotNull(args);
		Assert.Equal(address, args.Address);
		Assert.Equal(x, args.X);
		Assert.Equal(y, args.Y);
		Assert.True(args.IsRead);
		Assert.False(args.IsCacheHit);
		Assert.Equal(address, args.NewValue);
		Assert.Equal(address, args.OldValue);
	}

	[Fact]
	public void ReadingLoadedMemoryAddressTriggersMemoryAccessEvent()
	{
		const int MATRIX_X = 4;
		const int MATRIX_Y = 4;
		const int CACHE_SIZE = 4;
		const int CACHE_LINE_SIZE = 8;
		const int STARTING_OFFSET = CACHE_LINE_SIZE;

		// Set up the simulator components
		var memory = new Mock<IMemory>();
		memory.SetupGet(m => m.Size)
			.Returns((MATRIX_X * MATRIX_Y) + STARTING_OFFSET);
		memory.Setup(m => m.Read(It.IsAny<int>()))
			.Returns((int address) => address);

		const int INDEX = 1;
		var placementPolicy = new Mock<IPlacementPolicy>();
		placementPolicy.Setup(p => p.GetIndices(It.IsAny<ICacheLine>()))
			.Returns(new List<int> { INDEX });

		var evictionPolicy = new LeastRecentlyUsedEvictionPolicy(CACHE_SIZE);
		var validator = new Mock<IMemoryValidator>();
		var cache = new ModularCache(
			CACHE_SIZE,
			CACHE_LINE_SIZE,
			placementPolicy.Object,
			evictionPolicy
		);
		var cacheLineFactory = new WriteThroughCacheLineFactory(CACHE_LINE_SIZE);
		var matrix = new RowMajorMatrix(
			memory.Object,
			MATRIX_X,
			MATRIX_Y,
			STARTING_OFFSET
		);

		// Create the simulator and run the test
		var simulator = new ModularSimulator(
			memory.Object,
			cache,
			cacheLineFactory,
			validator.Object,
			matrix
		);

		// Load the memory address into the cache before running the test
		var address = STARTING_OFFSET + 1;
		var (x, y) = matrix.ToMatrixCoordinate(address);
		simulator.Read(address);

		OnMemoryAccessedEventArgs? args = null;
		simulator.OnMemoryAccess += (sender, e) => args = e;
		simulator.Read(address);

		Assert.NotNull(args);
		Assert.Equal(address, args.Address);
		Assert.Equal(x, args.X);
		Assert.Equal(y, args.Y);
		Assert.True(args.IsRead);
		Assert.True(args.IsCacheHit);
		Assert.Equal(address, args.NewValue);
		Assert.Equal(address, args.OldValue);
	}

	[Fact]
	public void ReadingNewMemoryLocationTriggersCacheLineEvictedEvent()
	{
		const int MATRIX_X = 4;
		const int MATRIX_Y = 4;
		const int CACHE_SIZE = 4;
		const int CACHE_LINE_SIZE = 8;
		const int STARTING_OFFSET = CACHE_LINE_SIZE;

		// Set up the simulator components
		var memory = new Mock<IMemory>();
		memory.SetupGet(m => m.Size)
			.Returns((MATRIX_X * MATRIX_Y) + STARTING_OFFSET);
		memory.Setup(m => m.Read(It.IsAny<int>()))
			.Returns((int address) => address);

		const int INDEX = 1;
		var placementPolicy = new Mock<IPlacementPolicy>();
		placementPolicy.Setup(p => p.GetIndices(It.IsAny<ICacheLine>()))
			.Returns(new List<int> { INDEX });

		var evictionPolicy = new LeastRecentlyUsedEvictionPolicy(CACHE_SIZE);
		var validator = new Mock<IMemoryValidator>();
		var cache = new ModularCache(
			CACHE_SIZE,
			CACHE_LINE_SIZE,
			placementPolicy.Object,
			evictionPolicy
		);
		var cacheLineFactory = new WriteThroughCacheLineFactory(CACHE_LINE_SIZE);
		var matrix = new RowMajorMatrix(
			memory.Object,
			MATRIX_X,
			MATRIX_Y,
			STARTING_OFFSET
		);

		// Create the simulator and run the test
		var simulator = new ModularSimulator(
			memory.Object,
			cache,
			cacheLineFactory,
			validator.Object,
			matrix
		);

		// Load a cache line into memory
		var address = STARTING_OFFSET;
		simulator.Read(address);

		// Access a memory address in a different cache line
		OnCacheLineEvictedEventArgs? args = null;
		simulator.OnCacheLineEvicted += (sender, e) => args = e;
		simulator.Read(address + CACHE_LINE_SIZE);

		Assert.NotNull(args);
		Assert.Equal(INDEX, args.Index);
		Assert.Equal(address, args.Address);
		Assert.Equal(CACHE_LINE_SIZE, args.Size);
	}

	[Fact]
	public void WriteValidAddresses()
	{
		const int MATRIX_X = 4;
		const int MATRIX_Y = 4;
		const int CACHE_SIZE = 4;
		const int CACHE_LINE_SIZE = 8;
		const int STARTING_OFFSET = CACHE_LINE_SIZE;

		// Set up the simulator components
		var memory = new ArrayMemory((MATRIX_X * MATRIX_Y) + STARTING_OFFSET);
		for (var i = STARTING_OFFSET; i < memory.Size; i++)
		{
			memory.Write(i, i);
		}

		var placementPolicy = new FullyAssociativePlacementPolicy(CACHE_SIZE);
		var evictionPolicy = new LeastRecentlyUsedEvictionPolicy(CACHE_SIZE);
		var validator = new Mock<IMemoryValidator>();
		var cache = new ModularCache(
			CACHE_SIZE,
			CACHE_LINE_SIZE,
			placementPolicy,
			evictionPolicy
		);
		var cacheLineFactory = new WriteThroughCacheLineFactory(CACHE_LINE_SIZE);
		var matrix = new RowMajorMatrix(
			memory,
			MATRIX_X,
			MATRIX_Y,
			STARTING_OFFSET
		);

		// Create the simulator and run the test
		var simulator = new ModularSimulator(
			memory,
			cache,
			cacheLineFactory,
			validator.Object,
			matrix
		);
		for (var i = STARTING_OFFSET; i < memory.Size; i++)
		{
			simulator.Write(i, i * 2);
		}
		for (var i = STARTING_OFFSET; i < memory.Size; i++)
		{
			Assert.Equal(i * 2, simulator.Read(i));
		}
	}

	[Fact]
	public void WritingInvalidAddressThrows()
	{
		const int MATRIX_X = 4;
		const int MATRIX_Y = 4;
		const int CACHE_SIZE = 4;
		const int CACHE_LINE_SIZE = 8;
		const int STARTING_OFFSET = CACHE_LINE_SIZE;

		// Set up the simulator components
		var memory = new Mock<IMemory>();
		memory.SetupGet(m => m.Size)
			.Returns((MATRIX_X * MATRIX_Y) + STARTING_OFFSET);
		memory.Setup(m => m.Read(It.IsAny<int>()))
			.Returns((int address) => address);

		var placementPolicy = new FullyAssociativePlacementPolicy(CACHE_SIZE);
		var evictionPolicy = new LeastRecentlyUsedEvictionPolicy(CACHE_SIZE);
		var validator = new Mock<IMemoryValidator>();
		var cache = new ModularCache(
			CACHE_SIZE,
			CACHE_LINE_SIZE,
			placementPolicy,
			evictionPolicy
		);
		var cacheLineFactory = new WriteThroughCacheLineFactory(CACHE_LINE_SIZE);
		var matrix = new RowMajorMatrix(
			memory.Object,
			MATRIX_X,
			MATRIX_Y,
			STARTING_OFFSET
		);

		// Create the simulator and run the test
		var simulator = new ModularSimulator(
			memory.Object,
			cache,
			cacheLineFactory,
			validator.Object,
			matrix
		);

		// This should throw because it's not a valid memory address
		Assert.Throws<ArgumentOutOfRangeException>(() => simulator.Write(-1, 0));

		// This should throw because it's outside the memory region that the
		//   matrix occupies
		Assert.Throws<ArgumentOutOfRangeException>(() => simulator.Write(0, 0));
	}

	[Fact]
	public void WritingUnloadedMemoryAddressTriggersCacheLineLoadEvent()
	{
		const int MATRIX_X = 4;
		const int MATRIX_Y = 4;
		const int CACHE_SIZE = 4;
		const int CACHE_LINE_SIZE = 8;
		const int STARTING_OFFSET = CACHE_LINE_SIZE;

		// Set up the simulator components
		var memory = new Mock<IMemory>();
		memory.SetupGet(m => m.Size)
			.Returns((MATRIX_X * MATRIX_Y) + STARTING_OFFSET);
		memory.Setup(m => m.Read(It.IsAny<int>()))
			.Returns((int address) => address);

		const int INDEX = 1;
		var placementPolicy = new Mock<IPlacementPolicy>();
		placementPolicy.Setup(p => p.GetIndices(It.IsAny<ICacheLine>()))
			.Returns(new List<int> { INDEX });

		var evictionPolicy = new LeastRecentlyUsedEvictionPolicy(CACHE_SIZE);
		var validator = new Mock<IMemoryValidator>();
		var cache = new ModularCache(
			CACHE_SIZE,
			CACHE_LINE_SIZE,
			placementPolicy.Object,
			evictionPolicy
		);
		var cacheLineFactory = new WriteThroughCacheLineFactory(CACHE_LINE_SIZE);
		var matrix = new RowMajorMatrix(
			memory.Object,
			MATRIX_X,
			MATRIX_Y,
			STARTING_OFFSET
		);

		// Create the simulator and run the test
		var simulator = new ModularSimulator(
			memory.Object,
			cache,
			cacheLineFactory,
			validator.Object,
			matrix
		);
		OnCacheLineLoadedEventArgs? args = null;
		simulator.OnCacheLineLoaded += (sender, e) => args = e;
		simulator.Write(STARTING_OFFSET, 0);

		Assert.NotNull(args);
		Assert.Equal(INDEX, args.Index);
		Assert.Equal(STARTING_OFFSET, args.Address);
		Assert.Equal(CACHE_LINE_SIZE, args.Size);
	}

	[Fact]
	public void WritingUnloadedMemoryAddressTriggersMemoryAccessEvent()
	{
		const int MATRIX_X = 4;
		const int MATRIX_Y = 4;
		const int CACHE_SIZE = 4;
		const int CACHE_LINE_SIZE = 8;
		const int STARTING_OFFSET = CACHE_LINE_SIZE;

		// Set up the simulator components
		var memory = new Mock<IMemory>();
		memory.SetupGet(m => m.Size)
			.Returns((MATRIX_X * MATRIX_Y) + STARTING_OFFSET);
		memory.Setup(m => m.Read(It.IsAny<int>()))
			.Returns((int address) => address);

		const int INDEX = 1;
		var placementPolicy = new Mock<IPlacementPolicy>();
		placementPolicy.Setup(p => p.GetIndices(It.IsAny<ICacheLine>()))
			.Returns(new List<int> { INDEX });

		var evictionPolicy = new LeastRecentlyUsedEvictionPolicy(CACHE_SIZE);
		var validator = new Mock<IMemoryValidator>();
		var cache = new ModularCache(
			CACHE_SIZE,
			CACHE_LINE_SIZE,
			placementPolicy.Object,
			evictionPolicy
		);
		var cacheLineFactory = new WriteThroughCacheLineFactory(CACHE_LINE_SIZE);
		var matrix = new RowMajorMatrix(
			memory.Object,
			MATRIX_X,
			MATRIX_Y,
			STARTING_OFFSET
		);

		// Create the simulator and run the test
		var simulator = new ModularSimulator(
			memory.Object,
			cache,
			cacheLineFactory,
			validator.Object,
			matrix
		);
		OnMemoryAccessedEventArgs? args = null;
		simulator.OnMemoryAccess += (sender, e) => args = e;
		var address = STARTING_OFFSET + 1;
		var (x, y) = matrix.ToMatrixCoordinate(address);
		const int NEW_VALUE = 0xF00;
		simulator.Write(address, NEW_VALUE);

		Assert.NotNull(args);
		Assert.Equal(address, args.Address);
		Assert.Equal(x, args.X);
		Assert.Equal(y, args.Y);
		Assert.False(args.IsRead);
		Assert.False(args.IsCacheHit);
		Assert.Equal(NEW_VALUE, args.NewValue);
		Assert.Equal(address, args.OldValue);
	}

	[Fact]
	public void WritingLoadedMemoryAddressTriggersMemoryAccessEvent()
	{
		const int MATRIX_X = 4;
		const int MATRIX_Y = 4;
		const int CACHE_SIZE = 4;
		const int CACHE_LINE_SIZE = 8;
		const int STARTING_OFFSET = CACHE_LINE_SIZE;

		// Set up the simulator components
		var memory = new Mock<IMemory>();
		memory.SetupGet(m => m.Size)
			.Returns((MATRIX_X * MATRIX_Y) + STARTING_OFFSET);
		memory.Setup(m => m.Read(It.IsAny<int>()))
			.Returns((int address) => address);

		const int INDEX = 1;
		var placementPolicy = new Mock<IPlacementPolicy>();
		placementPolicy.Setup(p => p.GetIndices(It.IsAny<ICacheLine>()))
			.Returns(new List<int> { INDEX });

		var evictionPolicy = new LeastRecentlyUsedEvictionPolicy(CACHE_SIZE);
		var validator = new Mock<IMemoryValidator>();
		var cache = new ModularCache(
			CACHE_SIZE,
			CACHE_LINE_SIZE,
			placementPolicy.Object,
			evictionPolicy
		);
		var cacheLineFactory = new WriteThroughCacheLineFactory(CACHE_LINE_SIZE);
		var matrix = new RowMajorMatrix(
			memory.Object,
			MATRIX_X,
			MATRIX_Y,
			STARTING_OFFSET
		);

		// Create the simulator and run the test
		var simulator = new ModularSimulator(
			memory.Object,
			cache,
			cacheLineFactory,
			validator.Object,
			matrix
		);

		// Load the memory address into the cache before running the test
		var address = STARTING_OFFSET + 1;
		var (x, y) = matrix.ToMatrixCoordinate(address);
		simulator.Read(address);

		OnMemoryAccessedEventArgs? args = null;
		simulator.OnMemoryAccess += (sender, e) => args = e;
		const int NEW_VALUE = 0xF00;
		simulator.Write(address, NEW_VALUE);

		Assert.NotNull(args);
		Assert.Equal(address, args.Address);
		Assert.Equal(x, args.X);
		Assert.Equal(y, args.Y);
		Assert.False(args.IsRead);
		Assert.True(args.IsCacheHit);
		Assert.Equal(NEW_VALUE, args.NewValue);
		Assert.Equal(address, args.OldValue);
	}

	[Fact]
	public void WritingNewMemoryLocationTriggersCacheLineEvictedEvent()
	{
		const int MATRIX_X = 4;
		const int MATRIX_Y = 4;
		const int CACHE_SIZE = 4;
		const int CACHE_LINE_SIZE = 8;
		const int STARTING_OFFSET = CACHE_LINE_SIZE;

		// Set up the simulator components
		var memory = new Mock<IMemory>();
		memory.SetupGet(m => m.Size)
			.Returns((MATRIX_X * MATRIX_Y) + STARTING_OFFSET);
		memory.Setup(m => m.Read(It.IsAny<int>()))
			.Returns((int address) => address);

		const int INDEX = 1;
		var placementPolicy = new Mock<IPlacementPolicy>();
		placementPolicy.Setup(p => p.GetIndices(It.IsAny<ICacheLine>()))
			.Returns(new List<int> { INDEX });

		var evictionPolicy = new LeastRecentlyUsedEvictionPolicy(CACHE_SIZE);
		var validator = new Mock<IMemoryValidator>();
		var cache = new ModularCache(
			CACHE_SIZE,
			CACHE_LINE_SIZE,
			placementPolicy.Object,
			evictionPolicy
		);
		var cacheLineFactory = new WriteThroughCacheLineFactory(CACHE_LINE_SIZE);
		var matrix = new RowMajorMatrix(
			memory.Object,
			MATRIX_X,
			MATRIX_Y,
			STARTING_OFFSET
		);

		// Create the simulator and run the test
		var simulator = new ModularSimulator(
			memory.Object,
			cache,
			cacheLineFactory,
			validator.Object,
			matrix
		);

		// Load a cache line into memory
		var address = STARTING_OFFSET;
		simulator.Read(address);

		// Access a memory address in a different cache line
		OnCacheLineEvictedEventArgs? args = null;
		simulator.OnCacheLineEvicted += (sender, e) => args = e;
		simulator.Write(address + CACHE_LINE_SIZE, 0);

		Assert.NotNull(args);
		Assert.Equal(INDEX, args.Index);
		Assert.Equal(address, args.Address);
		Assert.Equal(CACHE_LINE_SIZE, args.Size);
	}

	[Fact]
	public void ValidateCorrectlyTransposedMatrix()
	{
		const int MATRIX_X = 4;
		const int MATRIX_Y = 4;
		const int CACHE_SIZE = 4;
		const int CACHE_LINE_SIZE = 8;
		const int STARTING_OFFSET = CACHE_LINE_SIZE;

		// Create the simulator components
		var memory = new ArrayMemory((MATRIX_X * MATRIX_Y) + STARTING_OFFSET);
		var placementPolicy = new FullyAssociativePlacementPolicy(CACHE_SIZE);
		var evictionPolicy = new LeastRecentlyUsedEvictionPolicy(CACHE_SIZE);
		var validator = new SequentialMemoryValidator();
		var cache = new ModularCache(
			CACHE_SIZE,
			CACHE_LINE_SIZE,
			placementPolicy,
			evictionPolicy
		);
		var cacheLineFactory = new WriteThroughCacheLineFactory(CACHE_LINE_SIZE);
		var matrix = new RowMajorMatrix(
			memory,
			MATRIX_X,
			MATRIX_Y,
			STARTING_OFFSET
		);

		// Create the simulator and run the test
		var simulator = new ModularSimulator(
			memory,
			cache,
			cacheLineFactory,
			validator,
			matrix
		);
		for (var x = 0; x < MATRIX_X; x++)
		{
			for (var y = 0; y < MATRIX_Y; y++)
			{
				if (x > y)
				{
					continue;
				}

				var a1 = matrix.ToMemoryAddress(x, y);
				var a2 = matrix.ToMemoryAddress(y, x);
				var v1 = simulator.Read(a1);
				var v2 = simulator.Read(a2);
				simulator.Write(a1, v2);
				simulator.Write(a2, v1);
			}
		}

		// Validate the matrix
		Assert.True(simulator.Validate());
	}

	[Fact]
	public void ValidateIncorrectlyTransposedMatrix()
	{
		const int MATRIX_X = 4;
		const int MATRIX_Y = 4;
		const int CACHE_SIZE = 4;
		const int CACHE_LINE_SIZE = 8;
		const int STARTING_OFFSET = CACHE_LINE_SIZE;

		// Create the simulator components
		var memory = new ArrayMemory((MATRIX_X * MATRIX_Y) + STARTING_OFFSET);
		var placementPolicy = new FullyAssociativePlacementPolicy(CACHE_SIZE);
		var evictionPolicy = new LeastRecentlyUsedEvictionPolicy(CACHE_SIZE);
		var validator = new SequentialMemoryValidator();
		var cache = new ModularCache(
			CACHE_SIZE,
			CACHE_LINE_SIZE,
			placementPolicy,
			evictionPolicy
		);
		var cacheLineFactory = new WriteThroughCacheLineFactory(CACHE_LINE_SIZE);
		var matrix = new RowMajorMatrix(
			memory,
			MATRIX_X,
			MATRIX_Y,
			STARTING_OFFSET
		);

		// Create the simulator but don't modify the matrix
		var simulator = new ModularSimulator(
			memory,
			cache,
			cacheLineFactory,
			validator,
			matrix
		);

		// Validate the matrix
		Assert.False(simulator.Validate());
	}
}
