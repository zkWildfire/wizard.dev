/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator;
using Mcs.Simulator.Actions;
using Mcs.Simulator.Simulation;
namespace McsTests.Simulator.Actions;

public class ActionStaticsTests
{
	[Fact]
	public void CodeCoverageOnly()
	{
		var action = new Mock<IAction>(true, 0, 0);
		var simulator = new Mock<ISimulator>();
		var registers = new Mock<IReadOnlyList<Register>>();
		var matrix = new Mock<IMatrix>();

		action.Object.ApplyAction(simulator.Object, registers.Object);
		action.Object.ApplyAction(matrix.Object, registers.Object);
	}
}
