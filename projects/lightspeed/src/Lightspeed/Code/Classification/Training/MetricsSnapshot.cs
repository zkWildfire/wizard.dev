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
}
