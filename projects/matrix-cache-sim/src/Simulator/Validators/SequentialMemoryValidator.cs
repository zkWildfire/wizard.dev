/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.Simulation;
using Mcs.Simulator.Memory;
namespace Mcs.Simulator.Validators;

/// Validator that writes sequential values to the matrix.
public class SequentialMemoryValidator : IMemoryValidator
{
	/// Value written to memory locations not part of the matrix.
	public const int UNINITIALIZED_VALUE = 0x12345678;

	/// Initializes the data for the matrix.
	/// @param memory Memory that the matrix is stored in.
	/// @param matrix Matrix that will be used by the simulation.
	public void Initialize(IMemory memory, IMatrix matrix)
	{
		// Initialize the memory elements
		for (var i = 0; i < memory.Size; i++)
		{
			memory.Write(i, UNINITIALIZED_VALUE);

			// If the address is part of the matrix, initialize it to the
			//   starting value for the cell
			if (i >= matrix.StartingAddress && i < matrix.EndingAddress)
			{
				var (xCoord, yCoord) = matrix.ToMatrixCoordinate(i);
				memory.Write(
					i,
					GetInitialValue(xCoord, yCoord, matrix)
				);
			}
		}
	}

	/// Checks whether the matrix was transposed correctly.
	/// @param memory Memory that the matrix is stored in.
	/// @param matrix Matrix used by the simulation.
	/// @returns Whether the matrix was transposed correctly.
	public bool Validate(IMemory memory, IMatrix matrix)
	{
		// Iterate over each element in the matrix
		for (var i = matrix.StartingAddress; i < matrix.EndingAddress; i++)
		{
			var (xCoord, yCoord) = matrix.ToMatrixCoordinate(i);

			// Check that the value is correct
			if (memory.Read(i) != GetExpectedValue(xCoord, yCoord, matrix))
			{
				return false;
			}
		}

		return true;
	}

	/// Gets the value that the memory cell should be initialized to.
	/// @param x X coordinate of the memory cell.
	/// @param y Y coordinate of the memory cell.
	/// @param matrix Matrix that the memory cell is part of.
	private static int GetInitialValue(int x, int y, IMatrix matrix)
	{
		var offset = matrix.IsColumnMajor
			? (x * matrix.Y) + y
			: (y * matrix.X) + x;

		// Start at 1 instead of at 0
		return offset + 1;
	}

	/// Gets the value that the memory cell is expected to be set to.
	/// @param x X coordinate of the memory cell.
	/// @param y Y coordinate of the memory cell.
	/// @param matrix Matrix that the memory cell is part of.
	private static int GetExpectedValue(int x, int y, IMatrix matrix)
	{
		return GetInitialValue(y, x, matrix);
	}
}
