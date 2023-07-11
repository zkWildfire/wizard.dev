/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.CacheLines;
using Mcs.Simulator.Events;
using Mcs.Simulator.Policies.Eviction;
using Mcs.Simulator.Policies.Placement;
namespace Mcs.Simulator.Caches;

/// Modular implementation of the cache interface.
public class ModularCache : ICache
{
	/// Event broadcast when a cache line is loaded.
	public event EventHandler<OnCacheLineLoadedEventArgs>? OnCacheLineLoaded;

	/// Event broadcast when a cache line is evicted.
	public event EventHandler<OnCacheLineEvictedEventArgs>? OnCacheLineEvicted;

	/// Size of the cache in number of cache lines.
	public int CacheSize { get; }

	/// Size of a cache line in number of elements.
	public int CacheLineSize { get; }

	/// Cache lines in the cache.
	private readonly List<ICacheLine?> _cacheLines;

	/// Placement policy for the cache.
	private readonly IPlacementPolicy _placementPolicy;

	/// Eviction policy for the cache.
	private readonly IEvictionPolicy _evictionPolicy;

	/// Initializes the cache.
	/// @param cacheSize Size of the cache in number of cache lines.
	/// @param cacheLineSize Size of a cache line in number of elements.
	/// @param placementPolicy Placement policy for the cache.
	/// @param evictionPolicy Eviction policy for the cache.
	public ModularCache(
		int cacheSize,
		int cacheLineSize,
		IPlacementPolicy placementPolicy,
		IEvictionPolicy evictionPolicy)
	{
		CacheSize = cacheSize;
		CacheLineSize = cacheLineSize;
		_placementPolicy = placementPolicy;
		_evictionPolicy = evictionPolicy;
		_cacheLines = new List<ICacheLine?>(CacheSize);
	}

	/// Gets the cache line containing the given memory address.
	/// @param address Memory address to get the cache line for.
	/// @pre `IsPresent(address)` must be true.
	/// @returns The cache line containing the memory address.
	/// @throws ArgumentOutOfRangeException If the address is not a valid
	///   main memory address.
	public ICacheLine GetCacheLine(int address)
	{
		Debug.Assert(IsPresent(address));

		// The number of cache lines in each cache will be relatively low, so
		//   a simple linear search is sufficient.
		foreach (var cacheLine in _cacheLines)
		{
			if (cacheLine != null && cacheLine.Contains(address))
			{
				return cacheLine;
			}
		}

		throw new UnreachableException();
	}

	/// Checks if the cache contains a memory address.
	/// @param address Address to check. Must be a main memory address.
	/// @returns True if the cache contains the address, false otherwise.
	public bool IsPresent(int address)
	{
		return _cacheLines.Any(cacheLine =>
			cacheLine != null && cacheLine.Contains(address)
		);
	}

	/// Loads the cache line into the cache.
	/// @remarks This will always trigger the `OnCacheLineLoaded` event and
	///   may trigger the `OnCacheLineEvicted` event. If the `OnCacheLineEvicted`
	///   event is triggered, it will be triggered before the `OnCacheLineLoaded`
	///   event.
	/// @param cacheLine Cache line to load.
	public void LoadCacheLine(ICacheLine cacheLine)
	{
		// Determine the indices that the cache line may be placed at
		var indices = _placementPolicy.GetIndices(cacheLine);

		// Find the first index that is not occupied
		var index = indices.FirstOrDefault(i => _cacheLines[i] == null, -1);

		// If no unoccupied index was found, evict a cache line
		if (index == -1)
		{
			index = _evictionPolicy.GetIndexToEvict(indices);

			// Handle evicting the cache line
			var evictedCacheLine = _cacheLines[index];
			Debug.Assert(evictedCacheLine != null);
			evictedCacheLine.OnCacheLineAccessed -= OnCacheLineAccessed;
			evictedCacheLine.Flush();
			OnCacheLineEvicted?.Invoke(
				this,
				new OnCacheLineEvictedEventArgs()
				{
					Index = index,
					Address = evictedCacheLine.StartingAddress,
					Size = evictedCacheLine.Size
				}
			);

			// This isn't strictly necessary, but it's good practice and ensures
			//   that an assert can be added that verifies that the cache line
			//   in the target index is null
			_cacheLines[index] = null;
		}

		// An empty spot should have been found, either as a result of evicting
		//   a cache line or because the slot was empty
		Debug.Assert(index != -1);
		Debug.Assert(_cacheLines[index] == null);

		// Load the cache line
		_cacheLines[index] = cacheLine;
		cacheLine.OnCacheLineAccessed += OnCacheLineAccessed;
		OnCacheLineLoaded?.Invoke(
			this,
			new OnCacheLineLoadedEventArgs()
			{
				Index = index,
				Address = cacheLine.StartingAddress,
				Size = cacheLine.Size
			}
		);
	}

	/// Callback bound to each cache line's `OnCacheLineAccessed` event.
	/// @param sender Cache line that was accessed.
	/// @param args Event arguments.
	private void OnCacheLineAccessed(
		object? sender,
		OnCacheLineAccessedEventArgs args)
	{
		// Find the index of the cache line containing the memory address
		var index = _cacheLines.FindIndex(
			cacheLine => cacheLine != null && cacheLine.Contains(args.Address)
		);
		Debug.Assert(index != -1);
		Debug.Assert(ReferenceEquals(_cacheLines[index], sender));

		// Notify the eviction policy that the cache line was accessed
		_evictionPolicy.OnCacheLineAccessed(index);
	}
}
