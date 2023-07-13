/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Common.Simulation;
using Mcs.Simulator.Memory;
namespace Mcs.Simulator.Validators;

/// Interface for classes that verify a matrix was transposed correctly.
public interface IMemoryValidator
{
	/// Initializes the data for the matrix.
	/// @param memory Memory that the matrix is stored in.
	/// @param matrix Matrix that will be used by the simulation.
	void Initialize(IMemory memory, IMatrix matrix);

	/// Checks whether the matrix was transposed correctly.
	/// @param memory Memory that the matrix is stored in.
	/// @param matrix Matrix that will be used by the simulation.
	/// @returns Whether the matrix was transposed correctly.
	bool Validate(IMemory memory, IMatrix matrix);
}
