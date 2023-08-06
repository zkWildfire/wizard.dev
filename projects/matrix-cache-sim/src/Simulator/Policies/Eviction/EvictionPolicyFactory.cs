/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Simulator.Policies.Eviction;

/// Interface for factory classes that create eviction policy instances.
public interface IEvictionPolicyFactory
{
	/// Name of the policy that the factory creates.
	string PolicyName { get; }

	/// Creates an eviction policy instance.
	/// @param numCacheLines Number of cache lines in the cache.
	/// @returns A new eviction policy instance.
	IEvictionPolicy Construct(int numCacheLines);
}
