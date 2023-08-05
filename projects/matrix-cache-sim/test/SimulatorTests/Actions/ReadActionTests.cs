/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator;
using Mcs.Simulator.Actions;
using Mcs.Simulator.Simulation;

namespace McsTests.Common.Actions;

public class ReadActionTests
{
	[Theory]
	[InlineData(1, 2)]
	[InlineData(2, 1)]
	public void CtorSetsProperties(int address, int registerIndex)
	{
		var action = new ReadAction(address, registerIndex);

		Assert.True(action.IsRead);
		Assert.Equal(address, action.Address);
		Assert.Equal(registerIndex, action.RegisterIndex);
	}

	[Theory]
	[InlineData(-1, 0)]
	[InlineData(int.MinValue, 1)]
	public void CtorThrowsIfAddressIsNegative(
		int address,
		int registerIndex)
	{
		Assert.Throws<ArgumentOutOfRangeException>(
			() => new ReadAction(address, registerIndex)
		);
	}

	[Theory]
	[InlineData(0, -1)]
	[InlineData(1, int.MinValue)]
	public void CtorThrowsIfRegisterIndexIsNegative(
		int address,
		int registerIndex)
	{
		Assert.Throws<ArgumentOutOfRangeException>(
			() => new ReadAction(address, registerIndex)
		);
	}

	[Fact]
	public void ReadIntoRegisterFromSimulator()
	{
		const int ADDRESS = 5;
		const int REGISTER_COUNT = 4;
		const int INDEX = 1;
		const int VALUE = 0xF00;
		var simulator = new Mock<ISimulator>();
		simulator.Setup(s => s.Read(ADDRESS)).Returns(VALUE);
		var registers = Enumerable.Range(0, REGISTER_COUNT)
			.Select(i => new Register(i))
			.ToArray();
		var action = new ReadAction(ADDRESS, INDEX);
		action.ApplyAction(simulator.Object, registers);

		// Make sure the expected methods were invoked
		simulator.Verify(
			s => s.Read(ADDRESS),
			Times.Once
		);
		Assert.Equal(VALUE, registers[INDEX].Value);
	}

	[Fact]
	public void ReadIntoRegisterFromMatrix()
	{
		const int ADDRESS = 1;
		const int REGISTER_COUNT = 4;
		const int INDEX = 1;
		const int X = 2;
		const int Y = 2;

		var matrix = new Mock<IMatrix>();
		matrix.SetupGet(m => m.X).Returns(X);
		matrix.SetupGet(m => m.Y).Returns(Y);
		matrix.SetupGet(m => m.StartingAddress).Returns(0);
		matrix.SetupGet(m => m.EndingAddress).Returns(X * Y);

		var registers = Enumerable.Range(0, REGISTER_COUNT)
			.Select(i => new Register(i))
			.ToArray();
		var action = new ReadAction(ADDRESS, INDEX);
		action.ApplyAction(matrix.Object, registers);

		// Make sure the expected methods were invoked
		var (x, y) = matrix.Object.ToMatrixCoordinate(ADDRESS);
		matrix.Verify(
			m => m.Read(x, y, registers[INDEX]),
			Times.Once
		);
	}
}
