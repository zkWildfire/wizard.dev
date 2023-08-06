/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator;
namespace Mcs.Agents.Naive;

/// Defines the interface for factories that create agent instances.
public class NaiveAgentFactory : IAgentFactory
{
	/// Name of the agent type that the factory constructs.
	public string AgentName => "Naive Agent";

	/// Creates a new agent instance.
	/// @param simulator Simulator for the agent to use.
	/// @param registerCount Number of registers the agent can use.
	/// @returns A new agent instance.
	public IAgent Construct(ISimulator simulator, int registerCount)
	{
		return new NaiveAgent(simulator.Matrix, registerCount);
	}
}
