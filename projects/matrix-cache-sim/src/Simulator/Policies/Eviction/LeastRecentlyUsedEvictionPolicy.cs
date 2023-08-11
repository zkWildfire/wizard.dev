/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Simulator.Policies.Eviction;

/// Policy that evicts the least recently used cache line in a set.
public class LeastRecentlyUsedEvictionPolicy : IEvictionPolicy
{
	/// Number of cache lines in the cache.
	public int CacheSize => _lastAccessTimes.Count;

	/// "Timestamp" used for cache lines that are not loaded.
	private const int UNLOADED_TIME = -1;

	/// Times at which each cache line was most recently accessed.
	/// These times will be based on this policy's counter, not actual time.
	///   The higher the value, the more recently the cache line was accessed.
	///   Cache lines that are not loaded will have a value of -1.
	private readonly List<int> _lastAccessTimes;

	/// Counter used to generate "timestamps" for cache lines.
	private int _counter;

	/// Initializes the policy.
	/// @param numCacheLines Number of cache lines in the cache.
	public LeastRecentlyUsedEvictionPolicy(int numCacheLines)
	{
		_lastAccessTimes = Enumerable.Repeat(UNLOADED_TIME, numCacheLines)
			.ToList();
	}

	/// Notifies the policy that a loaded cache line was accessed.
	/// @param index Index of the cache line that was accessed.
	public void OnCacheLineAccessed(int index)
	{
		_lastAccessTimes[index] = _counter++;
	}

	/// Notifies the policy that a cache line was evicted.
	/// @param index Index that the cache line was evicted from.
	public void OnCacheLineEvicted(int index)
	{
		_lastAccessTimes[index] = UNLOADED_TIME;
	}

	/// Notifies the policy that a cache line was loaded.
	/// @param index Index that the cache line was loaded into.
	public void OnCacheLineLoaded(int index)
	{
		_lastAccessTimes[index] = _counter++;
	}

	/// Gets the index of the cache line to evict.
	/// @param indices Indices of the cache lines that are valid eviction
	///   targets.
	public int GetIndexToEvict(IReadOnlyList<int> indices)
	{
		var minIndex = indices[0];
		var minTime = _lastAccessTimes[minIndex];

		for (var i = 1; i < indices.Count; i++)
		{
			var index = indices[i];
			var time = _lastAccessTimes[index];

			// Ignore unloaded cache lines
			if (time == UNLOADED_TIME)
			{
				continue;
			}

			// Replace the current minimum index if the current minimum index
			//   is a cache line that isn't loaded or if the current cache line
			//   was accessed less recently than the current minimum index
			if (minTime == UNLOADED_TIME || time < minTime)
			{
				minIndex = index;
				minTime = time;
			}
		}

		// At least one cache line in the indices provided must be loaded
		Debug.Assert(minTime != UNLOADED_TIME);
		return minIndex;
	}
}
