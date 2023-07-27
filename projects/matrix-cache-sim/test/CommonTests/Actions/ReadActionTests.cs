/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Common.Actions;
using Mcs.Common.Simulation;

namespace McsTests.Common.Actions;

public class ReadActionTests
{
	[Theory]
	[InlineData(1, 2)]
	[InlineData(2, 1)]
	public void CtorSetsProperties(int address, int registerIndex)
	{
		var action = new ReadAction(address, registerIndex);

		Assert.True(action.IsRead);
		Assert.Equal(address, action.Address);
		Assert.Equal(registerIndex, action.RegisterIndex);
	}

	[Theory]
	[InlineData(-1, 0)]
	[InlineData(int.MinValue, 1)]
	public void CtorThrowsIfAddressIsNegative(
		int address,
		int registerIndex)
	{
		Assert.Throws<ArgumentOutOfRangeException>(
			() => new ReadAction(address, registerIndex)
		);
	}

	[Theory]
	[InlineData(0, -1)]
	[InlineData(1, int.MinValue)]
	public void CtorThrowsIfRegisterIndexIsNegative(
		int address,
		int registerIndex)
	{
		Assert.Throws<ArgumentOutOfRangeException>(
			() => new ReadAction(address, registerIndex)
		);
	}

	[Theory]
	[InlineData(2, 2, 1, 0)]
	[InlineData(2, 2, 0, 1)]
	[InlineData(4, 4, 2, 3)]
	public void ReadIntoRegister(
		int matrixX, int matrixY, int x, int y)
	{
		// Set up the matrix for the test
		var mockMatrix = new Mock<IMatrix>();
		mockMatrix.SetupGet(m => m.StartingAddress).Returns(0);
		mockMatrix.SetupGet(m => m.EndingAddress).Returns(matrixX * matrixY);
		mockMatrix.SetupGet(m => m.X).Returns(matrixX);
		mockMatrix.SetupGet(m => m.Y).Returns(matrixY);
		mockMatrix.SetupGet(m => m.IsColumnMajor).Returns(false);
		var matrix = mockMatrix.Object;

		// Set up the register that the test will use
		var mockRegister = new Mock<IRegister>();
		var register = mockRegister.Object;

		// Run the test
		var action = new ReadAction(matrix.ToMemoryAddress(x, y), 0);
		action.ApplyAction(matrix, new[] { register });

		// Make sure the expected methods were invoked
		mockMatrix.Verify(
			m => m.Read(x, y, register),
			Times.Once
		);
	}
}
