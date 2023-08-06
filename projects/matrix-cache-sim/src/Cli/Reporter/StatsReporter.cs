/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Cli.Results;
namespace Mcs.Cli.Reporter;

/// Interface for classes that report run statistics.
public interface IStatsReporter
{
	/// Reports results of evaluating multiple agents.
	/// @param results Results from the agent evaluations.
	void ReportResults(IEnumerable<AgentResults> results);

	/// Reports results of evaluating an agent.
	/// @param results Results from the agent evaluation.
	void ReportResults(AgentResults results);

	/// Reports results from a single simulation run.
	/// @param results Results from the simulation run.
	void ReportResults(SimulationRun results);

	/// Reports results from a single set of simulation runs.
	/// @param results Results from the simulation runs.
	void ReportResults(SimulationResults results);
}
