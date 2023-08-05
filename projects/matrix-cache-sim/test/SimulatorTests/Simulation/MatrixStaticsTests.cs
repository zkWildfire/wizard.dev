/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.Actions;
using Mcs.Simulator.Simulation;
namespace McsTests.Simulator.Simulation;

public class MatrixStaticsTests
{
	[Theory]
	[InlineData(0, 2, 2, 1, 0)]
	[InlineData(0, 2, 2, 0, 1)]
	[InlineData(0, 2, 2, 1, 1)]
	[InlineData(4, 8, 8, 0, 0)]
	[InlineData(4, 8, 8, 5, 2)]
	[InlineData(4, 8, 8, 1, 6)]
	[InlineData(4, 8, 8, 7, 7)]
	[InlineData(5, 3, 5, 1, 3)]
	[InlineData(9, 4, 8, 3, 2)]
	[InlineData(13, 5, 3, 3, 0)]
	[InlineData(17, 8, 4, 7, 2)]
	public void ConvertRowMajorMatrixCoordinateToMemoryAddress(
		int startingAddress, int matrixX, int matrixY, int x, int y)
	{
		// Set up the matrix for the test
		var mockMatrix = new Mock<IMatrix>();
		mockMatrix.SetupGet(m => m.StartingAddress).Returns(startingAddress);
		mockMatrix.SetupGet(m => m.EndingAddress)
			.Returns(startingAddress + (matrixX * matrixY));
		mockMatrix.SetupGet(m => m.X).Returns(matrixX);
		mockMatrix.SetupGet(m => m.Y).Returns(matrixY);
		mockMatrix.SetupGet(m => m.IsColumnMajor).Returns(false);
		var matrix = mockMatrix.Object;

		// Run the test
		var expected = startingAddress + (y * matrixX) + x;
		var actual = matrix.ToMemoryAddress(x, y);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(0, 2, 2, 1, 0)]
	[InlineData(0, 2, 2, 0, 1)]
	[InlineData(0, 2, 2, 1, 1)]
	[InlineData(4, 8, 8, 0, 0)]
	[InlineData(4, 8, 8, 5, 2)]
	[InlineData(4, 8, 8, 1, 6)]
	[InlineData(4, 8, 8, 7, 7)]
	[InlineData(5, 3, 5, 1, 3)]
	[InlineData(9, 4, 8, 3, 2)]
	[InlineData(13, 5, 3, 3, 0)]
	[InlineData(17, 8, 4, 7, 2)]
	public void ConvertColumnMajorMatrixCoordinateToMemoryAddress(
		int startingAddress, int matrixX, int matrixY, int x, int y)
	{
		// Set up the matrix for the test
		var mockMatrix = new Mock<IMatrix>();
		mockMatrix.SetupGet(m => m.StartingAddress).Returns(startingAddress);
		mockMatrix.SetupGet(m => m.EndingAddress)
			.Returns(startingAddress + (matrixX * matrixY));
		mockMatrix.SetupGet(m => m.X).Returns(matrixX);
		mockMatrix.SetupGet(m => m.Y).Returns(matrixY);
		mockMatrix.SetupGet(m => m.IsColumnMajor).Returns(true);
		var matrix = mockMatrix.Object;

		// Run the test
		var expected = startingAddress + (x * matrixY) + y;
		var actual = matrix.ToMemoryAddress(x, y);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(0, 2, 2, 1, 0)]
	[InlineData(0, 2, 2, 0, 1)]
	[InlineData(0, 2, 2, 1, 1)]
	[InlineData(4, 8, 8, 0, 0)]
	[InlineData(4, 8, 8, 5, 2)]
	[InlineData(4, 8, 8, 1, 6)]
	[InlineData(4, 8, 8, 7, 7)]
	[InlineData(5, 3, 5, 1, 3)]
	[InlineData(9, 4, 8, 3, 2)]
	[InlineData(13, 5, 3, 3, 0)]
	[InlineData(17, 8, 4, 7, 2)]
	public void ConvertRowMemoryAddressToMatrixCoordinate(
		int startingAddress, int matrixX, int matrixY, int x, int y)
	{
		// Set up the matrix for the test
		var mockMatrix = new Mock<IMatrix>();
		mockMatrix.SetupGet(m => m.StartingAddress).Returns(startingAddress);
		mockMatrix.SetupGet(m => m.EndingAddress)
			.Returns(startingAddress + (matrixX * matrixY));
		mockMatrix.SetupGet(m => m.X).Returns(matrixX);
		mockMatrix.SetupGet(m => m.Y).Returns(matrixY);
		mockMatrix.SetupGet(m => m.IsColumnMajor).Returns(false);
		var matrix = mockMatrix.Object;

		// Run the test
		var memoryAddress = matrix.ToMemoryAddress(x, y);
		var expected = (x, y);
		var actual = matrix.ToMatrixCoordinate(memoryAddress);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(0, 2, 2, 1, 0)]
	[InlineData(0, 2, 2, 0, 1)]
	[InlineData(0, 2, 2, 1, 1)]
	[InlineData(4, 8, 8, 0, 0)]
	[InlineData(4, 8, 8, 5, 2)]
	[InlineData(4, 8, 8, 1, 6)]
	[InlineData(4, 8, 8, 7, 7)]
	[InlineData(5, 3, 5, 1, 3)]
	[InlineData(9, 4, 8, 3, 2)]
	[InlineData(13, 5, 3, 3, 0)]
	[InlineData(17, 8, 4, 7, 2)]
	public void ConvertColumnMemoryAddressToMatrixCoordinate(
		int startingAddress, int matrixX, int matrixY, int x, int y)
	{
		// Set up the matrix for the test
		var mockMatrix = new Mock<IMatrix>();
		mockMatrix.SetupGet(m => m.StartingAddress).Returns(startingAddress);
		mockMatrix.SetupGet(m => m.EndingAddress)
			.Returns(startingAddress + (matrixX * matrixY));
		mockMatrix.SetupGet(m => m.X).Returns(matrixX);
		mockMatrix.SetupGet(m => m.Y).Returns(matrixY);
		mockMatrix.SetupGet(m => m.IsColumnMajor).Returns(true);
		var matrix = mockMatrix.Object;

		// Run the test
		var memoryAddress = matrix.ToMemoryAddress(x, y);
		var expected = (x, y);
		var actual = matrix.ToMatrixCoordinate(memoryAddress);
		Assert.Equal(expected, actual);
	}

