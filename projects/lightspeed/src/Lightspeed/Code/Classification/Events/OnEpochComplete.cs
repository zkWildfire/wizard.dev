/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Lightspeed.Classification.Events;

/// <summary>
/// Type used as the event args when an epoch is completed.
/// </summary>
public class OnEpochCompleteEventArgs : EventArgs
{
	/// <summary>
	/// Index of the epoch that was completed.
	/// This will be a value in the range `(0, TotalEpochs]`.
	/// </summary>
	public required int CompletedEpoch { get; init; }

	/// <summary>
	/// Total number of epochs that will be completed.
	/// </summary>
	public required int TotalEpochs { get; init; }

	/// <summary>
	/// Accuracy of the model for the most recent epoch.
	/// </summary>
	public required float Accuracy { get; init; }

	/// <summary>
	/// Loss of the model for the most recent epoch.
	/// </summary>
	public required float Loss { get; init; }

	/// <summary>
	/// Time taken to complete the most recent epoch.
	/// </summary>
	public required TimeSpan EpochDuration { get; init; }

	/// <summary>
	/// Time taken to complete all epochs so far.
	/// </summary>
	public required TimeSpan TotalDuration { get; init; }
}
