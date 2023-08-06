/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Agents;
using Mcs.Cli.Results;
namespace Mcs.Cli.Runners;

/// Interface for classes that handle running one or more simulations.
/// Test runners are intended to run agents for evaluation purposes rather than
///   to run simulations for the purpose of training agents.
public interface ITestRunner
{
	/// Performs a set of simulation runs.
	/// @param agentFactory Factory to use for creating agents.
	/// @param configs Configurations to use for the simulation runs.
	/// @return Results from the simulation runs.
	IEnumerable<SimulationRun> RunSimulations(
		IAgentFactory agentFactory,
		IEnumerable<SimulationConfig> configs
	);

	/// Performs a single simulation run.
	/// @param agentFactory Factory to use for creating agents.
	/// @param config Configuration to use for the simulation.
	/// @return Results from the simulation run.
	SimulationRun RunSimulation(
		IAgentFactory agentFactory,
		SimulationConfig config
	);
}
