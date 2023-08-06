/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Simulator.Policies.Placement;

/// Interface for factory classes that create placement policy instances.
public interface IPlacementPolicyFactory
{
	/// Name of the policy that the factory creates.
	string PolicyName { get; }

	/// Creates an placement policy instance.
	/// @param cacheSize Size of the cache in number of cache lines.
	/// @param cacheLineSize Size of each cache line in number of elements.
	/// @returns A new placement policy instance.
	IPlacementPolicy Construct(int cacheSize, int cacheLineSize);
}
