/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Events;
using Lightspeed.Classification.Models;

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
	/// Unique ID assigned to the session.
	/// </summary>
	Guid SessionId { get; }

	/// <summary>
	/// Whether or not the training session is currently running.
	/// </summary>
	bool IsActive { get; }

	/// <summary>
	/// Metrics for the last completed epoch.
	/// If the training session has not completed an epoch yet, the snapshot
	///   will have most fields set to 0.
	/// </summary>
	/// <remarks>
	/// Retrieving this data is thread safe. All data in the snapshot will be
	///   consistent with each other.
	/// </remarks>
	MetricsSnapshot CurrentMetrics { get; }

	/// <summary>
	/// Hyperparameters used for the training session.
	/// </summary>
	Hyperparameters Hyperparameters { get; }

	/// <summary>
	/// Model-specific hyperparameters used for the model.
	/// Each key-value pair in this dictionary will be the display name for the
	///   hyperparameter and the value for the hyperparameter.
	/// </summary>
	IReadOnlyDictionary<string, string> ModelHyperparameters { get; }

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
