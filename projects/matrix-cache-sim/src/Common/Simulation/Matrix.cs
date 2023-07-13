/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Common.Simulation;

/// Interface that each agent will use when transposing a matrix.
public interface IMatrix
{
	/// Size of the matrix in the X dimension.
	int X { get; }

	/// Size of the matrix in the Y dimension.
	int Y { get; }

	/// Starting address of the matrix in memory.
	int StartingAddress { get; }

	/// Past-the-end address of the matrix in memory.
	int EndingAddress { get; }

	/// Checks whether the matrix is stored in column major order.
	bool IsColumnMajor { get; }

	/// Checks whether the matrix has been fully transposed.
	bool IsTransposed { get; }

	/// Reads the value at the given coordinates into the given register.
	/// @param x X coordinate of the value to read.
	/// @param y Y coordinate of the value to read.
	/// @param reg Register to read the value into.
	void Read(int x, int y, IRegister reg);

	/// Writes the value from the given register into the given coordinates.
	/// @param x X coordinate of the value to write.
	/// @param y Y coordinate of the value to write.
	/// @param reg Register to write the value from.
	void Write(int x, int y, IRegister reg);
}
