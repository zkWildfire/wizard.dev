/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.CacheLines;
namespace Mcs.Simulator.Policies.Placement;

/// Policy used by fully associative caches.
public class FullyAssociativePlacementPolicy : IPlacementPolicy
{
	/// Array to return whenever `GetIndices()` is called.
	/// A fully associative cache maps all cache lines to every possible index
	///   in the cache, so the array to return is always the same regardless
	///   of the location of the cache line passed to the policy.
	private readonly IReadOnlyList<int> _indices;

	/// Initializes the policy.
	/// @param cacheSize Size of the cache in number of cache lines.
	public FullyAssociativePlacementPolicy(int cacheSize)
	{
		_indices = Enumerable.Range(0, cacheSize).ToList();
	}

	/// Gets the indices in the cache that a cache line may be placed in.
	/// @param cacheLine Cache line to get the indices for.
	/// @returns The indices in the cache that the cache line may be placed in.
	public IReadOnlyList<int> GetIndices(ICacheLine cacheLine)
	{
		return _indices;
	}
}
