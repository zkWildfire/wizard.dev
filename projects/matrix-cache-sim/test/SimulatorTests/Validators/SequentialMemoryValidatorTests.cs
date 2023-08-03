/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Common.Simulation;
using Mcs.Simulator.Memory;
using Mcs.Simulator.Validators;
using Mcs.Simulator.Simulation;
namespace McsTests.Simulator.Validators;

public class SequentialMemoryValidatorTests
{
	[Fact]
	public void NonMatrixMemoryInitializedToDummyValue()
	{
		const int PADDING = 2;
		const int MATRIX_X = 2;
		const int MATRIX_Y = 2;
		const int STARTING_ADDRESS = PADDING;

		// Set up the matrix to use for the test
		var mockMatrix = new Mock<IMatrix>();
		mockMatrix.SetupGet(m => m.StartingAddress).Returns(STARTING_ADDRESS);
		mockMatrix.SetupGet(m => m.EndingAddress)
			.Returns(STARTING_ADDRESS + (MATRIX_X * MATRIX_Y));
		mockMatrix.SetupGet(m => m.X).Returns(MATRIX_X);
		mockMatrix.SetupGet(m => m.Y).Returns(MATRIX_Y);
		mockMatrix.SetupGet(m => m.IsColumnMajor).Returns(false);

		// Set up the memory object for the test
		var memory = new ArrayMemory(mockMatrix.Object.EndingAddress + PADDING);

		// Run the test
		var validator = new SequentialMemoryValidator();
		validator.Initialize(memory, mockMatrix.Object);

		// All padding elements (both before and after the matrix) should have
		//   the dummy value
		for (var i = 0; i < PADDING; i++)
		{
			Assert.Equal(
				SequentialMemoryValidator.UNINITIALIZED_VALUE,
				memory.Read(i)
			);
			Assert.Equal(
				SequentialMemoryValidator.UNINITIALIZED_VALUE,
				memory.Read(memory.Size - i - 1)
			);
		}
	}

	[Fact]
	public void MatrixMemoryInitializedToSequentialValues()
	{
		const int PADDING = 2;
		const int MATRIX_X = 2;
		const int MATRIX_Y = 2;
		const int STARTING_ADDRESS = PADDING;

		// Set up the matrix to use for the test
		var mockMatrix = new Mock<IMatrix>();
		mockMatrix.SetupGet(m => m.StartingAddress).Returns(STARTING_ADDRESS);
		mockMatrix.SetupGet(m => m.EndingAddress)
			.Returns(STARTING_ADDRESS + (MATRIX_X * MATRIX_Y));
		mockMatrix.SetupGet(m => m.X).Returns(MATRIX_X);
		mockMatrix.SetupGet(m => m.Y).Returns(MATRIX_Y);
		mockMatrix.SetupGet(m => m.IsColumnMajor).Returns(false);

		// Set up the memory object for the test
		var memory = new ArrayMemory(mockMatrix.Object.EndingAddress + PADDING);

		// Run the test
		var validator = new SequentialMemoryValidator();
		validator.Initialize(memory, mockMatrix.Object);

		// Since the matrix is row major, the values should be sequential
		//   starting at the starting address
		for (var i = STARTING_ADDRESS; i < mockMatrix.Object.EndingAddress; i++)
		{
			Assert.Equal(
				// The validator uses 1 as its starting value rather than 0,
				//   so it's necessary to add 1 to the index
				i - mockMatrix.Object.StartingAddress + 1,
				memory.Read(i)
			);
		}
	}

	[Theory]
	[InlineData(1, 1, 0)]
	[InlineData(1, 1, 1)]
	[InlineData(2, 2, 0)]
	[InlineData(2, 2, 1)]
	[InlineData(8, 8, 0)]
	[InlineData(8, 8, 2)]
	public void ValidateCorrectlyTransposedRowMajorMatrix(
		int matrixX, int matrixY, int startingAddress)
	{
		// Initialize the memory object
		var memory = new ArrayMemory(startingAddress + (matrixX * matrixY));
		var matrix = new RowMajorMatrix(
			memory,
			matrixX,
			matrixY,
			startingAddress
		);
		var validator = new SequentialMemoryValidator();
		validator.Initialize(memory, matrix);

		// Transpose the matrix
		var reg1 = new Register();
		var reg2 = new Register();
		for (var x = 0; x < matrixX; x++)
		{
			for (var y = 0; y < matrixY; y++)
			{
				if (x > y)
				{
					continue;
				}

				matrix.Read(x, y, reg1);
				matrix.Read(y, x, reg2);
				matrix.Write(x, y, reg2);
				matrix.Write(y, x, reg1);
			}
		}

		// Validate the matrix
		Assert.True(validator.Validate(memory, matrix));
	}

	[Theory]
	[InlineData(1, 1, 0)]
	[InlineData(1, 1, 1)]
	[InlineData(2, 2, 0)]
	[InlineData(2, 2, 1)]
	[InlineData(8, 8, 0)]
	[InlineData(8, 8, 2)]
	public void ValidateCorrectlyTransposedColumnMajorMatrix(
		int matrixX, int matrixY, int startingAddress)
	{
		// Initialize the memory object
		var memory = new ArrayMemory(startingAddress + (matrixX * matrixY));
		var matrix = new ColumnMajorMatrix(
			memory,
			matrixX,
			matrixY,
			startingAddress
		);
		var validator = new SequentialMemoryValidator();
		validator.Initialize(memory, matrix);

		// Transpose the matrix
		var reg1 = new Register();
		var reg2 = new Register();
		for (var x = 0; x < matrixX; x++)
		{
			for (var y = 0; y < matrixY; y++)
			{
				if (x > y)
				{
					continue;
				}

				matrix.Read(x, y, reg1);
				matrix.Read(y, x, reg2);
				matrix.Write(x, y, reg2);
				matrix.Write(y, x, reg1);
			}
		}

		// Validate the matrix
		Assert.True(validator.Validate(memory, matrix));
	}

	[Fact]
	public void ValidateIncorrectlyTransposedRowMajorMatrix()
	{
		const int MATRIX_X = 2;
		const int MATRIX_Y = 2;
		const int STARTING_ADDRESS = 2;

		// Initialize the memory object
		var memory = new ArrayMemory(STARTING_ADDRESS + (MATRIX_X * MATRIX_Y));
		var matrix = new RowMajorMatrix(
			memory,
			MATRIX_X,
			MATRIX_Y,
			STARTING_ADDRESS
		);
		var validator = new SequentialMemoryValidator();
		validator.Initialize(memory, matrix);

		// Validate the matrix
		// Since no transpose operation occurred, validation should fail
		Assert.False(validator.Validate(memory, matrix));
	}

	[Fact]
	public void ValidateIncorrectlyTransposedColumnMajorMatrix()
	{
		const int MATRIX_X = 2;
		const int MATRIX_Y = 2;
		const int STARTING_ADDRESS = 2;

		// Initialize the memory object
		var memory = new ArrayMemory(STARTING_ADDRESS + (MATRIX_X * MATRIX_Y));
		var matrix = new ColumnMajorMatrix(
			memory,
			MATRIX_X,
			MATRIX_Y,
			STARTING_ADDRESS
		);
		var validator = new SequentialMemoryValidator();
		validator.Initialize(memory, matrix);

		// Validate the matrix
		// Since no transpose operation occurred, validation should fail
		Assert.False(validator.Validate(memory, matrix));
	}
}
