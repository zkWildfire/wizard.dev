/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Cli.Results;

/// Results from all simulations that were run to evaluate an agent.
public readonly record struct AgentResults
{
	/// Name of the agent that was evaluated.
	public string AgentName { get; init; }
}
