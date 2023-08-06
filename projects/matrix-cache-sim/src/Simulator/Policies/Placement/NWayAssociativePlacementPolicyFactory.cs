/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Simulator.Policies.Placement;

/// Factory type for the `NWayAssociativePlacementPolicy` class.
public class NWayAssociativePlacementPolicyFactory : IPlacementPolicyFactory
{
	/// Name of the policy that the factory creates.
	public string PolicyName => $"{_associativity}-Way Associative";

	/// Associativity to use for constructed policies.
	private readonly int _associativity;

	/// Initializes the factory.
	/// @param associativity Associativity to use for constructed policies.
	public NWayAssociativePlacementPolicyFactory(int associativity)
	{
		_associativity = associativity;
	}

	/// Creates an placement policy instance.
	/// @param cacheSize Size of the cache in number of cache lines.
	/// @param cacheLineSize Size of each cache line in number of elements.
	/// @returns A new placement policy instance.
	public IPlacementPolicy Construct(int cacheSize, int cacheLineSize)
	{
		return new NWayAssociativePlacementPolicy(
			cacheSize,
			cacheLineSize,
			_associativity
		);
	}
}
