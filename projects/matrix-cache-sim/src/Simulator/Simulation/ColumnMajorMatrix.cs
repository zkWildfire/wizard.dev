/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.Memory;
namespace Mcs.Simulator.Simulation;

/// Column major implementation of the memory matrix class.
public class ColumnMajorMatrix : IMemoryMatrix
{
	/// Initializes the matrix.
	/// @param memory Memory to use for reading and writing.
	/// @param x Size of the matrix in the X dimension.
	/// @param y Size of the matrix in the Y dimension.
	/// @param startingAddress Starting address of the matrix in memory.
	/// @param isColumnMajor Whether the matrix is stored in column major order.
	public ColumnMajorMatrix(
		IMemory memory,
		int x,
		int y,
		int startingAddress)
		: base(memory, x, y, startingAddress, true)
	{
	}
}
