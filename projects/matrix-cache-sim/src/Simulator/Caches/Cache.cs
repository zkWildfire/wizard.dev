/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.CacheLines;
using Mcs.Simulator.Events;
namespace Mcs.Simulator.Caches;

/// Interface for classes that simulate a single-level processor cache.
public interface ICache
{
	/// Event broadcast when a cache line is loaded.
	event EventHandler<OnCacheLineLoadedEventArgs>? OnCacheLineLoaded;

	/// Event broadcast when a cache line is evicted.
	event EventHandler<OnCacheLineEvictedEventArgs>? OnCacheLineEvicted;

	/// Size of the cache in number of cache lines.
	int CacheSize { get; }

	/// Size of a cache line in number of elements.
	int CacheLineSize { get; }

	/// Gets the cache line containing the given memory address.
	/// @param address Memory address to get the cache line for.
	/// @pre `IsPresent(address)` must be true.
	/// @returns The cache line containing the memory address.
	/// @throws ArgumentOutOfRangeException If the address is not a valid
	///   main memory address.
	ICacheLine GetCacheLine(int address);

	/// Checks if the cache contains a memory address.
	/// @param address Address to check. Must be a main memory address.
	/// @returns True if the cache contains the address, false otherwise.
	bool IsPresent(int address);

	/// Loads the cache line into the cache.
	/// @remarks This will always trigger the `OnCacheLineLoaded` event and
	///   may trigger the `OnCacheLineEvicted` event. If the `OnCacheLineEvicted`
	///   event is triggered, it will be triggered before the `OnCacheLineLoaded`
	///   event.
	/// @param cacheLine Cache line to load.
	void LoadCacheLine(ICacheLine cacheLine);
}
