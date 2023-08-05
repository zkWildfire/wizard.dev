/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Simulator.Simulation;

/// Class that simulates a CPU register.
public class Register
{
	/// Value currently held in the register.
	public int Value { get; private set; }

	/// Memory address that the value in the register was read from.
	public int Address { get; private set; }

	/// Initializes the register.
	/// @param value Value to set the register to.
	/// @param address Memory address that the value was read from.
	public Register(int value = 0, int address = 0)
	{
		SetValue(value, address);
	}

	/// Sets the value of the register.
	/// @param value Value to set the register to.
	/// @param address Memory address that the value was read from.
	/// @throws ArgumentOutOfRangeException If the address is negative.
	public void SetValue(int value, int address)
	{
		if (address < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(address));
		}

		Value = value;
		Address = address;
	}
}
