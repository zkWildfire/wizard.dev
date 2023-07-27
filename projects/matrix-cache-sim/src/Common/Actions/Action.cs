/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Common.Actions;

/// Represents an action that an agent can take.
public abstract class IAction
{
	/// Whether the action is a read or a write.
	public bool IsRead { get; }

	/// Address of the matrix that the action is being performed on.
	public int Address { get; }

	/// Index of the register that the action is being performed on.
	public int RegisterIndex { get; }

	/// Creates a new action.
	/// @param isRead Whether the action is a read or a write.
	/// @param address Address of the memory location to write to.
	/// @param registerIndex Index of the register with the value to write.
	/// @throws ArgumentOutOfRangeException If the address or register index is
	///   negative.
	protected IAction(bool isRead, int address, int registerIndex)
	{
		if (address < 0)
		{
			throw new ArgumentOutOfRangeException(
				nameof(address),
				address,
				$"Expected address '{address}' to be non-negative."
			);
		}
		if (registerIndex < 0)
		{
			throw new ArgumentOutOfRangeException(
				nameof(registerIndex),
				registerIndex,
				$"Expected register index '{registerIndex}' to be non-negative."
			);
		}

		IsRead = isRead;
		Address = address;
		RegisterIndex = registerIndex;
	}
}
