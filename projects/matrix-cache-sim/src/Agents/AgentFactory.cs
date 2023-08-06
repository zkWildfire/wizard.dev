/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator;
namespace Mcs.Agents;

/// Defines the interface for factories that create agent instances.
public interface IAgentFactory
{
	/// Name of the agent type that the factory constructs.
	string AgentName { get; }

	/// Unique ID for the agent.
	string AgentId { get; }

	/// Creates a new agent instance.
	/// @param simulator Simulator for the agent to use.
	/// @param registerCount Number of registers the agent can use.
	/// @returns A new agent instance.
	IAgent Construct(ISimulator simulator, int registerCount);
}
