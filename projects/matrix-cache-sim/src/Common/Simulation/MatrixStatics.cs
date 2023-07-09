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
	/// @param matrix Matrix to convert the coordinates for.
	/// @param x X coordinate of the value to convert.
	/// @param y Y coordinate of the value to convert.
	/// @return Memory address of the given coordinates.
	public static int ToMemoryAddress(this IMatrix matrix, int x, int y)
	{
		return matrix.StartingAddress + (y * matrix.X) + x;
	}

	/// Generates a sequence of actions that will swap the two matrix cells.
	/// @param matrix Matrix to swap the cells in.
	/// @param x1 X coordinate of the first cell to swap.
	/// @param y1 Y coordinate of the first cell to swap.
	/// @param r1 Index of the register to use for the first cell.
	/// @param x2 X coordinate of the second cell to swap.
	/// @param y2 Y coordinate of the second cell to swap.
	/// @param r2 Index of the register to use for the second cell.
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
		yield return new WriteAction(matrix.ToMemoryAddress(x2, y2), r2);
		yield return new WriteAction(matrix.ToMemoryAddress(x1, y1), r1);
	}
}
