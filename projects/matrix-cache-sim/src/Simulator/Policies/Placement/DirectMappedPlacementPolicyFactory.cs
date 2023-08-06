/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Simulator.Policies.Placement;

/// Factory type for the `DirectMappedPlacementPolicy` class.
public class DirectMappedPlacementPolicyFactory : IPlacementPolicyFactory
{
	/// Name of the policy that the factory creates.
	public string PolicyName => "Direct Mapped";

	/// Creates an placement policy instance.
	/// @param cacheSize Size of the cache in number of cache lines.
	/// @param cacheLineSize Size of each cache line in number of elements.
	/// @returns A new placement policy instance.
	public IPlacementPolicy Construct(int cacheSize, int cacheLineSize)
	{
		return new DirectMappedPlacementPolicy(cacheSize, cacheLineSize);
	}
}
