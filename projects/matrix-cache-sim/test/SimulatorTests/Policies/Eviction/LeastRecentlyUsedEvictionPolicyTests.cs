/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.Policies.Eviction;
namespace McsTests.Simulator.Policies.Eviction;

public class LeastRecentlyUsedEvictionPolicyTests
{
	[Fact]
	public void EvictOldestCacheLineInOrderOfLoading()
	{
		const int COUNT = 3;
		var policy = new LeastRecentlyUsedEvictionPolicy(COUNT);
		for (var i = COUNT; i > 0; i--)
		{
			policy.OnCacheLineLoaded(i - 1);
		}

		// The first cache line to be added should be the one evicted
		var cacheLines = Enumerable.Range(0, COUNT).ToList();
		var cacheLineToEvict = policy.GetIndexToEvict(cacheLines);
		Assert.Equal(COUNT - 1, cacheLineToEvict);
	}

	[Fact]
	public void AccessingCacheLineResetsOrderOfEviction()
	{
		const int COUNT = 3;
		var policy = new LeastRecentlyUsedEvictionPolicy(COUNT);
		for (var i = COUNT; i > 0; i--)
		{
			policy.OnCacheLineLoaded(i - 1);
		}

		// Access the current oldest cache line
		policy.OnCacheLineAccessed(COUNT - 1);

		// Since the oldest cache line was accessed, the next oldest should be
		//   evicted
		var cacheLines = Enumerable.Range(0, COUNT).ToList();
		var cacheLineToEvict = policy.GetIndexToEvict(cacheLines);
		Assert.Equal(COUNT - 2, cacheLineToEvict);
	}

	[Fact]
	public void EvictedCacheLineNotEligibleToBeEvicted()
	{
		const int COUNT = 3;
		var policy = new LeastRecentlyUsedEvictionPolicy(COUNT);
		for (var i = COUNT; i > 0; i--)
		{
			policy.OnCacheLineLoaded(i - 1);
		}
		policy.OnCacheLineEvicted(COUNT - 1);

		// Since the oldest cache line was removed, the next oldest should be
		//   evicted
		var cacheLines = Enumerable.Range(0, COUNT).ToList();
		var cacheLineToEvict = policy.GetIndexToEvict(cacheLines);
		Assert.Equal(COUNT - 2, cacheLineToEvict);
	}
}
