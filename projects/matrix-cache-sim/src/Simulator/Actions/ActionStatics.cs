/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.Simulation;
namespace Mcs.Simulator.Actions;

/// Helper methods that apply to any `IAction`-derived type.
public static class ActionStatics
{
	/// Applies the action to the simulator and registers.
	/// @param action Action to apply.
	/// @param simulator Simulator to apply the action to.
	/// @param registers Registers to apply the action to.
	public static void ApplyAction(
		this IAction action,
		ISimulator simulator,
		IReadOnlyList<Register> registers)
	{
		var _ = action switch
		{
			ReadAction readAction => readAction.ApplyReadAction(
				simulator,
				registers
			),
			WriteAction writeAction => writeAction.ApplyWriteAction(
				simulator,
				registers
			),
			_ => false
		};
	}

	/// Applies the action to the given matrix and registers.
	/// @param action Action to apply.
	/// @param matrix Matrix to apply the action to.
	/// @param registers Registers to apply the action to.
	public static void ApplyAction(
		this IAction action,
		IMatrix matrix,
		IReadOnlyList<Register> registers)
	{
		var _ = action switch
		{
			ReadAction readAction => readAction.ApplyReadAction(
				matrix,
				registers
			),
			WriteAction writeAction => writeAction.ApplyWriteAction(
				matrix,
				registers
			),
			_ => false
		};
	}

	/// Applies the action to the simulator and registers.
	/// @param action Action to apply.
	/// @param simulator Simulator to apply the action to.
	/// @param registers Registers to apply the action to.
	/// @returns Dummy return type. Only used to ensure that this method can
	///   be used with a pattern matching switch statement, which requires each
	///   branch to evaluate to a value rather than void (though maybe this
	///   will change in a newer C# version?).
	private static bool ApplyReadAction(
		this ReadAction action,
		ISimulator simulator,
		IReadOnlyList<Register> registers)
	{
		// Read the value from the matrix into the target register
		registers[action.RegisterIndex].SetValue(
			simulator.Read(action.Address),
			action.Address
		);
		return true;
	}

	/// Applies the action to the given matrix and registers.
	/// @param action Action to apply.
	/// @param matrix Matrix to apply the action to.
	/// @param registers Registers to apply the action to.
	/// @returns Dummy return type. Only used to ensure that this method can
	///   be used with a pattern matching switch statement, which requires each
	///   branch to evaluate to a value rather than void (though maybe this
	///   will change in a newer C# version?).
	private static bool ApplyReadAction(
		this ReadAction action,
		IMatrix matrix,
		IReadOnlyList<Register> registers)
	{
		// Read the value from the matrix into the target register
		var (x, y) = matrix.ToMatrixCoordinate(action.Address);
		matrix.Read(x, y, registers[action.RegisterIndex]);
		return true;
	}

	/// Applies the action to the simulator and registers.
	/// @param action Action to apply.
	/// @param simulator Simulator to apply the action to.
	/// @param registers Registers to apply the action to.
	/// @returns Dummy return type. Only used to ensure that this method can
	///   be used with a pattern matching switch statement, which requires each
	///   branch to evaluate to a value rather than void (though maybe this
	///   will change in a newer C# version?).
	private static bool ApplyWriteAction(
		this WriteAction action,
		ISimulator simulator,
		IReadOnlyList<Register> registers)
	{
		// Write the value from the source register into the matrix
		simulator.Write(action.Address, registers[action.RegisterIndex].Value);
		return true;
	}

	/// Applies the action to the given matrix and registers.
	/// @param action Action to apply.
	/// @param matrix Matrix to apply the action to.
	/// @param registers Registers to apply the action to.
	/// @returns Dummy return type. Only used to ensure that this method can
	///   be used with a pattern matching switch statement, which requires each
	///   branch to evaluate to a value rather than void (though maybe this
	///   will change in a newer C# version?).
	private static bool ApplyWriteAction(
		this WriteAction action,
		IMatrix matrix,
		IReadOnlyList<Register> registers)
	{
		// Write the value from the source register into the matrix
		var (x, y) = matrix.ToMatrixCoordinate(action.Address);
		matrix.Write(x, y, registers[action.RegisterIndex]);
		return true;
	}
}
