/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Agents.Naive;
using Mcs.Simulator.Actions;
using Mcs.Simulator.Memory;
using Mcs.Simulator.Simulation;
using Mcs.Simulator.Validators;
namespace McsTests.Agents.Naive;

public class NaiveAgentTests
{
	[Theory]
	[InlineData(0)]
	[InlineData(1)]
	public void CtorThrowsIfInsufficientRegisters(int registerCount)
	{
		Assert.Throws<ArgumentOutOfRangeException>(
			() => new NaiveAgent(Mock.Of<IMatrix>(), registerCount)
		);
	}

	[Theory]
	[InlineData(2, 2, 0)]
	[InlineData(2, 2, 1)]
	[InlineData(3, 3, 0)]
	[InlineData(3, 3, 4)]
	[InlineData(4, 4, 0)]
	[InlineData(4, 4, 1)]
	[InlineData(4, 4, 2)]
	[InlineData(4, 4, 4)]
	public void AgentSuccessfullyTransposesRowMajorMatrix(
		int matrixX,
		int matrixY,
		int startingOffset)
	{
		// Create the matrix and supporting components
		var memory = new ArrayMemory((matrixX * matrixY) + startingOffset);
		var matrix = new RowMajorMatrix(
			memory,
			matrixX,
			matrixY,
			startingOffset
		);
		var validator = new SequentialMemoryValidator();
		validator.Initialize(memory, matrix);

		const int REGISTER_COUNT = 2;
		var registers = Enumerable.Range(0, REGISTER_COUNT)
			.Select(i => new Register(i))
			.ToArray();

		// Since the naive agent only ever touches matrix cells once, the upper
		//   bound on the number of steps the agent requires to transpose the
		//   matrix is known
		// Note that because the agent performs an in-place transpose, the
		//   number of steps it should take should be half of what's calculated
		//   here. However, since the purpose of this value is only to ensure
		//   that this test can't get stuck in an infinite loop, it's not an
		//   issue to have this higher than the actual number of steps required.
		var maxSteps = matrixX * matrixY;

		// Create the agent and transpose the matrix
		var agent = new NaiveAgent(matrix, REGISTER_COUNT);
		var steps = 0;
		while (steps < maxSteps)
		{
			// Get the next action
			var actions = agent.GetActions().ToList();
			Assert.NotEmpty(actions);

			// Run each action
			foreach (var action in actions)
			{
				// Run the action
				action.ApplyAction(matrix, registers);
				steps++;
			}

			if (validator.Validate(memory, matrix))
			{
				break;
			}
		}

		Assert.True(validator.Validate(memory, matrix));
	}
}
