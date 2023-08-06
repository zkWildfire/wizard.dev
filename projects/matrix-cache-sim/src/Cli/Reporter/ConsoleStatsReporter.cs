/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Cli.Results;
namespace Mcs.Cli.Reporter;

/// Stats reporter that prints to the console.
public class ConsoleStatsReporter : IStatsReporter
{
	/// Prefix used for each indented line.
	private const string PREFIX = "  ";

	/// Writer to write to.
	private readonly TextWriter _writer;

	/// Initializes a new instance of the class that writes to the console.
	public ConsoleStatsReporter()
		: this(Console.Out)
	{
	}

	/// Initializes a new instance of the class.
	/// @param writer Writer to write to.
	public ConsoleStatsReporter(TextWriter writer)
	{
		_writer = writer;
	}

	/// Reports results of evaluating multiple agents.
	/// @param results Results from the agent evaluations.
	public void ReportResults(IEnumerable<AgentResults> results)
	{
		foreach (var agent in results)
		{
			ReportResults(agent);
		}
	}

	/// Reports results of evaluating an agent.
	/// @param results Results from the agent evaluation.
	public void ReportResults(AgentResults results)
	{
		PrintBanner(results.AgentName);
		foreach (var run in results.SimulationRuns.Values)
		{
			ReportResults(run);
		}

		// Report aggregated statistics for the agent
		var stats = new ResultStatistics(results, 2);
		_writer.WriteLine("Agent Score Statistics:");
		_writer.WriteLine(
			$"{PREFIX}Arithmetic mean: {stats.ArithmeticMean}"
		);
		_writer.WriteLine(
			$"{PREFIX}Geometric mean: {stats.GeometricMean}"
		);
		_writer.WriteLine();
	}

	/// Reports results from a single simulation run.
	/// @param results Results from the simulation run.
	public void ReportResults(SimulationRun results)
	{
		_writer.WriteLine($"Configuration: {results.ConfigurationName}");
		ReportResults(results.Results);
	}

	/// Reports results from a single set of simulation runs.
	/// @param results Results from the simulation runs.
	public void ReportResults(SimulationResults results)
	{
		_writer.WriteLine(
			$"{PREFIX}Cache hits: {results.CacheHits}"
		);
		_writer.WriteLine(
			$"{PREFIX}Cache misses: {results.CacheMisses}"
		);
		_writer.WriteLine(
			$"{PREFIX}Total memory accesses: {results.TotalMemoryAccesses}"
		);
		_writer.WriteLine(
			$"{PREFIX}Score: {results.Score}"
		);
		_writer.WriteLine();
	}

	/// Prints a banner with the given text.
	/// @param bannerText Text to print in the banner.
	private void PrintBanner(
		string bannerText,
		char bannerChar = '=',
		int bannerWidth = 80)
	{
		_writer.WriteLine(new string(bannerChar, bannerWidth));
		_writer.WriteLine();
		_writer.WriteLine(bannerText);
		_writer.WriteLine();
		_writer.WriteLine(new string(bannerChar, bannerWidth));
		_writer.WriteLine();
	}
}
