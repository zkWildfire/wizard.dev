/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Common.Actions;
namespace Mcs.Common.Simulation;

/// Defines helper methods applicable to any matrix implementation.
public static class MatrixStatics
{
	/// Converts the given matrix coordinate into a memory address.
	/// @param x X coordinate of the value to convert.
	/// @param y Y coordinate of the value to convert.
	/// @param xSize Size of the matrix in the X dimension.
	/// @param ySize Size of the matrix in the Y dimension.
	/// @param startingOffset Offset to add to the first memory address.
	/// @param columnMajor Whether the matrix is stored in column major order.
	/// @returns The memory address of the given coordinates.
	public static int ToMemoryAddress(
		int x,
		int y,
		int xSize,
		int ySize,
		int startingOffset = 0,
		bool columnMajor = false)
	{
		return columnMajor
			? (x * ySize) + y + startingOffset
			: (y * xSize) + x + startingOffset;
	}

	/// Converts the given matrix coordinate into a memory address.
	/// @param matrix Matrix to convert the coordinates for.
	/// @param x X coordinate of the value to convert.
	/// @param y Y coordinate of the value to convert.
	/// @returns Memory address of the given coordinates.
	public static int ToMemoryAddress(this IMatrix matrix, int x, int y)
	{
		return ToMemoryAddress(
			x,
			y,
			matrix.X,
			matrix.Y,
			matrix.StartingAddress,
			matrix.IsColumnMajor
		);
	}

	/// Converts the given memory address into a matrix coordinate.
	/// @param memoryAddress Memory address to convert.
	/// @param matrix Matrix to convert the address for.
	/// @returns The matrix coordinate of the given memory address.
	public static (int, int) ToMatrixCoordinate(
		int memoryAddress,
		int startingOffset,
		int xSize,
		int ySize,
		bool columnMajor = false)
	{
		var offset = memoryAddress - startingOffset;
		return columnMajor
			? (offset / ySize, offset % ySize)
			: (offset % xSize, offset / xSize);
	}

	/// Converts the given memory address into a matrix coordinate.
	/// @param matrix Matrix to convert the address for.
	/// @param memoryAddress Memory address to convert.
	/// @throws ArgumentOutOfRangeException If the memory address is not within
	///   the matrix.
	/// @returns The matrix coordinate of the given memory address.
	public static (int, int) ToMatrixCoordinate(
		this IMatrix matrix,
		int memoryAddress)
	{
		// Make sure that the memory address is within the matrix
		var beforeStart = memoryAddress < matrix.StartingAddress;
		var afterEnd = memoryAddress >= matrix.EndingAddress;

		return beforeStart || afterEnd
			? throw new ArgumentOutOfRangeException(
				nameof(memoryAddress),
				memoryAddress,
				"Memory address is not within the matrix."
			)
			: ToMatrixCoordinate(
				memoryAddress,
				matrix.StartingAddress,
				matrix.X,
				matrix.Y,
				matrix.IsColumnMajor
			);
	}

	/// Generates a sequence of actions that will swap the two matrix cells.
	/// @param matrix Matrix to swap the cells in.
	/// @param x1 X coordinate of the first cell to swap.
	/// @param y1 Y coordinate of the first cell to swap.
	/// @param r1 Index of the register to use for the first cell.
	/// @param x2 X coordinate of the second cell to swap.
	/// @param y2 Y coordinate of the second cell to swap.
	/// @param r2 Index of the register to use for the second cell.
	/// @returns The sequence of actions that will swap the two matrix cells.
	public static IEnumerable<IAction> Swap(
		this IMatrix matrix,
		int x1,
		int y1,
		int r1,
		int x2,
		int y2,
		int r2)
	{
		yield return new ReadAction(matrix.ToMemoryAddress(x1, y1), r1);
		yield return new ReadAction(matrix.ToMemoryAddress(x2, y2), r2);
		yield return new WriteAction(matrix.ToMemoryAddress(x2, y2), r1);
		yield return new WriteAction(matrix.ToMemoryAddress(x1, y1), r2);
	}
}
