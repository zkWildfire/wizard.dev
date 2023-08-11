/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.CacheLines;
namespace Mcs.Simulator.Policies.Placement;

/// Policy used by N-way associative caches.
public class NWayAssociativePlacementPolicy : IPlacementPolicy
{
	/// Size of the cache in number of cache lines.
	public int CacheSize => _numSets * Associativity;

	/// Size of each cache line in number of elements.
	public int CacheLineSize { get; }

	/// Number of cache lines in each set.
	public int Associativity { get; }

	/// Number of cache sets in the cache.
	private readonly int _numSets;

	/// Initializes the policy.
	/// @param cacheSize Size of the cache in number of cache lines.
	/// @param cacheLineSize Size of each cache line in number of elements.
	/// @param associativity Number of cache lines in each set.
	/// @throws ArgumentException If `cacheSize` is not a multiple of
	///   `associativity`.
	public NWayAssociativePlacementPolicy(
		int cacheSize,
		int cacheLineSize,
		int associativity)
	{
		if (cacheSize % associativity != 0)
		{
			throw new ArgumentException(
				"Cache size must be a multiple of associativity."
			);
		}

		CacheLineSize = cacheLineSize;
		Associativity = associativity;
		_numSets = cacheSize / associativity;
	}

	/// Gets the indices in the cache that a cache line may be placed in.
	/// @param cacheLine Cache line to get the indices for.
	/// @returns The indices in the cache that the cache line may be placed in.
	public IReadOnlyList<int> GetIndices(ICacheLine cacheLine)
	{
		Debug.Assert(cacheLine.Size == CacheLineSize);

		// Determine which set the cache line is part of
		var setIndex = cacheLine.StartingAddress / CacheLineSize % _numSets;
		return Enumerable.Range(0, Associativity)
			.Select(i => (i * _numSets) + setIndex)
			.ToList();
	}
}
