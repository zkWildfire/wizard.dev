/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Simulator.Actions;

/// Represents a read operation.
public class ReadAction : IAction
{
	/// Creates a new read action.
	/// @param address Address of the memory location to read from.
	/// @param registerIndex Index of the register to read the value into.
	/// @throws ArgumentOutOfRangeException If the address or register index is
	///   negative.
	public ReadAction(int address, int registerIndex)
		: base(true, address, registerIndex)
	{
	}
}
