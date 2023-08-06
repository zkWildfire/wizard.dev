/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Agents;
using Mcs.Simulator.Policies.Eviction;
using Mcs.Simulator.Policies.Placement;
namespace Mcs.Cli.Results;

/// Contains all configuration data for a single simulation run.
public readonly record struct SimulationConfig
{
	/// UI-printable name of the configuration.
	public string ConfigurationName { get; init; }

	/// Unique ID assigned to the configuration.
	public string ConfigurationId { get; init; }

	/// Number of cache lines in the cache.
	public int CacheLineCount { get; init; }

	/// Size, in number of elements, of each cache line.
	public int CacheLineSize { get; init; }

	/// Placement policy to use for the cache.
	public IPlacementPolicyFactory PlacementPolicyFactory { get; init; }

	/// Eviction policy to use for the cache.
	public IEvictionPolicyFactory EvictionPolicyFactory { get; init; }

	/// Factory to use to construct the agent.
	public IAgentFactory AgentFactory { get; init; }

	/// Number of registers that the agent can use.
	public int RegisterCount { get; init; }

	/// X-dimension size of the matrix being transposed.
	public int MatrixSizeX { get; init; }

	/// Y-dimension size of the matrix being transposed.
	public int MatrixSizeY { get; init; }

	/// Starting offset used for the matrix.
	public int MatrixStartingOffset { get; init; }

	/// Whether the matrix is in row-major order.
	public bool MatrixIsRowMajor { get; init; }
}
