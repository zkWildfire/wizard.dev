/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.Simulation;
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

	/// Applies the action to the given matrix and registers.
	/// @param matrix Matrix to apply the action to.
	/// @param registers Registers to apply the action to.
	public override void ApplyAction(IMatrix matrix, Register[] registers)
	{
		// Read the value from the matrix into the target register
		var (x, y) = matrix.ToMatrixCoordinate(Address);
		matrix.Read(x, y, registers[RegisterIndex]);
	}
}
