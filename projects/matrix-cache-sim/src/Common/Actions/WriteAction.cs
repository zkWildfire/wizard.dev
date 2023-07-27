/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Common.Simulation;
namespace Mcs.Common.Actions;

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
	/// @param matrix Matrix to apply the action to.
	/// @param registers Registers to apply the action to.
	public override void ApplyAction(IMatrix matrix, IRegister[] registers)
	{
		// Write the value from the source register into the matrix
		var (x, y) = matrix.ToMatrixCoordinate(Address);
		matrix.Write(x, y, registers[RegisterIndex]);
	}
}
