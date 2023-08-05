/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.Actions;
using Mcs.Simulator.Simulation;
using static Mcs.Simulator.Simulation.MatrixStatics;
namespace Mcs.Agents.Naive;

/// Agent that implements a naive matrix transpose algorithm.
public class NaiveAgent : IAgent
{
	/// Matrix that the agent is transposing.
	private readonly IMatrix _matrix;

	/// Initializes a new instance of the class.
	/// @param matrix Matrix that the agent is transposing.
	/// @param registerCount Number of registers that the agent has access to.
	/// @throws ArgumentOutOfRangeException If the register count is less than
	///   two.
	public NaiveAgent(IMatrix matrix, int registerCount)
	{
		if (registerCount < 2)
		{
			throw new ArgumentOutOfRangeException(
				nameof(registerCount),
				registerCount,
				"The naive agent requires at least two registers."
			);
		}

		_matrix = matrix;
	}

	/// Generates the next action to take.
	/// @returns The next action to take.
	public IEnumerable<IAction> GetNextAction()
	{
		for (var y = 0; y < _matrix.Y; y++)
		{
			for (var x = 0; x < _matrix.X; x++)
			{
				// Only generate operations for one half of the matrix
				if (x <= y)
				{
					continue;
				}

				// Generate the actions to swap the two cells
				foreach (var action in _matrix.Swap(x, y, 0, y, x, 1))
				{
					yield return action;
				}
			}
		}
	}

	/// Provides feedback to the agent about the result of the last action.
	/// @param action Action that the result data is for.
	/// @param result Result of the last action.
	public void NotifyResult(IAction action, Result result)
	{
		// Do nothing
	}
}
