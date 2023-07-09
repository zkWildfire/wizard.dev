/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Common.Actions;

/// Represents a write operation.
public class WriteAction : IAction
{
	/// Whether the action is a read or a write.
	public bool IsRead => false;

	/// Address of the matrix that the action is being performed on.
	public int Address { get; }

	/// Index of the register that the action is being performed on.
	public int RegisterIndex { get; }

	/// Creates a new write action.
	/// @param address Address of the memory location to write to.
	/// @param registerIndex Index of the register with the value to write.
	public WriteAction(int address, int registerIndex)
	{
		Address = address;
		RegisterIndex = registerIndex;
	}
}
