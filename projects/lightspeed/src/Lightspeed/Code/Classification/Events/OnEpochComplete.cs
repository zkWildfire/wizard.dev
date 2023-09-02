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
	/// Time taken to complete the epoch.
	/// </summary>
	public required TimeSpan EpochDuration { get; init; }

	/// <summary>
	/// Average time spent per completed epoch.
	/// </summary>
	public required TimeSpan AverageEpochDuration { get; init; }

	/// <summary>
	/// Total time spent training so far.
	/// </summary>
	public required TimeSpan TotalDuration { get; init; }

	/// <summary>
	/// Accuracy of the model during the epoch.
	/// </summary>
	public required double Accuracy { get; init; }

	/// <summary>
	/// Loss of the model during the epoch.
	/// </summary>
	public required double Loss { get; init; }

	/// <summary>
	/// Converts the event args to a metrics snapshot.
	/// </summary>
	/// <returns></returns>
	public MetricsSnapshot ToMetricsSnapshot()
	{
		return new()
		{
			CurrentEpoch = CurrentEpoch,
			TotalEpochs = TotalEpochs,
			EpochDuration = EpochDuration,
			AverageEpochDuration = AverageEpochDuration,
			TotalDuration = TotalDuration,
			Accuracy = Accuracy,
			Loss = Loss
		};
	}
}
