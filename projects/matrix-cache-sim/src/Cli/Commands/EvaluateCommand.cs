/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Common.Agents;
using Mcs.Common.Simulation;
namespace Mcs.Cli.Commands;

/// Runs an agent on a variety of scenarios and evaluates its performance.
public class EvaluateCommand
{
	/// Factory function that creates an agent.
	/// This will be passed the matrix that the run will use and the number of
	/// registers that the agent has access to.
	private readonly Func<IMatrix, int, IAgent> _agentFactory;

	/// Initializes the command.
	/// @param agentFactory Factory function that creates an agent. This will
	///   be passed the matrix that the run will use and the number of registers
	///   that the agent has access to.
	public EvaluateCommand(Func<IMatrix, int, IAgent> agentFactory)
	{
		_agentFactory = agentFactory;
	}
}
