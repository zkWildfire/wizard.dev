/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Simulator.Events;

/// Event arguments for when a cache line is loaded into the cache.
public class OnCacheLineLoadedEventArgs : EventArgs
{
	/// Index that the cache line was loaded into.
	public required int Index { get; init; }

	/// Memory address that the cache line starts at.
	public required int Address { get; init; }

	/// Size of the cache line in number of elements.
	public required int Size { get; init; }
}
