/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Agents.Naive;
using Mcs.Simulator;
namespace McsTests.Agents.Naive;

public class NaiveAgentFactoryTests
{
	[Fact]
	public void FactoryAgentNameMatchesAgentName()
	{
		var factory = new NaiveAgentFactory();
		var agent = factory.Construct(Mock.Of<ISimulator>(), 2);
		Assert.Equal(agent.Name, factory.AgentName);
	}

	[Fact]
	public void FactoryAgentIdIsNonEmpty()
	{
		var factory = new NaiveAgentFactory();
		Assert.NotEmpty(factory.AgentId);
	}
}
