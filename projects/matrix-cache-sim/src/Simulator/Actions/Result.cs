/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Simulator.Actions;

/// Provides feedback to an agent about the result of an action.
public readonly record struct Result
{
	/// Whether the action resulted in a cache hit.
	public bool CacheHit { get; init; }

	/// Whether the action resulted in a cache miss.
	public bool CacheMiss { get; init; }

	/// Whether the action resulted in a cache eviction.
	public bool CacheEviction { get; init; }

	/// Reward for the action.
	public int Reward { get; init; }
}
