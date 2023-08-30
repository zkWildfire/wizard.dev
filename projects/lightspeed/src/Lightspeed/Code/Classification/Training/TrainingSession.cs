/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Events;
namespace Lightspeed.Classification.Training;

/// <summary>
/// Interface that provides thread safe access to a training session.
/// </summary>
public interface ITrainingSession : IDisposable
{
	/// <summary>
	/// Event that is fired when an epoch is completed.
	/// </summary>
	event EventHandler<OnEpochCompleteEventArgs>? OnEpochComplete;

	/// <summary>
	/// Whether or not the training session is currently running.
	/// </summary>
	bool IsActive { get; }

	/// <summary>
	/// Epoch that the training session is currently on.
	/// </summary>
	int CurrentEpoch { get; }

	/// <summary>
	/// Total number of epochs that will be completed.
	/// </summary>
	int TotalEpochs { get; }

	/// <summary>
	/// Average time spent per completed epoch.
	/// </summary>
	TimeSpan AverageEpochDuration { get; }

	/// <summary>
	/// Time since the model began training.
	/// </summary>
	TimeSpan TotalDuration { get; }

	/// <summary>
	/// Gets the metrics for each epoch that has been completed.
	/// </summary>
	/// <remarks>
	/// This will return a list that is guaranteed to be immutable even if
	///   training proceeds. However, this means that the list will not be
	///   updated with new metrics as training proceeds.
	/// </remarks>
	IReadOnlyList<MetricsSnapshot> Metrics { get; }

	/// <summary>
	/// Stops the training session.
	/// Most training sessions will not stop immediately, but will instead
	///   stop at the end of the current epoch.
	/// </summary>
	void Cancel();

	/// <summary>
	/// Waits for the training session to complete.
	/// </summary>
	/// <returns>
	/// A task set once the training session has completed or been cancelled.
	/// </returns>
	Task WaitAsync();
}
