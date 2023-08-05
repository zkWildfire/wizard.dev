/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.Actions;
namespace Mcs.Agents;

/// Interface for classes that implement a matrix transposition agent.
public interface IAgent
{
	/// Generates the next action to take.
	/// @returns The next action to take.
	IEnumerable<IAction> GetNextAction();

	/// Provides feedback to the agent about the result of the last action.
	/// @param action Action that the result data is for.
	/// @param result Result of the last action.
	void NotifyResult(IAction action, Result result);
}
