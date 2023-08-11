/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.Policies.Eviction;
namespace McsTests.Simulator.Policies.Eviction;

public class LeastRecentlyUsedEvictionFactoryTests
{
	[Fact]
	public void PolicyNameIsNotEmpty()
	{
		var factory = new LeastRecentlyUsedEvictionPolicyFactory();
		Assert.NotEmpty(factory.PolicyName);
	}

	[Fact]
	public void PolicyPropertiesMatchFactoryParameters()
	{
		const int CACHE_SIZE = 4;
		var factory = new LeastRecentlyUsedEvictionPolicyFactory();
		var policy = factory.Construct(CACHE_SIZE);

		Assert.IsType<LeastRecentlyUsedEvictionPolicy>(policy);
		var p = (LeastRecentlyUsedEvictionPolicy)policy;
		Assert.Equal(CACHE_SIZE, p.CacheSize);
	}
}
