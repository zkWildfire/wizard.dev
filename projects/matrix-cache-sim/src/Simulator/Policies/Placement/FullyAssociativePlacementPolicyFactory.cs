/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Simulator.Policies.Placement;

/// Factory type for the `FullyAssociativePlacementPolicy` class.
public class FullyAssociativePlacementPolicyFactory : IPlacementPolicyFactory
{
	/// Name of the policy that the factory creates.
	public string PolicyName => "Fully Associative";

	/// Creates an placement policy instance.
	/// @param cacheSize Size of the cache in number of cache lines.
	/// @param cacheLineSize Size of each cache line in number of elements.
	/// @param associativity Associativity of the cache.
	/// @returns A new placement policy instance.
	public IPlacementPolicy Construct(int cacheSize, int cacheLineSize)
	{
		return new FullyAssociativePlacementPolicy(cacheSize);
	}
}
