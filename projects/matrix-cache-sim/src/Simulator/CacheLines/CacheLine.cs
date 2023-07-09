/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Simulator.CacheLines;

/// Interface for classes that simulate a cache line.
public interface ICacheLine
{
	/// Memory address that the cache line starts at.
	int StartingAddress { get; }

	/// Past-the-end memory address that the cache line ends at.
	int EndingAddress { get; }

	/// Number of elements in the cache line.
	int Size { get; }

	/// Checks if the cache line contains a memory address.
	/// @param address Address to check. Must be a main memory address.
	bool Contains(int address);

	/// Flushes the cache line to main memory.
	void Flush();

	/// Reads a value from the cache line.
	/// @param address Address to read from. Must be a main memory address.
	/// @throws ArgumentOutOfRangeException If the address is not a valid
	///   address in the cache line.
	/// @returns The value at the address.
	int Read(int address);

	/// Writes a value to the cache line.
	/// @param address Address to write to. Must be a main memory address.
	/// @param value Value to write.
	/// @throws ArgumentOutOfRangeException If the address is not a valid
	///   address in the cache line.
	void Write(int address, int value);
}
