/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.CacheLines;
using Mcs.Simulator.Memory;
namespace McsTests.Simulator.CacheLines;

public class WriteThroughCacheLineFactoryTests
{
	[Fact]
	public void CacheLineSizeMatchesFactoryArg()
	{
		const int SIZE = 4;
		var factory = new WriteThroughCacheLineFactory(SIZE);
		Assert.Equal(
			SIZE,
			factory.Construct(Mock.Of<IMemory>(), SIZE * 2).Size
		);
	}

	[Fact]
	public void MisalignedStartingAddressThrows()
	{
		const int SIZE = 4;
		var factory = new WriteThroughCacheLineFactory(SIZE);
		Assert.Throws<ArgumentException>(
			() => factory.Construct(Mock.Of<IMemory>(), SIZE + 1)
		);
	}
}
