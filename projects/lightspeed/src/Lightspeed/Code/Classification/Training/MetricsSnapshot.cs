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
	/// </summary>
	public required int Epoch { get; init; }

	/// <summary>
	/// Accuracy of the model during the epoch.
	/// </summary>
	public required float Accuracy { get; init; }

	/// <summary>
	/// Loss of the model during the epoch.
	/// </summary>
	public required float Loss { get; init; }
}
