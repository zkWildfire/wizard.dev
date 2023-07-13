/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Simulator.Memory;

/// Interface for classes that simulates a block of system memory.
/// For this simulation, the smallest unit of memory is an integer. This is
///   because agents always operate on full matrix cells and because the effects
///   of using different sized integers can be simulated by using different
///   sized cache lines.
public interface IMemory
{
	/// Number of elements in the memory.
	int Size { get; }

	/// Reads a value from memory.
	/// @param address Address to read from.
	/// @throws ArgumentOutOfRangeException If the address is not a valid
	///   address in the memory block.
	/// @returns The value at the address.
	int Read(int address);

	/// Writes a value to memory.
	/// @param address Address to write to.
	/// @param value Value to write.
	/// @throws ArgumentOutOfRangeException If the address is not a valid
	///   address in the memory block.
	void Write(int address, int value);

	/// Checks if the address is a valid address in the memory block.
	/// @throws ArgumentOutOfRangeException If the address is not a valid
	///   address in the memory block.
	void ValidateAddress(int address);
}
