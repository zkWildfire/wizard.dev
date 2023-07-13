/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.Memory;
namespace Mcs.Simulator.Validators;

/// Interface for classes that verify a matrix was transposed correctly.
public interface IMemoryValidator
{
	/// Initializes the data for the matrix.
	/// @param memory Memory that the matrix is stored in.
	/// @param startingAddress Starting address of the matrix in memory.
	/// @param x Size of the matrix in the X dimension.
	/// @param y Size of the matrix in the Y dimension.
	void Initialize(IMemory memory, int startingAddress, int x, int y);

	/// Checks whether the matrix was transposed correctly.
	/// @param memory Memory that the matrix is stored in.
	/// @param startingAddress Starting address of the matrix in memory.
	/// @param x Size of the matrix in the X dimension.
	/// @param y Size of the matrix in the Y dimension.
	bool Validate(IMemory memory, int startingAddress, int x, int y);
}
