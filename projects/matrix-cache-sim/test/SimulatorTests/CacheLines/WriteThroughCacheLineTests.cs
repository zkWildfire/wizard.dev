/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.CacheLines;
using Mcs.Simulator.Events;
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

	[Fact]
	public void ReadValidMemoryAddress()
	{
		const int ADDRESS = 1;
		const int VALUE = 2;
		var mockMemory = new Mock<IMemory>();
		mockMemory.Setup(x => x.Read(ADDRESS))
			.Returns(VALUE);

		var cacheLine = new WriteThroughCacheLine(mockMemory.Object, 0, 4);
		Assert.Equal(VALUE, cacheLine.Read(ADDRESS));
	}

	[Fact]
	public void ReadValidMemoryAddressEmitsAccessedEvent()
	{
		const int ADDRESS = 1;
		const int VALUE = 2;
		var mockMemory = new Mock<IMemory>();
		mockMemory.Setup(x => x.Read(ADDRESS))
			.Returns(VALUE);

		var cacheLine = new WriteThroughCacheLine(mockMemory.Object, 0, 4);
		OnCacheLineAccessedEventArgs? args = null;
		cacheLine.OnCacheLineAccessed += (_, a) => args = a;

		cacheLine.Read(ADDRESS);
		Assert.NotNull(args);
		Assert.Equal(ADDRESS, args.Address);
		Assert.True(args.IsRead);
		Assert.Equal(VALUE, args.OldValue);
		Assert.Equal(VALUE, args.NewValue);
	}

	[Fact]
	public void ReadInvalidMemoryAddressThrows()
	{
		var mockMemory = new Mock<IMemory>();
		var cacheLine = new WriteThroughCacheLine(mockMemory.Object, 0, 4);

		Assert.Throws<ArgumentOutOfRangeException>(
			() => cacheLine.Read(-1));
		Assert.Throws<ArgumentOutOfRangeException>(
			() => cacheLine.Read(123));
		mockMemory.Verify(x => x.Read(It.IsAny<int>()), Times.Never);
	}

	[Fact]
	public void WriteValidMemoryAddress()
	{
		const int ADDRESS = 1;
		const int VALUE = 2;
		var mockMemory = new Mock<IMemory>();
		mockMemory.Setup(x => x.Write(ADDRESS, VALUE));

		var cacheLine = new WriteThroughCacheLine(mockMemory.Object, 0, 4);
		cacheLine.Write(ADDRESS, VALUE);
		mockMemory.Verify(x => x.Write(ADDRESS, VALUE), Times.Once);
	}

	[Fact]
	public void WriteValidMemoryAddressEmitsAccessedEvent()
	{
		const int ADDRESS = 1;
		const int VALUE = 2;
		var mockMemory = new Mock<IMemory>();
		mockMemory.Setup(x => x.Read(ADDRESS))
			.Returns(VALUE);

		var cacheLine = new WriteThroughCacheLine(mockMemory.Object, 0, 4);
		OnCacheLineAccessedEventArgs? args = null;
		cacheLine.OnCacheLineAccessed += (_, a) => args = a;

		cacheLine.Write(ADDRESS, VALUE);
		Assert.NotNull(args);
		Assert.Equal(ADDRESS, args.Address);
		Assert.False(args.IsRead);
		Assert.Equal(VALUE, args.OldValue);
		Assert.Equal(VALUE, args.NewValue);
	}

	[Fact]
	public void WriteInvalidMemoryAddressThrows()
	{
		var mockMemory = new Mock<IMemory>();
		var cacheLine = new WriteThroughCacheLine(mockMemory.Object, 0, 4);

		Assert.Throws<ArgumentOutOfRangeException>(
			() => cacheLine.Write(-1, 0));
		Assert.Throws<ArgumentOutOfRangeException>(
			() => cacheLine.Write(123, 0));
		mockMemory.Verify(x => x.Write(
			It.IsAny<int>(),
			It.IsAny<int>()),
			Times.Never
		);
	}

	[Fact]
	public void CodeCoverageOnly()
	{
		var memory = Mock.Of<IMemory>();
		var cacheLine = new WriteThroughCacheLine(memory, 0, 4);

		// This method does nothing
		cacheLine.Flush();
	}
}
