/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Simulator.Actions;

/// Represents a write operation.
public class WriteAction : IAction
{
	/// Creates a new write action.
	/// @param address Address of the memory location to write to.
	/// @param registerIndex Index of the register with the value to write.
	/// @throws ArgumentOutOfRangeException If the address or register index is
	///   negative.
	public WriteAction(int address, int registerIndex)
		: base(false, address, registerIndex)
	{
	}
}
