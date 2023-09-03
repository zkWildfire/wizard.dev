/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Lightspeed.Classification.Training;

/// <summary>
/// Snapshot of the metrics for a model from a single epoch.
/// </summary>
public readonly record struct MetricsSnapshot
{
	/// <summary>
	/// Epoch that the snapshot was taken at.
	/// This will be a value in the range `(0, TotalEpochs]`.
	/// </summary>
	public required int CurrentEpoch { get; init; }

	/// <summary>
	/// Total number of epochs that will be completed.
	/// </summary>
	public required int TotalEpochs { get; init; }

	/// <summary>
	/// Total time spent training so far.
	/// </summary>
	public required TimeSpan TotalDuration { get; init; }

	/// <summary>
	/// Metrics from the epoch for the training dataset.
	/// </summary>
	public required ModelMetrics TrainingMetrics { get; init; }

	/// <summary>
	/// Metrics from the epoch for the validation dataset.
	/// </summary>
	public required ModelMetrics ValidationMetrics { get; init; }

	/// <summary>
	/// Average duration of an epoch.
	/// </summary>
	public TimeSpan AverageEpochDuration
	{
		get
		{
			return CurrentEpoch == 0
				? TimeSpan.Zero
				: TotalDuration / CurrentEpoch;
		}
	}
}
