/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.Simulation;
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

	/// Applies the action to the given matrix and registers.
	/// @param simulator Simulator to apply the action to.
	/// @param registers Registers to apply the action to.
	public override void ApplyAction(
		ISimulator simulator,
		IReadOnlyList<Register> registers)
	{
		// Write the value from the source register into the matrix
		simulator.Write(Address, registers[RegisterIndex].Value);
	}
}
