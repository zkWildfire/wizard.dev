/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Common.Actions;

/// Represents a read operation.
public class ReadAction : IAction
{
	/// Whether the action is a read or a write.
	public bool IsRead => true;

	/// Address of the matrix that the action is being performed on.
	public int Address { get; }

	/// Index of the register that the action is being performed on.
	public int RegisterIndex { get; }

	/// Creates a new read action.
	/// @param address Address of the memory location to read from.
	/// @param registerIndex Index of the register to read the value into.
	public ReadAction(int address, int registerIndex)
	{
		Address = address;
		RegisterIndex = registerIndex;
	}
}
