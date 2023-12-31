/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.Memory;
namespace Mcs.Simulator.Simulation;

/// Base type for matrices backed by an `IMemory` instance.
public abstract class IMemoryMatrix : IMatrix
{
	/// Size of the matrix in the X dimension.
	public int X { get; }

	/// Size of the matrix in the Y dimension.
	public int Y { get; }

	/// Starting address of the matrix in memory.
	public int StartingAddress { get; }

	/// Past-the-end address of the matrix in memory.
	public int EndingAddress { get; }

	/// Checks whether the matrix is stored in column major order.
	public bool IsColumnMajor { get; }

	/// Checks whether the matrix has been fully transposed.
	public bool IsTransposed { get; }

	/// Memory to use for reading and writing.
	private readonly IMemory _memory;

	/// Initializes the matrix.
	/// @param memory Memory to use for reading and writing.
	/// @param x Size of the matrix in the X dimension.
	/// @param y Size of the matrix in the Y dimension.
	/// @param startingAddress Starting address of the matrix in memory.
	/// @param isColumnMajor Whether the matrix is stored in column major order.
	/// @throws ArgumentOutOfRangeException If `x` or `y` is less than or equal
	///   to zero.
	/// @throws ArgumentOutOfRangeException If `startingAddress` is less than
	///   zero.
	protected IMemoryMatrix(
		IMemory memory,
		int x,
		int y,
		int startingAddress,
		bool isColumnMajor)
	{
		if (x <= 0)
		{
			throw new ArgumentOutOfRangeException(nameof(x));
		}
		if (y <= 0)
		{
			throw new ArgumentOutOfRangeException(nameof(y));
		}
		if (startingAddress < 0)
		{
			throw new ArgumentOutOfRangeException(nameof(startingAddress));
		}

		_memory = memory;
		X = x;
		Y = y;
		StartingAddress = startingAddress;
		EndingAddress = startingAddress + (x * y);
		IsColumnMajor = isColumnMajor;
		IsTransposed = false;
	}

	/// Reads the value at the given coordinates into the given register.
	/// @param x X coordinate of the value to read.
	/// @param y Y coordinate of the value to read.
	/// @param reg Register to read the value into.
	public void Read(int x, int y, Register reg)
	{
		// `ToMemoryAddress()` is an extension method, hence the use of `this`
		var address = this.ToMemoryAddress(x, y);
		var value = _memory.Read(address);
		reg.SetValue(value, address);
	}

	/// Writes the value from the given register into the given coordinates.
	/// @param x X coordinate of the value to write.
	/// @param y Y coordinate of the value to write.
	/// @param reg Register to write the value from.
	public void Write(int x, int y, Register reg)
	{
		// `ToMemoryAddress()` is an extension method, hence the use of `this`
		var address = this.ToMemoryAddress(x, y);
		_memory.Write(address, reg.Value);
	}
}
