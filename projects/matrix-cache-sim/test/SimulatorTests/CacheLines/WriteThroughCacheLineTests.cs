/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.CacheLines;
using Mcs.Simulator.Memory;
namespace McsTests.Simulator.CacheLines;

public class WriteThroughCacheLineTests
{
	[Fact]
	public void CtorSetsProperties()
	{
		var memory = Mock.Of<IMemory>();
		var startingAddress = 1;
		var size = 4;

		var cacheLine = new WriteThroughCacheLine(memory, startingAddress, size);
		Assert.Equal(startingAddress, cacheLine.StartingAddress);
		Assert.Equal(startingAddress + size, cacheLine.EndingAddress);
		Assert.Equal(size, cacheLine.Size);
	}

	[Theory]
	[InlineData(0, 4, 0)]
	[InlineData(0, 4, 1)]
	[InlineData(0, 4, 2)]
	[InlineData(0, 4, 3)]
	[InlineData(1, 4, 1)]
	[InlineData(1, 4, 4)]
	[InlineData(3, 8, 3)]
	[InlineData(3, 8, 10)]
	public void AddressContainedInCacheLine(
		int startingAddress, int size, int address)
	{
		var memory = Mock.Of<IMemory>();
		var cacheLine = new WriteThroughCacheLine(memory, startingAddress, size);
		Assert.True(cacheLine.Contains(address));
	}

	[Theory]
	[InlineData(0, 4, -1)]
	[InlineData(0, 4, 4)]
	[InlineData(1, 4, 0)]
	[InlineData(1, 4, 5)]
	[InlineData(0, 8, -1)]
	[InlineData(0, 8, 8)]
	[InlineData(3, 8, 2)]
	[InlineData(3, 8, 12)]
	public void AddressNotContainedInCacheLine(
		int startingAddress, int size, int address)
	{
		var memory = Mock.Of<IMemory>();
		var cacheLine = new WriteThroughCacheLine(memory, startingAddress, size);
		Assert.False(cacheLine.Contains(address));
	}
}
