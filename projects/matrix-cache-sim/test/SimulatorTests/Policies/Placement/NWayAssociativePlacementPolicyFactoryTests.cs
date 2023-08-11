/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.Policies.Placement;
namespace McsTests.Simulator.Policies.Placement;

public class NWayAssociativePlacementPolicyFactoryTests
{
	[Fact]
	public void PolicyNameIsNotEmpty()
	{
		var factory = new NWayAssociativePlacementPolicyFactory(1);
		Assert.NotEmpty(factory.PolicyName);
	}

	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(4)]
	public void PolicyPropertiesMatchFactoryParameters(int associativity)
	{
		const int CACHE_SIZE = 4;
		const int CACHE_LINE_SIZE = 8;
		var factory = new NWayAssociativePlacementPolicyFactory(associativity);
		var policy = factory.Construct(CACHE_SIZE, CACHE_LINE_SIZE);

		Assert.IsType<NWayAssociativePlacementPolicy>(policy);
		var p = (NWayAssociativePlacementPolicy)policy;
		Assert.Equal(associativity, p.Associativity);
		Assert.Equal(CACHE_SIZE, p.CacheSize);
		Assert.Equal(CACHE_LINE_SIZE, p.CacheLineSize);
	}
}
