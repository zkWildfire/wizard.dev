/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Common.Actions;

namespace McsTests.Common.Actions;

public class ActionTests
{
	[Theory]
	[InlineData(true, 1, 2)]
	[InlineData(false, 2, 1)]
	public void CtorSetsProperties(bool isRead, int address, int registerIndex)
	{
		var mock = new Mock<IAction>(isRead, address, registerIndex);
		var action = mock.Object;

		Assert.Equal(isRead, action.IsRead);
		Assert.Equal(address, action.Address);
		Assert.Equal(registerIndex, action.RegisterIndex);
	}
}
