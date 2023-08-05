/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.Memory;
using Mcs.Simulator.Simulation;
namespace McsTests.Simulator.Simulation;

public class ColumnMajorMatrixTests
{
	[Fact]
	public void CtorSetsProperties()
	{
		const int X = 1;
		const int Y = 2;
		const int STARTING_ADDRESS = 3;
		const bool IS_COLUMN_MAJOR = true;

		var matrix = new ColumnMajorMatrix(
			new ArrayMemory(STARTING_ADDRESS + (X * Y)),
			1,
			2,
			3
		);

		Assert.Equal(X, matrix.X);
		Assert.Equal(Y, matrix.Y);
		Assert.Equal(STARTING_ADDRESS, matrix.StartingAddress);
		Assert.Equal(STARTING_ADDRESS + (X * Y), matrix.EndingAddress);
		Assert.Equal(IS_COLUMN_MAJOR, matrix.IsColumnMajor);
	}

	[Theory]
	[InlineData(0)]
	[InlineData(-1)]
	public void CtorThrowsIfXIsInvalid(int x)
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
		{
			_ = new ColumnMajorMatrix(
				new ArrayMemory(1),
				x,
				1,
				1
			);
		});
	}

	[Theory]
	[InlineData(0)]
	[InlineData(-1)]
	public void CtorThrowsIfYIsInvalid(int y)
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
		{
			_ = new ColumnMajorMatrix(
				new ArrayMemory(1),
				1,
				y,
				1
			);
		});
	}

	[Theory]
	[InlineData(-1)]
	public void CtorThrowsIfStartingAddressIsInvalid(int startingAddress)
	{
		Assert.Throws<ArgumentOutOfRangeException>(() =>
		{
			_ = new ColumnMajorMatrix(
				new ArrayMemory(1),
				1,
				1,
				startingAddress
			);
		});
	}

	[Theory]
	[InlineData(1, 1, 0)]
	[InlineData(1, 2, 0)]
	[InlineData(2, 1, 0)]
	[InlineData(2, 2, 0)]
	[InlineData(1, 1, 1)]
	[InlineData(1, 2, 1)]
	[InlineData(2, 1, 1)]
	[InlineData(2, 2, 1)]
	public void ReadMatrixValues(int x, int y, int startingAddress)
	{
		var memory = new ArrayMemory(startingAddress + (x * y));
		// Initialize the memory with the values expected to be read
		for (var i = 0; i < memory.Size; i++)
		{
			memory.Write(i, i);
		}

		var matrix = new ColumnMajorMatrix(memory, x, y, startingAddress);
		for (var i = startingAddress; i < matrix.EndingAddress; i++)
		{
			var expectedValue = i;
			var (matrixX, matrixY) = matrix.ToMatrixCoordinate(i);
			var register = new Register();

			matrix.Read(matrixX, matrixY, register);
			Assert.Equal(expectedValue, register.Value);
			Assert.Equal(i, register.Address);
		}
	}

	[Theory]
	[InlineData(1, 1, 0)]
	[InlineData(1, 2, 0)]
	[InlineData(2, 1, 0)]
	[InlineData(2, 2, 0)]
	[InlineData(1, 1, 1)]
	[InlineData(1, 2, 1)]
	[InlineData(2, 1, 1)]
	[InlineData(2, 2, 1)]
	public void WriteMatrixValues(int x, int y, int startingAddress)
	{
		var memory = new ArrayMemory(startingAddress + (x * y));
		var matrix = new ColumnMajorMatrix(memory, x, y, startingAddress);
		for (var i = startingAddress; i < matrix.EndingAddress; i++)
		{
			var expectedValue = i;
			var (matrixX, matrixY) = matrix.ToMatrixCoordinate(i);
			var register = new Register(expectedValue, i);

			matrix.Write(matrixX, matrixY, register);
			Assert.Equal(expectedValue, memory.Read(i));
		}
	}
}
