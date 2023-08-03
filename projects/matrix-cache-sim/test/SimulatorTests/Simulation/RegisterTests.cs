/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.Simulation;
namespace McsTests.Simulator.Simulation;

public class RegisterTests
{
	[Fact]
	public void CtorSetsProperties()
	{
		const int VALUE = 1;
		const int ADDRESS = 2;

		var register = new Register(VALUE, ADDRESS);

		Assert.Equal(VALUE, register.Value);
		Assert.Equal(ADDRESS, register.Address);
	}

	[Theory]
	[InlineData(0)]
	[InlineData(1)]
	[InlineData(-1)]
	public void SetValue(int value)
	{
		var register = new Register();
		register.SetValue(value, 0);
		Assert.Equal(value, register.Value);
	}

	[Theory]
	[InlineData(0)]
	[InlineData(1)]
	public void SetValidAddress(int address)
	{
		var register = new Register();
		register.SetValue(0, address);
		Assert.Equal(address, register.Address);
	}

	[Theory]
	[InlineData(-1)]
	public void SetInvalidAddress(int address)
	{
		var register = new Register();
		Assert.Throws<ArgumentOutOfRangeException>(() =>
		{
			register.SetValue(0, address);
		});
	}
}
