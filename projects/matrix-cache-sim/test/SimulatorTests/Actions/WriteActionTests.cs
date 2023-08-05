/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator;
using Mcs.Simulator.Actions;
using Mcs.Simulator.Simulation;

namespace McsTests.Common.Actions;

public class WriteActionTests
{
	[Theory]
	[InlineData(1, 2)]
	[InlineData(2, 1)]
	public void CtorSetsProperties(int address, int registerIndex)
	{
		var action = new WriteAction(address, registerIndex);

		Assert.False(action.IsRead);
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
			() => new WriteAction(address, registerIndex)
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
			() => new WriteAction(address, registerIndex)
		);
	}

	[Fact]
	public void WriteFromRegister()
	{
		const int ADDRESS = 5;
		const int REGISTER_COUNT = 4;
		const int INDEX = 1;
		const int VALUE = 0xF00;
		var simulator = new Mock<ISimulator>();
		var registers = Enumerable.Range(0, REGISTER_COUNT)
			.Select(i => new Register(i))
			.ToArray();
		registers[INDEX].SetValue(VALUE, ADDRESS);
		var action = new WriteAction(ADDRESS, INDEX);
		action.ApplyAction(simulator.Object, registers);

		// Make sure the expected methods were invoked
		simulator.Verify(
			s => s.Write(ADDRESS, VALUE),
			Times.Once
		);
	}
}
