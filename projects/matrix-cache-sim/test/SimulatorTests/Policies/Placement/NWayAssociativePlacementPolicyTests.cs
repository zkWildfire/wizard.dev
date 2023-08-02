/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.CacheLines;
using Mcs.Simulator.Policies.Placement;
namespace McsTests.Simulator.Policies.Placement;

public class NWayAssociativePlacementPolicyTests
{
	[Theory]
	[InlineData(1)]
	[InlineData(2)]
	[InlineData(4)]
	[InlineData(8)]
	[InlineData(16)]
	public void CacheLineMapping(int associativity)
	{
		var cacheSize = 16;
		var cacheLineSize = 8;
		var policy = new NWayAssociativePlacementPolicy(
			cacheSize,
			cacheLineSize,
			associativity
		);

		// Use a separate counter for determining the expected index returned
		//   by the policy instead of re-implementing logic that the policy
		//   should contain
		var counter = 0;
		for (var i = 0; i < cacheSize * cacheLineSize * 2; i += cacheLineSize)
		{
			var mockCacheLine = new Mock<ICacheLine>();
			mockCacheLine.SetupGet(x => x.StartingAddress)
				.Returns(i);
			mockCacheLine.SetupGet(x => x.EndingAddress)
				.Returns(i + cacheLineSize);
			mockCacheLine.SetupGet(x => x.Size)
				.Returns(cacheLineSize);

			var indices = policy.GetIndices(mockCacheLine.Object);
			Assert.Equal(associativity, indices.Count);

			for (var j = 0; j < associativity; j++)
			{
				var baseIndex = counter % cacheSize;
				var offset = j * (cacheSize / associativity);
				Assert.Contains((baseIndex + offset) % cacheSize, indices);
			}
			counter++;
		}
	}

	[Fact]
	public void CtorThrowsIfAssociativityIsNotMultipleOfCacheSize()
	{
		var cacheSize = 16;
		var cacheLineSize = 8;
		var associativity = 3;

		Assert.Throws<ArgumentException>(() =>
		{
			var policy = new NWayAssociativePlacementPolicy(
				cacheSize,
				cacheLineSize,
				associativity
			);
		});
	}
}
