/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Lightspeed.Classification.Training;

/// <summary>
/// Struct that contains all per-class metrics for a model.
/// </summary>
public readonly record struct ClassMetrics
{
	/// <summary>
	/// Rate of true positives for the class.
	/// This will be a value in the range `[0, 1]`.
	/// </summary>
	public required double TruePositiveRate { get; init; }

	/// <summary>
	/// Rate of false positives for the class.
	/// This will be a value in the range `[0, 1]`.
	/// </summary>
	public required double FalsePositiveRate { get; init; }

	/// <summary>
	/// Rate of true negatives for the class.
	/// This will be a value in the range `[0, 1]`.
	/// </summary>
	public required double TrueNegativeRate { get; init; }

	/// <summary>
	/// Rate of false negatives for the class.
	/// This will be a value in the range `[0, 1]`.
	/// </summary>
	public required double FalseNegativeRate { get; init; }

	/// <summary>
	/// Precision of the model for the class.
	/// This will be a value in the range `[0, 1]`.
	/// </summary>
	public required double Precision { get; init; }

	/// <summary>
	/// Recall of the model for the class.
	/// This will be a value in the range `[0, 1]`.
	/// </summary>
	public required double Recall { get; init; }

	/// <summary>
	/// F1 score of the model for the class.
	/// This will be a value in the range `[0, 1]`.
	/// </summary>
	public required double F1Score { get; init; }
}
