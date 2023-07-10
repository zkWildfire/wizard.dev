/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.CacheLines;
namespace Mcs.Simulator.Policies.Placement;

/// Policy used by direct mapped caches.
public class DirectMappedPlacementPolicy : IPlacementPolicy
{
	/// Size of the cache in number of cache lines.
	private readonly int _cacheSize;

	/// Size of each cache line in number of elements.
	private readonly int _cacheLineSize;

	/// Initializes the policy.
	/// @param cacheSize Size of the cache in number of cache lines.
	/// @param cacheLineSize Size of each cache line in number of elements.
	public DirectMappedPlacementPolicy(int cacheSize, int cacheLineSize)
	{
		_cacheSize = cacheSize;
		_cacheLineSize = cacheLineSize;
	}

	/// Gets the indices in the cache that a cache line may be placed in.
	/// @param cacheLine Cache line to get the indices for.
	/// @returns The indices in the cache that the cache line may be placed in.
	public IReadOnlyList<int> GetIndices(ICacheLine cacheLine)
	{
		Debug.Assert(cacheLine.Size == _cacheLineSize);

		// Each cache line may be mapped to exactly one location in the cache
		//   based on its starting address.
		var cacheLineIndex = cacheLine.StartingAddress / _cacheLineSize;
		return new List<int> {
			cacheLineIndex % _cacheSize
		};
	}
}
