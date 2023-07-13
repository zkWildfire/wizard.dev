/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.Events;
namespace Mcs.Simulator;

/// Interface for all simulator implementations.
public interface ISimulator
{
	/// Event emitted when a cache line is loaded.
	event EventHandler<OnCacheLineLoadedEventArgs>? OnCacheLineLoaded;

	/// Event emitted when a cache line is evicted.
	event EventHandler<OnCacheLineEvictedEventArgs>? OnCacheLineEvicted;

	/// Event raised when a memory location is accessed.
	event EventHandler<OnMemoryAccessedEventArgs>? OnMemoryAccess;

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

	/// Checks whether the simulation's matrix has been fully transposed.
	/// @returns True if the matrix has been fully transposed, false otherwise.
	bool Validate();
}
