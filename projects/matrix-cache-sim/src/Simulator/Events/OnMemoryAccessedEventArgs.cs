/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Simulator.Events;

/// Event arguments for when a memory address is accessed.
public class OnMemoryAccessedEventArgs : EventArgs
{
	/// Memory address that was accessed.
	public required int Address { get; init; }

	/// X-coordinate within the matrix of the memory address that was accessed.
	public required int X { get; init; }

	/// Y-coordinate within the matrix of the memory address that was accessed.
	public required int Y { get; init; }

	/// Whether the memory access was a read or a write.
	public required bool IsRead { get; init; }

	/// Whether the memory access resulted in a cache hit or a miss.
	public required bool IsCacheHit { get; init; }

	/// Value now stored at the memory address.
	public required int NewValue { get; init; }

	/// Value previously stored at the memory address.
	public required int OldValue { get; init; }
}
