using Mcs.Agents;
using Mcs.Agents.Naive;
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

// Set up the simulator
const int MATRIX_X = 32;
const int MATRIX_Y = 32;
const int STARTING_OFFSET = 0;
const int CACHE_SIZE = 16;
const int CACHE_LINE_SIZE = 8;
const int REGISTER_COUNT = 8;

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

// Set up the evaluation instance
const int CACHE_HIT_REWARD = 1;
const int CACHE_MISS_REWARD = -1;
const int MEMORY_ACCESS_REWARD = 2;
var agentFactory = (ISimulator _, int registerCount) =>
	new NaiveAgent(matrix, registerCount);
IAgent agent = agentFactory(simulator, REGISTER_COUNT);
var rewardComponent = new ConstRewardComponent(
	CACHE_HIT_REWARD,
	CACHE_MISS_REWARD,
	MEMORY_ACCESS_REWARD
);
var instance = new Instance(
	simulator,
	rewardComponent,
	REGISTER_COUNT,
	agent
);

// Run the simulation and print results
var results = instance.Run();
Console.WriteLine($"Agent: {agent.Name}");
Console.WriteLine($"Cache hits: {results.CacheHits}");
Console.WriteLine($"Cache misses: {results.CacheMisses}");
Console.WriteLine($"Total memory accesses: {results.TotalMemoryAccesses}");
Console.WriteLine($"Score: {results.Score}");
