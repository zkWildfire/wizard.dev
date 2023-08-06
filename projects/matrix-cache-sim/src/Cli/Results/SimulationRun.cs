/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Cli.Results;

/// Bundles together all information for a single simulation run.
public readonly record struct SimulationRun
{
	/// UI-printable name of the configuration.
	public string ConfigurationName => Config.ConfigurationName;

	/// Unique ID assigned to the configuration.
	public string ConfigurationId => Config.ConfigurationId;

	/// Configuration used for the simulation.
	public SimulationConfig Config { get; init; }

	/// Results of the simulation run.
	public SimulationResults Results { get; init; }
}
