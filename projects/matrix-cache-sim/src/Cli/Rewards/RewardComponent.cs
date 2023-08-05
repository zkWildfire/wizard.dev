/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.Actions;
namespace Mcs.Cli.Rewards;

/// Interface for components that generate a reward for an agent's actions.
public interface IRewardComponent
{
	/// Generates a reward for an agent's action.
	/// @param action Action that the reward is for.
	/// @param result Result of the action. Should have all fields except the
	///   reward field set.
	/// @returns The reward for the action.
	public int GetReward(IAction action, Result result);
}
