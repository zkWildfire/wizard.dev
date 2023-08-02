/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.CacheLines;
using Mcs.Simulator.Policies.Placement;
namespace McsTests.Simulator.Policies.Placement;

public class FullyAssociativePlacementPolicyTests
{
	[Fact]
	public void AllCacheLinesMapToEntireCache()
	{
		var cacheSize = 4;
		var cacheLineSize = 8;
		var policy = new FullyAssociativePlacementPolicy(cacheSize);
		var cacheIndices = Enumerable.Range(0, cacheSize).ToList();

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
			Assert.Equal(cacheSize, indices.Count);
			Assert.Equal(cacheIndices, indices);
		}
	}
}
