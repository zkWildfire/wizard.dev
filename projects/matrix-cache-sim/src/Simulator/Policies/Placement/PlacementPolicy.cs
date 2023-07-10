/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.CacheLines;
namespace Mcs.Simulator.Policies.Placement;

/// Policy that determines where a cache line may be placed in the cache.
public interface IPlacementPolicy
{
	/// Gets the indices in the cache that a cache line may be placed in.
	/// @param cacheLine Cache line to get the indices for.
	/// @returns The indices in the cache that the cache line may be placed in.
	IReadOnlyList<int> GetIndices(ICacheLine cacheLine);
}
