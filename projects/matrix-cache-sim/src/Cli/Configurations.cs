/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Cli.Results;
using Mcs.Simulator.Policies.Eviction;
using Mcs.Simulator.Policies.Placement;
namespace Mcs.Cli;

/// Defines the various configurations that agents may be tested under.
public static class Configurations
{
	/// Dictionary of all configurations, indexed by configuration ID.
	public static IReadOnlyDictionary<string, SimulationConfig> Configs =>
		MakeConfigsDict();

	/// Register count used for the majority of configurations.
	private const int DEFAULT_REGISTER_COUNT = 8;

	/// Elements per cache line used for the majority of configurations.
	private const int DEFAULT_CACHE_LINE_SIZE = 8;

	/// Number of cache lines present in small cache configurations.
	private const int SMALL_CACHE_SIZE = 4;

	/// Number of cache lines present in medium cache configurations.
	private const int MEDIUM_CACHE_SIZE = 8;

	/// Number of cache lines present in large cache configurations.
	private const int LARGE_CACHE_SIZE = 16;

	/// Number of cache lines present in extra large cache configurations.
	private const int XL_CACHE_SIZE = 32;

	/// Helper method used to create the dictionary of configurations.
	private static IReadOnlyDictionary<string, SimulationConfig> MakeConfigsDict()
	{
		const string CONFIG_32X32_SMALL_DIRECT_ID = "32x32-small-direct";
		const string CONFIG_32X32_SMALL_2WAY_ID = "32x32-small-2way";
		const string CONFIG_32X32_SMALL_FULLY_ID = "32x32-small-fully";
		const string CONFIG_32X32_MEDIUM_DIRECT_ID = "32x32-medium-direct";
		const string CONFIG_32X32_MEDIUM_2WAY_ID = "32x32-medium-2way";
		const string CONFIG_32X32_MEDIUM_4WAY_ID = "32x32-medium-4way";
		const string CONFIG_32X32_MEDIUM_FULLY_ID = "32x32-medium-fully";
		const string CONFIG_32X32_LARGE_DIRECT_ID = "32x32-large-direct";
		const string CONFIG_32X32_LARGE_2WAY_ID = "32x32-large-2way";
		const string CONFIG_32X32_LARGE_4WAY_ID = "32x32-large-4way";
		const string CONFIG_32X32_LARGE_8WAY_ID = "32x32-large-8way";
		const string CONFIG_32X32_LARGE_FULLY_ID = "32x32-large-fully";
		const string CONFIG_32X32_XL_DIRECT_ID = "32x32-xl-direct";
		const string CONFIG_32X32_XL_2WAY_ID = "32x32-xl-2way";
		const string CONFIG_32X32_XL_4WAY_ID = "32x32-xl-4way";
		const string CONFIG_32X32_XL_8WAY_ID = "32x32-xl-8way";
		const string CONFIG_32X32_XL_16WAY_ID = "32x32-xl-16way";
		const string CONFIG_32X32_XL_FULLY_ID = "32x32-xl-fully";

		const string CONFIG_64X64_SMALL_DIRECT_ID = "64x64-small-direct";
		const string CONFIG_64X64_SMALL_2WAY_ID = "64x64-small-2way";
		const string CONFIG_64X64_SMALL_FULLY_ID = "64x64-small-fully";
		const string CONFIG_64X64_MEDIUM_DIRECT_ID = "64x64-medium-direct";
		const string CONFIG_64X64_MEDIUM_2WAY_ID = "64x64-medium-2way";
		const string CONFIG_64X64_MEDIUM_4WAY_ID = "64x64-medium-4way";
		const string CONFIG_64X64_MEDIUM_FULLY_ID = "64x64-medium-fully";
		const string CONFIG_64X64_LARGE_DIRECT_ID = "64x64-large-direct";
		const string CONFIG_64X64_LARGE_2WAY_ID = "64x64-large-2way";
		const string CONFIG_64X64_LARGE_4WAY_ID = "64x64-large-4way";
		const string CONFIG_64X64_LARGE_8WAY_ID = "64x64-large-8way";
		const string CONFIG_64X64_LARGE_FULLY_ID = "64x64-large-fully";
		const string CONFIG_64X64_XL_DIRECT_ID = "64x64-xl-direct";
		const string CONFIG_64X64_XL_2WAY_ID = "64x64-xl-2way";
		const string CONFIG_64X64_XL_4WAY_ID = "64x64-xl-4way";
		const string CONFIG_64X64_XL_8WAY_ID = "64x64-xl-8way";
		const string CONFIG_64X64_XL_16WAY_ID = "64x64-xl-16way";
		const string CONFIG_64X64_XL_FULLY_ID = "64x64-xl-fully";

		return new Dictionary<string, SimulationConfig>()
		{
			{
				CONFIG_32X32_SMALL_DIRECT_ID,
				Make32x32RowMajorConfig(
					"32x32 Small Direct Mapped",
					CONFIG_32X32_SMALL_DIRECT_ID,
					SMALL_CACHE_SIZE,
					1
				)
			},
			{
				CONFIG_32X32_SMALL_2WAY_ID,
				Make32x32RowMajorConfig(
					"32x32 Small 2-Way Set Associative",
					CONFIG_32X32_SMALL_2WAY_ID,
					SMALL_CACHE_SIZE,
					2
				)
			},
			{
				CONFIG_32X32_SMALL_FULLY_ID,
				Make32x32RowMajorConfig(
					"32x32 Small Fully Associative",
					CONFIG_32X32_SMALL_FULLY_ID,
					SMALL_CACHE_SIZE,
					SMALL_CACHE_SIZE
				)
			},
			{
				CONFIG_32X32_MEDIUM_DIRECT_ID,
				Make32x32RowMajorConfig(
					"32x32 Medium Direct Mapped",
					CONFIG_32X32_MEDIUM_DIRECT_ID,
					MEDIUM_CACHE_SIZE,
					1
				)
			},
			{
				CONFIG_32X32_MEDIUM_2WAY_ID,
				Make32x32RowMajorConfig(
					"32x32 Medium 2-Way Set Associative",
					CONFIG_32X32_MEDIUM_2WAY_ID,
					MEDIUM_CACHE_SIZE,
					2
				)
			},
			{
				CONFIG_32X32_MEDIUM_4WAY_ID,
				Make32x32RowMajorConfig(
					"32x32 Medium 4-Way Set Associative",
					CONFIG_32X32_MEDIUM_4WAY_ID,
					MEDIUM_CACHE_SIZE,
					4
				)
			},
			{
				CONFIG_32X32_MEDIUM_FULLY_ID,
				Make32x32RowMajorConfig(
					"32x32 Medium Fully Associative",
					CONFIG_32X32_MEDIUM_FULLY_ID,
					MEDIUM_CACHE_SIZE,
					MEDIUM_CACHE_SIZE
				)
			},
			{
				CONFIG_32X32_LARGE_DIRECT_ID,
				Make32x32RowMajorConfig(
					"32x32 Large Direct Mapped",
					CONFIG_32X32_LARGE_DIRECT_ID,
					LARGE_CACHE_SIZE,
					1
				)
			},
			{
				CONFIG_32X32_LARGE_2WAY_ID,
				Make32x32RowMajorConfig(
					"32x32 Large 2-Way Set Associative",
					CONFIG_32X32_LARGE_2WAY_ID,
					LARGE_CACHE_SIZE,
					2
				)
			},
			{
				CONFIG_32X32_LARGE_4WAY_ID,
				Make32x32RowMajorConfig(
					"32x32 Large 4-Way Set Associative",
					CONFIG_32X32_LARGE_4WAY_ID,
					LARGE_CACHE_SIZE,
					4
				)
			},
			{
				CONFIG_32X32_LARGE_8WAY_ID,
				Make32x32RowMajorConfig(
					"32x32 Large 8-Way Set Associative",
					CONFIG_32X32_LARGE_8WAY_ID,
					LARGE_CACHE_SIZE,
					8
				)
			},
			{
				CONFIG_32X32_LARGE_FULLY_ID,
				Make32x32RowMajorConfig(
					"32x32 Large Fully Associative",
					CONFIG_32X32_LARGE_FULLY_ID,
					LARGE_CACHE_SIZE,
					LARGE_CACHE_SIZE
				)
			},
			{
				CONFIG_32X32_XL_DIRECT_ID,
				Make32x32RowMajorConfig(
					"32x32 Extra Large Direct Mapped",
					CONFIG_32X32_XL_DIRECT_ID,
					XL_CACHE_SIZE,
					1
				)
			},
			{
				CONFIG_32X32_XL_2WAY_ID,
				Make32x32RowMajorConfig(
					"32x32 Extra Large 2-Way Set Associative",
					CONFIG_32X32_XL_2WAY_ID,
					XL_CACHE_SIZE,
					2
				)
			},
			{
				CONFIG_32X32_XL_4WAY_ID,
				Make32x32RowMajorConfig(
					"32x32 Extra Large 4-Way Set Associative",
					CONFIG_32X32_XL_4WAY_ID,
					XL_CACHE_SIZE,
					4
				)
			},
			{
				CONFIG_32X32_XL_8WAY_ID,
				Make32x32RowMajorConfig(
					"32x32 Extra Large 8-Way Set Associative",
					CONFIG_32X32_XL_8WAY_ID,
					XL_CACHE_SIZE,
					8
				)
			},
			{
				CONFIG_32X32_XL_16WAY_ID,
				Make32x32RowMajorConfig(
					"32x32 Extra Large 16-Way Set Associative",
					CONFIG_32X32_XL_16WAY_ID,
					XL_CACHE_SIZE,
					16
				)
			},
			{
				CONFIG_32X32_XL_FULLY_ID,
				Make32x32RowMajorConfig(
					"32x32 Extra Large Fully Associative",
					CONFIG_32X32_XL_FULLY_ID,
					XL_CACHE_SIZE,
					XL_CACHE_SIZE
				)
			},
			{
				CONFIG_64X64_SMALL_DIRECT_ID,
				Make64x64RowMajorConfig(
					"64x64 Small Direct Mapped",
					CONFIG_64X64_SMALL_DIRECT_ID,
					SMALL_CACHE_SIZE,
					1
				)
			},
			{
				CONFIG_64X64_SMALL_2WAY_ID,
				Make64x64RowMajorConfig(
					"64x64 Small 2-Way Set Associative",
					CONFIG_64X64_SMALL_2WAY_ID,
					SMALL_CACHE_SIZE,
					2
				)
			},
			{
				CONFIG_64X64_SMALL_FULLY_ID,
				Make64x64RowMajorConfig(
					"64x64 Small Fully Associative",
					CONFIG_64X64_SMALL_FULLY_ID,
					SMALL_CACHE_SIZE,
					SMALL_CACHE_SIZE
				)
			},
			{
				CONFIG_64X64_MEDIUM_DIRECT_ID,
				Make64x64RowMajorConfig(
					"64x64 Medium Direct Mapped",
					CONFIG_64X64_MEDIUM_DIRECT_ID,
					MEDIUM_CACHE_SIZE,
					1
				)
			},
			{
				CONFIG_64X64_MEDIUM_2WAY_ID,
				Make64x64RowMajorConfig(
					"64x64 Medium 2-Way Set Associative",
					CONFIG_64X64_MEDIUM_2WAY_ID,
					MEDIUM_CACHE_SIZE,
					2
				)
			},
			{
				CONFIG_64X64_MEDIUM_4WAY_ID,
				Make64x64RowMajorConfig(
					"64x64 Medium 4-Way Set Associative",
					CONFIG_64X64_MEDIUM_4WAY_ID,
					MEDIUM_CACHE_SIZE,
					4
				)
			},
			{
				CONFIG_64X64_MEDIUM_FULLY_ID,
				Make64x64RowMajorConfig(
					"64x64 Medium Fully Associative",
					CONFIG_64X64_MEDIUM_FULLY_ID,
					MEDIUM_CACHE_SIZE,
					MEDIUM_CACHE_SIZE
				)
			},
			{
				CONFIG_64X64_LARGE_DIRECT_ID,
				Make64x64RowMajorConfig(
					"64x64 Large Direct Mapped",
					CONFIG_64X64_LARGE_DIRECT_ID,
					LARGE_CACHE_SIZE,
					1
				)
			},
			{
				CONFIG_64X64_LARGE_2WAY_ID,
				Make64x64RowMajorConfig(
					"64x64 Large 2-Way Set Associative",
					CONFIG_64X64_LARGE_2WAY_ID,
					LARGE_CACHE_SIZE,
					2
				)
			},
			{
				CONFIG_64X64_LARGE_4WAY_ID,
				Make64x64RowMajorConfig(
					"64x64 Large 4-Way Set Associative",
					CONFIG_64X64_LARGE_4WAY_ID,
					LARGE_CACHE_SIZE,
					4
				)
			},
			{
				CONFIG_64X64_LARGE_8WAY_ID,
				Make64x64RowMajorConfig(
					"64x64 Large 8-Way Set Associative",
					CONFIG_64X64_LARGE_8WAY_ID,
					LARGE_CACHE_SIZE,
					8
				)
			},
			{
				CONFIG_64X64_LARGE_FULLY_ID,
				Make64x64RowMajorConfig(
					"64x64 Large Fully Associative",
					CONFIG_64X64_LARGE_FULLY_ID,
					LARGE_CACHE_SIZE,
					LARGE_CACHE_SIZE
				)
			},
			{
				CONFIG_64X64_XL_DIRECT_ID,
				Make64x64RowMajorConfig(
					"64x64 Extra Large Direct Mapped",
					CONFIG_64X64_XL_DIRECT_ID,
					XL_CACHE_SIZE,
					1
				)
			},
			{
				CONFIG_64X64_XL_2WAY_ID,
				Make64x64RowMajorConfig(
					"64x64 Extra Large 2-Way Set Associative",
					CONFIG_64X64_XL_2WAY_ID,
					XL_CACHE_SIZE,
					2
				)
			},
			{
				CONFIG_64X64_XL_4WAY_ID,
				Make64x64RowMajorConfig(
					"64x64 Extra Large 4-Way Set Associative",
					CONFIG_64X64_XL_4WAY_ID,
					XL_CACHE_SIZE,
					4
				)
			},
			{
				CONFIG_64X64_XL_8WAY_ID,
				Make64x64RowMajorConfig(
					"64x64 Extra Large 8-Way Set Associative",
					CONFIG_64X64_XL_8WAY_ID,
					XL_CACHE_SIZE,
					8
				)
			},
			{
				CONFIG_64X64_XL_16WAY_ID,
				Make64x64RowMajorConfig(
					"64x64 Extra Large 16-Way Set Associative",
					CONFIG_64X64_XL_16WAY_ID,
					XL_CACHE_SIZE,
					16
				)
			},
			{
				CONFIG_64X64_XL_FULLY_ID,
				Make64x64RowMajorConfig(
					"64x64 Extra Large Fully Associative",
					CONFIG_64X64_XL_FULLY_ID,
					XL_CACHE_SIZE,
					XL_CACHE_SIZE
				)
			}
		};
	}

