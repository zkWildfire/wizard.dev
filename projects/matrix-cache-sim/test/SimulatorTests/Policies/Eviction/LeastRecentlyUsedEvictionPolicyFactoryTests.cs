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
	public void CodeCoverageOnly()
	{
		var factory = new LeastRecentlyUsedEvictionPolicyFactory();
		Assert.NotNull(factory.Construct(1));
	}
}
