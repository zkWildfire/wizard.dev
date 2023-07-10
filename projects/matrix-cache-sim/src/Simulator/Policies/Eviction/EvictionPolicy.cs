/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Simulator.Policies.Eviction;

/// Policy that determines which cache line to evict from the cache.
public interface IEvictionPolicy
{
	/// Notifies the policy that a loaded cache line was accessed.
	/// @param index Index of the cache line that was accessed.
	void OnCacheLineAccessed(int index);

	/// Notifies the policy that a cache line was evicted.
	/// @param index Index that the cache line was evicted from.
	void OnCacheLineEvicted(int index);

	/// Notifies the policy that a cache line was loaded.
	/// @param index Index that the cache line was loaded into.
	void OnCacheLineLoaded(int index);

	/// Gets the index of the cache line to evict.
	/// @param indices Indices of the cache lines that are valid eviction
	///   targets.
	int GetIndexToEvict(IReadOnlyList<int> indices);
}
