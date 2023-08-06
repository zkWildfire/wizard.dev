/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Cli.Results;

/// Helper class used to calculate various statistics from an agent's results.
public class ResultStatistics
{
	/// Arithmetic mean of the agent's scores.
	public double ArithmeticMean => Math.Round(
		_arithmeticMean,
		_digits
	);

	/// Geometric mean of the agent's scores.
	public double GeometricMean => Math.Round(
		_geometricMean,
		_digits
	);

	/// Field backing the `ArithmeticMean` property.
	private readonly double _arithmeticMean;

	/// Field backing the `GeometricMean` property.
	private readonly double _geometricMean;

	/// Number of digits to round to.
	private readonly int _digits;

	/// Initializes the class.
	/// @param results Results to calculate statistics from.
	/// @param digits Number of digits to round to.
	public ResultStatistics(AgentResults results, int digits)
	{
		_arithmeticMean = CalculateArithmeticMean(
			results.SimulationRuns.Values.Select(
				run => (double)run.Results.Score
			)
		);
		_geometricMean = CalculateGeometricMean(
			results.SimulationRuns.Values.Select(
				run => (double)run.Results.Score
			)
		);
		_digits = digits;
	}

	/// Calculates the arithmetic mean of the agent's scores.
	/// @param results Results to calculate the mean from.
	private static double CalculateArithmeticMean(IEnumerable<double> values)
	{
		return values.Average();
	}

	private static double CalculateGeometricMean(IEnumerable<double> values)
	{
		// Disable CA1851: Possible multiple enumerations of 'IEnumerable'
		//   collection. Consider using an implementation that avoids multiple
		//   enumerations.
		// This code is only used for reporting, so performance is not a
		//   concern.
#pragma warning disable CA1851
		return Math.Pow(
			values.Aggregate((a, b) => a * b),
			1.0 / values.Count()
		);
#pragma warning restore CA1851
	}
}
