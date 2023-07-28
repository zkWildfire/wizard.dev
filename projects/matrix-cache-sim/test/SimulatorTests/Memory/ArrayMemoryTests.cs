/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.Memory;
namespace McsTests.Simulator.Memory;

public class ArrayMemoryTests
{
	[Theory]
	[InlineData(1)]
	[InlineData(10)]
	[InlineData(100)]
	public void ReadValidAddresses(int size)
	{
		var memory = new ArrayMemory(size);
		for (var i = 0; i < size; i++)
		{
			// None of the calls to `Read()` should throw since only valid
			//   addresses are being read
			memory.Read(i);
		}
	}

	[Theory]
	[InlineData(10, -1)]
	[InlineData(10, 10)]
	[InlineData(1, 1)]
	public void ReadInvalidAddresses(int size, int address)
	{
		var memory = new ArrayMemory(size);
		Assert.Throws<ArgumentOutOfRangeException>(
			() => memory.Read(address)
		);
	}

	[Theory]
	[InlineData(1)]
	[InlineData(10)]
	[InlineData(100)]
	public void WriteValidAddresses(int size)
	{
		var memory = new ArrayMemory(size);
		for (var i = 0; i < size; i++)
		{
			// None of the calls to `Write()` should throw since only valid
			//   addresses are being written to
			memory.Write(i, 0);
		}
	}

	[Theory]
	[InlineData(10, -1)]
	[InlineData(10, 10)]
	[InlineData(1, 1)]
	public void WriteInvalidAddresses(int size, int address)
	{
		var memory = new ArrayMemory(size);
		Assert.Throws<ArgumentOutOfRangeException>(
			() => memory.Write(address, 0)
		);
	}

	[Theory]
	[InlineData(1, 0)]
	[InlineData(10, 0)]
	[InlineData(10, 9)]
	public void ValidateValidAddresses(int size, int address)
	{
		var memory = new ArrayMemory(size);
		// This should not throw since the address is valid
		memory.ValidateAddress(address);
	}

	[Theory]
	[InlineData(1)]
	[InlineData(10)]
	public void ReadNewValue(int size)
	{
		var memory = new ArrayMemory(size);

		// Write a value to each memory location
		for (var i = 0; i < size; i++)
		{
			memory.Write(i, i);
		}

		// Verify that the values read back are the same as the values written
		for (var i = 0; i < size; i++)
		{
			Assert.Equal(i, memory.Read(i));
		}
	}

	[Theory]
	[InlineData(1, -1)]
	[InlineData(1, 1)]
	[InlineData(10, -1)]
	[InlineData(10, 10)]
	public void ValidateInvalidAddresses(int size, int address)
	{
		var memory = new ArrayMemory(size);
		Assert.Throws<ArgumentOutOfRangeException>(
			() => memory.ValidateAddress(address)
		);
	}
}
