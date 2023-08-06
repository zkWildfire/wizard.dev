using Mcs.Agents;
using Mcs.Agents.Naive;
using Mcs.Cli.Reporter;
using Mcs.Cli.Results;
using Mcs.Cli.Rewards;
using Mcs.Cli.Runners;
using Mcs.Simulator.Policies.Eviction;
using Mcs.Simulator.Policies.Placement;

// Configuration parameters
const int DEFAULT_REGISTER_COUNT = 8;
const int CACHE_HIT_REWARD = 1;
const int CACHE_MISS_REWARD = -1;
const int MEMORY_ACCESS_REWARD = 0;

const int DEFAULT_CACHE_LINE_SIZE = 8;
const int SMALL_CACHE_SIZE = 4;
const int MEDIUM_CACHE_SIZE = 8;
const int LARGE_CACHE_SIZE = 16;
const int XL_CACHE_SIZE = 32;

// Configurations and agents
SimulationConfig[] CONFIGS = {
	new SimulationConfig
	{
		ConfigurationName = "32x32 Small Direct Mapped",
		ConfigurationId = "32x32-small-direct",
		CacheLineCount = SMALL_CACHE_SIZE,
		CacheLineSize = DEFAULT_CACHE_LINE_SIZE,
		PlacementPolicyFactory = new DirectMappedPlacementPolicyFactory(),
		EvictionPolicyFactory = new LeastRecentlyUsedEvictionPolicyFactory(),
		RegisterCount = DEFAULT_REGISTER_COUNT,
		MatrixSizeX = 32,
		MatrixSizeY = 32,
		MatrixStartingOffset = 0,
		MatrixIsRowMajor = true
	},
	new SimulationConfig
	{
		ConfigurationName = "32x32 Small 2-Way Associative",
		ConfigurationId = "32x32-small-2way",
		CacheLineCount = SMALL_CACHE_SIZE,
		CacheLineSize = DEFAULT_CACHE_LINE_SIZE,
		PlacementPolicyFactory = new NWayAssociativePlacementPolicyFactory(2),
		EvictionPolicyFactory = new LeastRecentlyUsedEvictionPolicyFactory(),
		RegisterCount = DEFAULT_REGISTER_COUNT,
		MatrixSizeX = 32,
		MatrixSizeY = 32,
		MatrixStartingOffset = 0,
		MatrixIsRowMajor = true
	},
	new SimulationConfig
	{
		ConfigurationName = "32x32 Small Fully Associative",
		ConfigurationId = "32x32-small-fully",
		CacheLineCount = SMALL_CACHE_SIZE,
		CacheLineSize = DEFAULT_CACHE_LINE_SIZE,
		PlacementPolicyFactory = new FullyAssociativePlacementPolicyFactory(),
		EvictionPolicyFactory = new LeastRecentlyUsedEvictionPolicyFactory(),
		RegisterCount = DEFAULT_REGISTER_COUNT,
		MatrixSizeX = 32,
		MatrixSizeY = 32,
		MatrixStartingOffset = 0,
		MatrixIsRowMajor = true
	},
	new SimulationConfig
	{
		ConfigurationName = "32x32 Medium Direct Mapped",
		ConfigurationId = "32x32-medium-direct",
		CacheLineCount = MEDIUM_CACHE_SIZE,
		CacheLineSize = DEFAULT_CACHE_LINE_SIZE,
		PlacementPolicyFactory = new DirectMappedPlacementPolicyFactory(),
		EvictionPolicyFactory = new LeastRecentlyUsedEvictionPolicyFactory(),
		RegisterCount = DEFAULT_REGISTER_COUNT,
		MatrixSizeX = 32,
		MatrixSizeY = 32,
		MatrixStartingOffset = 0,
		MatrixIsRowMajor = true
	},
	new SimulationConfig
	{
		ConfigurationName = "32x32 Medium 2-Way Associative",
		ConfigurationId = "32x32-medium-2way",
		CacheLineCount = MEDIUM_CACHE_SIZE,
		CacheLineSize = DEFAULT_CACHE_LINE_SIZE,
		PlacementPolicyFactory = new NWayAssociativePlacementPolicyFactory(2),
		EvictionPolicyFactory = new LeastRecentlyUsedEvictionPolicyFactory(),
		RegisterCount = DEFAULT_REGISTER_COUNT,
		MatrixSizeX = 32,
		MatrixSizeY = 32,
		MatrixStartingOffset = 0,
		MatrixIsRowMajor = true
	},
	new SimulationConfig
	{
		ConfigurationName = "32x32 Medium 4-Way Associative",
		ConfigurationId = "32x32-medium-4way",
		CacheLineCount = MEDIUM_CACHE_SIZE,
		CacheLineSize = DEFAULT_CACHE_LINE_SIZE,
		PlacementPolicyFactory = new NWayAssociativePlacementPolicyFactory(4),
		EvictionPolicyFactory = new LeastRecentlyUsedEvictionPolicyFactory(),
		RegisterCount = DEFAULT_REGISTER_COUNT,
		MatrixSizeX = 32,
		MatrixSizeY = 32,
		MatrixStartingOffset = 0,
		MatrixIsRowMajor = true
	},
	new SimulationConfig
	{
		ConfigurationName = "32x32 Medium Fully Associative",
		ConfigurationId = "32x32-medium-fully",
		CacheLineCount = MEDIUM_CACHE_SIZE,
		CacheLineSize = DEFAULT_CACHE_LINE_SIZE,
		PlacementPolicyFactory = new FullyAssociativePlacementPolicyFactory(),
		EvictionPolicyFactory = new LeastRecentlyUsedEvictionPolicyFactory(),
		RegisterCount = DEFAULT_REGISTER_COUNT,
		MatrixSizeX = 32,
		MatrixSizeY = 32,
		MatrixStartingOffset = 0,
		MatrixIsRowMajor = true
	},
	new SimulationConfig
	{
		ConfigurationName = "32x32 Large Direct Mapped",
		ConfigurationId = "32x32-large-direct",
		CacheLineCount = LARGE_CACHE_SIZE,
		CacheLineSize = DEFAULT_CACHE_LINE_SIZE,
		PlacementPolicyFactory = new DirectMappedPlacementPolicyFactory(),
		EvictionPolicyFactory = new LeastRecentlyUsedEvictionPolicyFactory(),
		RegisterCount = DEFAULT_REGISTER_COUNT,
		MatrixSizeX = 32,
		MatrixSizeY = 32,
		MatrixStartingOffset = 0,
		MatrixIsRowMajor = true
	},
	new SimulationConfig
	{
		ConfigurationName = "32x32 Large 2-Way Associative",
		ConfigurationId = "32x32-large-2way",
		CacheLineCount = LARGE_CACHE_SIZE,
		CacheLineSize = DEFAULT_CACHE_LINE_SIZE,
		PlacementPolicyFactory = new NWayAssociativePlacementPolicyFactory(2),
		EvictionPolicyFactory = new LeastRecentlyUsedEvictionPolicyFactory(),
		RegisterCount = DEFAULT_REGISTER_COUNT,
		MatrixSizeX = 32,
		MatrixSizeY = 32,
		MatrixStartingOffset = 0,
		MatrixIsRowMajor = true
	},
	new SimulationConfig
	{
		ConfigurationName = "32x32 Large 4-Way Associative",
		ConfigurationId = "32x32-large-4way",
		CacheLineCount = LARGE_CACHE_SIZE,
		CacheLineSize = DEFAULT_CACHE_LINE_SIZE,
		PlacementPolicyFactory = new NWayAssociativePlacementPolicyFactory(4),
		EvictionPolicyFactory = new LeastRecentlyUsedEvictionPolicyFactory(),
		RegisterCount = DEFAULT_REGISTER_COUNT,
		MatrixSizeX = 32,
		MatrixSizeY = 32,
		MatrixStartingOffset = 0,
		MatrixIsRowMajor = true
	},
	new SimulationConfig
	{
		ConfigurationName = "32x32 Large 8-Way Associative",
		ConfigurationId = "32x32-large-8way",
		CacheLineCount = LARGE_CACHE_SIZE,
		CacheLineSize = DEFAULT_CACHE_LINE_SIZE,
		PlacementPolicyFactory = new NWayAssociativePlacementPolicyFactory(8),
		EvictionPolicyFactory = new LeastRecentlyUsedEvictionPolicyFactory(),
		RegisterCount = DEFAULT_REGISTER_COUNT,
		MatrixSizeX = 32,
		MatrixSizeY = 32,
		MatrixStartingOffset = 0,
		MatrixIsRowMajor = true
	},
	new SimulationConfig
	{
		ConfigurationName = "32x32 Large Fully Associative",
		ConfigurationId = "32x32-large-fully",
		CacheLineCount = LARGE_CACHE_SIZE,
		CacheLineSize = DEFAULT_CACHE_LINE_SIZE,
		PlacementPolicyFactory = new FullyAssociativePlacementPolicyFactory(),
		EvictionPolicyFactory = new LeastRecentlyUsedEvictionPolicyFactory(),
		RegisterCount = DEFAULT_REGISTER_COUNT,
		MatrixSizeX = 32,
		MatrixSizeY = 32,
		MatrixStartingOffset = 0,
		MatrixIsRowMajor = true
	},
	new SimulationConfig
	{
		ConfigurationName = "32x32 XL Direct Mapped",
		ConfigurationId = "32x32-xl-direct",
		CacheLineCount = XL_CACHE_SIZE,
		CacheLineSize = DEFAULT_CACHE_LINE_SIZE,
		PlacementPolicyFactory = new DirectMappedPlacementPolicyFactory(),
		EvictionPolicyFactory = new LeastRecentlyUsedEvictionPolicyFactory(),
		RegisterCount = DEFAULT_REGISTER_COUNT,
		MatrixSizeX = 32,
		MatrixSizeY = 32,
		MatrixStartingOffset = 0,
		MatrixIsRowMajor = true
	},
	new SimulationConfig
	{
		ConfigurationName = "32x32 XL 2-Way Associative",
		ConfigurationId = "32x32-xl-2way",
		CacheLineCount = XL_CACHE_SIZE,
		CacheLineSize = DEFAULT_CACHE_LINE_SIZE,
		PlacementPolicyFactory = new NWayAssociativePlacementPolicyFactory(2),
		EvictionPolicyFactory = new LeastRecentlyUsedEvictionPolicyFactory(),
		RegisterCount = DEFAULT_REGISTER_COUNT,
		MatrixSizeX = 32,
		MatrixSizeY = 32,
		MatrixStartingOffset = 0,
		MatrixIsRowMajor = true
	},
	new SimulationConfig
	{
		ConfigurationName = "32x32 XL 4-Way Associative",
		ConfigurationId = "32x32-xl-4way",
		CacheLineCount = XL_CACHE_SIZE,
		CacheLineSize = DEFAULT_CACHE_LINE_SIZE,
		PlacementPolicyFactory = new NWayAssociativePlacementPolicyFactory(4),
		EvictionPolicyFactory = new LeastRecentlyUsedEvictionPolicyFactory(),
		RegisterCount = DEFAULT_REGISTER_COUNT,
		MatrixSizeX = 32,
		MatrixSizeY = 32,
		MatrixStartingOffset = 0,
		MatrixIsRowMajor = true
	},
	new SimulationConfig
	{
		ConfigurationName = "32x32 XL 8-Way Associative",
		ConfigurationId = "32x32-xl-8way",
		CacheLineCount = XL_CACHE_SIZE,
		CacheLineSize = DEFAULT_CACHE_LINE_SIZE,
		PlacementPolicyFactory = new NWayAssociativePlacementPolicyFactory(8),
		EvictionPolicyFactory = new LeastRecentlyUsedEvictionPolicyFactory(),
		RegisterCount = DEFAULT_REGISTER_COUNT,
		MatrixSizeX = 32,
		MatrixSizeY = 32,
		MatrixStartingOffset = 0,
		MatrixIsRowMajor = true
	},
	new SimulationConfig
	{
		ConfigurationName = "32x32 XL 16-Way Associative",
		ConfigurationId = "32x32-xl-16way",
		CacheLineCount = XL_CACHE_SIZE,
		CacheLineSize = DEFAULT_CACHE_LINE_SIZE,
		PlacementPolicyFactory = new NWayAssociativePlacementPolicyFactory(16),
		EvictionPolicyFactory = new LeastRecentlyUsedEvictionPolicyFactory(),
		RegisterCount = DEFAULT_REGISTER_COUNT,
		MatrixSizeX = 32,
		MatrixSizeY = 32,
		MatrixStartingOffset = 0,
		MatrixIsRowMajor = true
	},
	new SimulationConfig
	{
		ConfigurationName = "32x32 XL Fully Associative",
		ConfigurationId = "32x32-xl-fully",
		CacheLineCount = XL_CACHE_SIZE,
		CacheLineSize = DEFAULT_CACHE_LINE_SIZE,
		PlacementPolicyFactory = new FullyAssociativePlacementPolicyFactory(),
		EvictionPolicyFactory = new LeastRecentlyUsedEvictionPolicyFactory(),
		RegisterCount = DEFAULT_REGISTER_COUNT,
		MatrixSizeX = 32,
		MatrixSizeY = 32,
		MatrixStartingOffset = 0,
		MatrixIsRowMajor = true
	},
};
IAgentFactory[] AGENT_FACTORIES = {
	new NaiveAgentFactory()
};

// Evaluate each agent
var agentResults = new List<AgentResults>();
var runner = new SequentialLocalTestRunner(
	() =>
	{
		return new ConstRewardComponent(
			CACHE_HIT_REWARD,
			CACHE_MISS_REWARD,
			MEMORY_ACCESS_REWARD
		);
	}
);
foreach (var agentFactory in AGENT_FACTORIES)
{
	var runs = runner.RunSimulations(agentFactory, CONFIGS);
	agentResults.Add(new AgentResults
	{
		AgentName = agentFactory.AgentName,
		AgentId = agentFactory.AgentId,
		SimulationRuns = new Dictionary<string, SimulationRun>(
			runs.Select(run => new KeyValuePair<string, SimulationRun>(
				run.Config.ConfigurationId,
				run
			))
		)
	});
}

// Report results
var reporter = new ConsoleStatsReporter();
reporter.ReportResults(agentResults);
