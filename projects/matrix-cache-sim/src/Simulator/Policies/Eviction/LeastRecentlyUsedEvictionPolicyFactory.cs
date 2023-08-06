/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Simulator.Policies.Eviction;

/// Factory type for the `LeastRecentlyUsedEvictionPolicy` class.
public class LeastRecentlyUsedEvictionPolicyFactory : IEvictionPolicyFactory
{
	/// Name of the policy that the factory creates.
	public string PolicyName => "Least Recently Used";

	/// Creates an eviction policy instance.
	/// @param numCacheLines Number of cache lines in the cache.
	/// @returns A new eviction policy instance.
	public IEvictionPolicy Construct(int numCacheLines)
	{
		return new LeastRecentlyUsedEvictionPolicy(numCacheLines);
	}
}
