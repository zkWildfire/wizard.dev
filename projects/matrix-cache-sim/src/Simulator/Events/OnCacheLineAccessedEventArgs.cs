/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Simulator.Events;

/// Event arguments for when a cache line element is accessed.
public class OnCacheLineAccessedEventArgs : EventArgs
{
	/// Memory address that was accessed.
	public required int Address { get; init; }

	/// Whether the memory access was a read or a write.
	public required bool IsRead { get; init; }

	/// Value now stored at the memory address.
	public required int NewValue { get; init; }

	/// Value previously stored at the memory address.
	public required int OldValue { get; init; }
}
