/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.Actions;
namespace Mcs.Cli.Rewards;

/// Reward component that provides a flat reward for different types of actions.
public class ConstRewardComponent : IRewardComponent
{
	/// Reward for a cache hit.
	private readonly int _cacheHitReward;

	/// Reward for a cache miss.
	private readonly int _cacheMissReward;

	/// Reward for a memory access.
	private readonly int _memoryAccessReward;

	/// Initializes the reward component.
	/// All reward values may be set to a negative value.
	/// @param cacheHitReward Reward for a cache hit.
	/// @param cacheMissReward Reward for a cache miss.
	/// @param memoryAccessReward Reward for a memory access.
	public ConstRewardComponent(
		int cacheHitReward,
		int cacheMissReward,
		int memoryAccessReward)
	{
		_cacheHitReward = cacheHitReward;
		_cacheMissReward = cacheMissReward;
		_memoryAccessReward = memoryAccessReward;
	}

	/// Generates a reward for an agent's action.
	/// @param action Action that the reward is for.
	/// @param result Result of the action.
	public int GetReward(IAction action, Result result)
	{
		return result switch
		{
			{ CacheHit: true } => _cacheHitReward,
			{ CacheMiss: true } => _cacheMissReward,
			_ => _memoryAccessReward
		};
	}
}
