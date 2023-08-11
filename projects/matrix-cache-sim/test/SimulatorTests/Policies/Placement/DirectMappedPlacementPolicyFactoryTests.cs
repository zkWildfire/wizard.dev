/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.Policies.Placement;
namespace McsTests.Simulator.Policies.Placement;

public class DirectMappedPlacementPolicyFactoryTests
{
	[Fact]
	public void PolicyNameIsNotEmpty()
	{
		var factory = new DirectMappedPlacementPolicyFactory();
		Assert.NotEmpty(factory.PolicyName);
	}

	[Fact]
	public void PolicyPropertiesMatchFactoryParameters()
	{
		const int CACHE_SIZE = 4;
		const int CACHE_LINE_SIZE = 8;
		var factory = new DirectMappedPlacementPolicyFactory();
		var policy = factory.Construct(CACHE_SIZE, CACHE_LINE_SIZE);

		Assert.IsType<DirectMappedPlacementPolicy>(policy);
		var p = (DirectMappedPlacementPolicy)policy;
		Assert.Equal(CACHE_SIZE, p.CacheSize);
		Assert.Equal(CACHE_LINE_SIZE, p.CacheLineSize);
	}
}
