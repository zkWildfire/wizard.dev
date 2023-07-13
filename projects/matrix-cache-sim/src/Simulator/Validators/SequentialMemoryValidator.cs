/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Common.Simulation;
using Mcs.Simulator.Memory;
namespace Mcs.Simulator.Validators;

/// Validator that writes sequential values to the matrix.
public class SequentialMemoryValidator : IMemoryValidator
{
	/// Value written to memory locations not part of the matrix.
	private const int UNINITIALIZED_VALUE = 0x12345678;

	/// Initializes the data for the matrix.
	/// @param memory Memory that the matrix is stored in.
	/// @param startingAddress Starting address of the matrix in memory.
	/// @param x Size of the matrix in the X dimension.
	/// @param y Size of the matrix in the Y dimension.
	public void Initialize(IMemory memory, int startingAddress, int x, int y)
	{
		// Figure out the range of addresses that the matrix occupies
		var elementCount = x * y;
		var endingAddress = startingAddress + elementCount;

		// Initialize the memory elements
		for (var i = 0; i < memory.Size; i++)
		{
			memory.Write(i, UNINITIALIZED_VALUE);

			// If the address is part of the matrix, initialize it to the
			//   starting value for the cell
			if (i >= startingAddress && i < endingAddress)
			{
				var (xCoord, yCoord) = MatrixStatics.ToMatrixCoordinate(
					i,
					startingAddress,
					x,
					y
				);
				memory.Write(i, GetInitialValue(xCoord, yCoord, x, y));
			}
		}
	}

	/// Checks whether the matrix was transposed correctly.
	/// @param memory Memory that the matrix is stored in.
	/// @param startingAddress Starting address of the matrix in memory.
	/// @param x Size of the matrix in the X dimension.
	/// @param y Size of the matrix in the Y dimension.
	public bool Validate(IMemory memory, int startingAddress, int x, int y)
	{
		// Figure out the range of addresses that the matrix occupies
		var elementCount = x * y;
		var endingAddress = startingAddress + elementCount;

		// Iterate over each element in the matrix
		for (var i = startingAddress; i < endingAddress; i++)
		{
			var (xCoord, yCoord) = MatrixStatics.ToMatrixCoordinate(
				i,
				startingAddress,
				x,
				y
			);

			// Check that the value is correct
			if (memory.Read(i) != GetExpectedValue(xCoord, yCoord, x, y))
			{
				return false;
			}
		}

		return true;
	}

	/// Gets the value that the memory cell should be initialized to.
	/// @param x X coordinate of the memory cell.
	/// @param y Y coordinate of the memory cell.
	/// @param sizeX Size of the matrix in the X dimension.
	/// @param sizeY Size of the matrix in the Y dimension.
	private static int GetInitialValue(int x, int y, int sizeX, int sizeY)
	{
		return MatrixStatics.ToMemoryAddress(x, y, sizeX, sizeY) + 1;
	}

	/// Gets the value that the memory cell is expected to be set to.
	/// @param x X coordinate of the memory cell.
	/// @param y Y coordinate of the memory cell.
	/// @param sizeX Size of the matrix in the X dimension.
	/// @param sizeY Size of the matrix in the Y dimension.
	private static int GetExpectedValue(int x, int y, int sizeX, int sizeY)
	{
		return MatrixStatics.ToMemoryAddress(y, x, sizeX, sizeY) + 1;
	}
}
