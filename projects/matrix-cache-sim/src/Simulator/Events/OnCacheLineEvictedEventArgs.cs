/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Simulator.Events;

/// Event arguments for when a cache line is evicted from the cache.
public class OnCacheLineEvictedEventArgs : EventArgs
{
	/// Index that the cache line was evicted from.
	public required int Index { get; init; }

	/// Memory address that the evicted cache line started at.
	public required int Address { get; init; }

	/// Size of the evicted cache line in number of elements.
	public required int Size { get; init; }
}
