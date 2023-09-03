/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Lightspeed.Classification.Training;

/// <summary>
/// Struct that contains all metrics for a model.
/// </summary>
public readonly record struct ModelMetrics
{
	/// <summary>
	/// Per-class metrics that were computed for the model.
	/// </summary>
	public required IReadOnlyList<ClassMetrics> ClassMetrics { get; init; }

	/// <summary>
	/// Overall accuracy of the model across all classes.
	/// This will be a value in the range `[0, 1]`.
	/// </summary>
	public required double Accuracy { get; init; }

	/// <summary>
	/// Total number of samples that were used to compute the metrics.
	/// </summary>
	public required long Count { get; init; }

	/// <summary>
	/// Average rate of true positives across all classes.
	/// This will be a value in the range `[0, 1]`. This value will be computed
	///   as a macro-average, not a micro-average.
	/// </summary>
	public required double TruePositiveRate { get; init; }

	/// <summary>
	/// Average rate of false positives across all classes.
	/// This will be a value in the range `[0, 1]`. This value will be computed
	///   as a macro-average, not a micro-average.
	/// </summary>
	public required double FalsePositiveRate { get; init; }

	/// <summary>
	/// Average rate of true negatives across all classes.
	/// This will be a value in the range `[0, 1]`. This value will be computed
	///   as a macro-average, not a micro-average.
	/// </summary>
	public required double TrueNegativeRate { get; init; }

	/// <summary>
	/// Average rate of false negatives across all classes.
	/// This will be a value in the range `[0, 1]`. This value will be computed
	///   as a macro-average, not a micro-average.
	/// </summary>
	public required double FalseNegativeRate { get; init; }

	/// <summary>
	/// Average precision of the model across all classes.
	/// This will be a value in the range `[0, 1]`. This value will be computed
	///   as a macro-average, not a micro-average.
	/// </summary>
	public required double Precision { get; init; }

	/// <summary>
	/// Average recall of the model across all classes.
	/// This will be a value in the range `[0, 1]`. This value will be computed
	///   as a macro-average, not a micro-average.
	/// </summary>
	public required double Recall { get; init; }

	/// <summary>
	/// Average F1 score of the model across all classes.
	/// This will be a value in the range `[0, 1]`. This value will be computed
	///   as a macro-average, not a micro-average.
	/// </summary>
	public required double F1Score { get; init; }

	/// <summary>
	/// Total loss over the epoch.
	/// </summary>
	public required double Loss { get; init; }
}
