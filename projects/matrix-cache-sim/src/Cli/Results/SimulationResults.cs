/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Cli.Results;

/// Results of running a single simulation instance.
public readonly record struct SimulationResults
{
	/// Number of cache hits that occurred during the simulation.
	public int CacheHits { get; init; }

	/// Number of cache misses that occurred during the simulation.
	public int CacheMisses { get; init; }

	/// Total number of memory accesses that occurred during the simulation.
	public int TotalMemoryAccesses { get; init; }

	/// Total score for the run.
	public int Score { get; init; }
}
