/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Common.Simulation;

/// Interface for classes that simulate a CPU register.
public interface IRegister
{
	/// Value currently held in the register.
	int Value { get; }

	/// Memory address that the value in the register was read from.
	int Address { get; }

	/// Sets the value of the register.
	/// @param value Value to set the register to.
	/// @param address Memory address that the value was read from.
	/// @throws ArgumentOutOfRangeException If the address is negative.
	void SetValue(int value, int address);
}
