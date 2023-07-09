/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Common.Actions;

/// Represents an action that an agent can take.
public interface IAction
{
	/// Whether the action is a read or a write.
	bool IsRead { get; }

	/// Address of the matrix that the action is being performed on.
	int Address { get; }

	/// Index of the register that the action is being performed on.
	int RegisterIndex { get; }
}
