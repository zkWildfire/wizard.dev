/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.Memory;
namespace Mcs.Simulator.CacheLines;

/// Cache line that uses write-through caching.
public class WriteThroughCacheLine : ICacheLine
{
	/// Memory address that the cache line starts at.
	public int StartingAddress { get; }

	/// Past-the-end memory address that the cache line ends at.
	public int EndingAddress { get; }

	/// Number of elements in the cache line.
	public int Size { get; }

	/// Memory that the cache line reads and writes to.
	private readonly IMemory _memory;

	/// Initializes the cache line.
	/// @param memory Memory to read and write to.
	/// @param startingAddress Memory address that the cache line starts at.
	/// @param size Number of elements in the cache line.
	public WriteThroughCacheLine(
		IMemory memory,
		int startingAddress,
		int size)
	{
		_memory = memory;
		StartingAddress = startingAddress;
		EndingAddress = startingAddress + size;
		Size = size;
	}

	/// Checks if the cache line contains a memory address.
	/// @param address Address to check. Must be a main memory address.
	public bool Contains(int address)
	{
		return address >= StartingAddress && address < EndingAddress;
	}

	/// Flushes the cache line to main memory.
	public void Flush()
	{
		// Do nothing - a write through cache line writes modified values to
		//   main memory immediately.
	}

	/// Reads a value from the cache line.
	/// @param address Address to read from. Must be a main memory address.
	/// @throws ArgumentOutOfRangeException If the address is not a valid
	///   address in the cache line.
	/// @returns The value at the address.
	public int Read(int address)
	{
		return Contains(address)
			? _memory.Read(address)
			: throw new ArgumentOutOfRangeException(
				nameof(address),
				address,
				$"Expected address '{address}' to be in the range " +
				$"[{StartingAddress}, {EndingAddress})."
			);
	}

	/// Writes a value to the cache line.
	/// @param address Address to write to. Must be a main memory address.
	/// @param value Value to write.
	/// @throws ArgumentOutOfRangeException If the address is not a valid
	///   address in the cache line.
	public void Write(int address, int value)
	{
		if (Contains(address))
		{
			_memory.Write(address, value);
		}
		else
		{
			throw new ArgumentOutOfRangeException(
				nameof(address),
				address,
				$"Expected address '{address}' to be in the range " +
				$"[{StartingAddress}, {EndingAddress})."
			);
		}
	}
}