	/// Helper method used to create a 32x32 row-major configuration.
	/// @param configName Name of the configuration.
	/// @param configId ID of the configuration.
	/// @param cacheSize Number of cache lines in the cache.
	/// @param associativity Number of ways in the cache.
	/// @return The configuration.
	private static SimulationConfig Make32x32RowMajorConfig(
		string configName,
		string configId,
		int cacheSize,
		int associativity)
	{
		return new SimulationConfig()
		{
			ConfigurationName = configName,
			ConfigurationId = configId,
			CacheLineCount = cacheSize,
			CacheLineSize = DEFAULT_CACHE_LINE_SIZE,
			PlacementPolicyFactory = new NWayAssociativePlacementPolicyFactory(
				associativity
			),
			EvictionPolicyFactory = new LeastRecentlyUsedEvictionPolicyFactory(),
			RegisterCount = DEFAULT_REGISTER_COUNT,
			MatrixSizeX = 32,
			MatrixSizeY = 32,
			MatrixStartingOffset = 0,
			MatrixIsRowMajor = true
		};
	}

	/// Helper method used to create a 64x64 row-major configuration.
	/// @param configName Name of the configuration.
	/// @param configId ID of the configuration.
	/// @param cacheSize Number of cache lines in the cache.
	/// @param associativity Number of ways in the cache.
	/// @return The configuration.
	private static SimulationConfig Make64x64RowMajorConfig(
		string configName,
		string configId,
		int cacheSize,
		int associativity)
	{
		return new SimulationConfig()
		{
			ConfigurationName = configName,
			ConfigurationId = configId,
			CacheLineCount = cacheSize,
			CacheLineSize = DEFAULT_CACHE_LINE_SIZE,
			PlacementPolicyFactory = new NWayAssociativePlacementPolicyFactory(
				associativity
			),
			EvictionPolicyFactory = new LeastRecentlyUsedEvictionPolicyFactory(),
			RegisterCount = DEFAULT_REGISTER_COUNT,
			MatrixSizeX = 64,
			MatrixSizeY = 64,
			MatrixStartingOffset = 0,
			MatrixIsRowMajor = true
		};
	}
}
