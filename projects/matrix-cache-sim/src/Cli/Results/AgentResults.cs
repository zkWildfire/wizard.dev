/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Cli.Results;

/// Results from all simulations that were run to evaluate an agent.
public readonly record struct AgentResults
{
	/// Name of the agent that was evaluated.
	public string AgentName { get; init; }

	/// Unique ID assigned to the agent.
	public string AgentId { get; init; }

	/// All simulation runs that were run to evaluate the agent.
	/// Each run is indexed by its configuration ID.
	public IReadOnlyDictionary<string, SimulationRun> SimulationRuns
	{
		get;
		init;
	}
}
