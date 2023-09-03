/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Training;

namespace Lightspeed.Classification.Events;

/// <summary>
/// Type used as the event args when an epoch is completed.
/// </summary>
public class OnEpochCompleteEventArgs : EventArgs
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
	/// Converts the event args to a metrics snapshot.
	/// </summary>
	/// <returns>
	/// A metrics snapshot that contains the same data as the event args.
	/// </returns>
	public MetricsSnapshot ToMetricsSnapshot()
	{
		return new MetricsSnapshot()
		{
			CurrentEpoch = CurrentEpoch,
			TotalEpochs = TotalEpochs,
			TotalDuration = TotalDuration,
			TrainingMetrics = TrainingMetrics,
			ValidationMetrics = ValidationMetrics
		};
	}
}
