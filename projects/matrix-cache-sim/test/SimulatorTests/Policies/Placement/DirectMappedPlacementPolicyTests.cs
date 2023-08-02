/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.CacheLines;
using Mcs.Simulator.Policies.Placement;
namespace McsTests.Simulator.Policies.Placement;

public class DirectMappedPlacementPolicyTests
{
	[Fact]
	public void AllCacheLinesMapToSingleCacheIndex()
	{
		var cacheSize = 4;
		var cacheLineSize = 8;
		var policy = new DirectMappedPlacementPolicy(cacheSize, cacheLineSize);

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
			Assert.Single(indices);
			Assert.Equal(counter % cacheSize, indices[0]);
			counter++;
		}
	}
}
