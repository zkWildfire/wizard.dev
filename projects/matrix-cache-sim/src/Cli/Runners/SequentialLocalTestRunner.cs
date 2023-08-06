/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Cli.Results;
using Mcs.Cli.Rewards;
using Mcs.Cli.Simulation;
using Mcs.Simulator;
using Mcs.Simulator.CacheLines;
using Mcs.Simulator.Caches;
using Mcs.Simulator.Memory;
using Mcs.Simulator.Policies.Eviction;
using Mcs.Simulator.Policies.Placement;
using Mcs.Simulator.Simulation;
using Mcs.Simulator.Validators;
namespace Mcs.Cli.Runners;

/// Test runner that runs simulations sequentially on the local machine.
public class SequentialLocalTestRunner : ITestRunner
{
	/// Factory for creating reward components.
	private readonly Func<IRewardComponent> _rewardComponentFactory;

	/// Initializes the test runner.
	/// @param rewardComponentFactory Factory for creating reward components.
	public SequentialLocalTestRunner(
		Func<IRewardComponent> rewardComponentFactory)
	{
		_rewardComponentFactory = rewardComponentFactory;
	}

	/// Performs a set of simulation runs.
	/// @param configs Configurations to use for the simulation runs.
	/// @return Results from the simulation runs.
	public IEnumerable<SimulationRun> RunSimulations(
		IEnumerable<SimulationConfig> configs)
	{
		foreach (var config in configs)
		{
			yield return RunSimulation(config);
		}
	}

	/// Performs a single simulation run.
	/// @param config Configuration to use for the simulation.
	/// @return Results from the simulation run.
	public SimulationRun RunSimulation(SimulationConfig config)
	{
		// Extract parameters into local variables for convenience
		var MATRIX_X = config.MatrixSizeX;
		var MATRIX_Y = config.MatrixSizeY;
		var STARTING_OFFSET = config.MatrixStartingOffset;
		var CACHE_SIZE = config.CacheLineCount;
		var CACHE_LINE_SIZE = config.CacheLineSize;

		// Create the simulator and its components
		var memory = new ArrayMemory((MATRIX_X * MATRIX_Y) + STARTING_OFFSET);
		var placementPolicy = new FullyAssociativePlacementPolicy(CACHE_SIZE);
		var evictionPolicy = new LeastRecentlyUsedEvictionPolicy(CACHE_SIZE);
		var validator = new SequentialMemoryValidator();
		var cache = new ModularCache(
			CACHE_SIZE,
			CACHE_LINE_SIZE,
			placementPolicy,
			evictionPolicy
		);
		var cacheLineFactory = new WriteThroughCacheLineFactory(CACHE_LINE_SIZE);
		var matrix = new RowMajorMatrix(
			memory,
			MATRIX_X,
			MATRIX_Y,
			STARTING_OFFSET
		);
		var simulator = new ModularSimulator(
			memory,
			cache,
			cacheLineFactory,
			validator,
			matrix
		);

		// Set up the components for running the simulation
		var rewardComponent = _rewardComponentFactory();
		var agent = config.AgentFactory.Construct(
			simulator,
			config.RegisterCount
		);
		var instance = new Instance(
			simulator,
			rewardComponent,
			config.RegisterCount,
			agent
		);

		// Run the simulation and return the results
		return new SimulationRun
		{
			Config = config,
			Results = instance.Run()
		};
	}
}