	[Theory]
	[InlineData(0, 2, 2, 0, 0, 0, 1, 1, 1)]
	[InlineData(0, 2, 2, 1, 0, 1, 0, 1, 0)]
	public void SwapGeneratesTwoReadsTwoWrites(
		int startingAddress,
		int matrixX,
		int matrixY,
		int x1,
		int y1,
		int r1,
		int x2,
		int y2,
		int r2)
	{
		// Set up the matrix for the test
		var mockMatrix = new Mock<IMatrix>();
		mockMatrix.SetupGet(m => m.StartingAddress).Returns(startingAddress);
		mockMatrix.SetupGet(m => m.EndingAddress)
			.Returns(startingAddress + (matrixX * matrixY));
		mockMatrix.SetupGet(m => m.X).Returns(matrixX);
		mockMatrix.SetupGet(m => m.Y).Returns(matrixY);
		mockMatrix.SetupGet(m => m.IsColumnMajor).Returns(false);
		var matrix = mockMatrix.Object;

		// Run the test
		// Note that the actions that are generated by the swap method must be
		//   in this specific order
		var expectedTypes = new List<Type>
		{
			typeof(ReadAction),
			typeof(ReadAction),
			typeof(WriteAction),
			typeof(WriteAction)
		};
		var nextIndex = 0;
		foreach (var action in matrix.Swap(x1, y1, r1, x2, y2, r2))
		{
			Assert.Equal(expectedTypes[nextIndex], action.GetType());
			nextIndex++;
		}
	}
}
