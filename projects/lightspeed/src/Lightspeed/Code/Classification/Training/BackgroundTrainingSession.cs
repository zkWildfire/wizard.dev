/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Events;
using Lightspeed.Classification.Models;
namespace Lightspeed.Classification.Training;

/// <summary>
/// Training session implementation that runs on a background thread.
/// </summary>
public sealed class BackgroundTrainingSession : ITrainingSession
{
	/// <summary>
	/// Event that is fired when an epoch is completed.
	/// </summary>
	public event EventHandler<OnEpochCompleteEventArgs>? OnEpochComplete;

	/// <summary>
	/// Event that is fired when the training session is completed.
	/// </summary>
	public event EventHandler? OnTrainingCompleted;

	/// <summary>
	/// Unique ID assigned to the session.
	/// </summary>
	public Guid SessionId { get; }

	/// <summary>
	/// Whether or not the training session is currently running.
	/// </summary>
	public bool IsActive => !_trainingComplete;

	/// <summary>
	/// Metrics for the last completed epoch.
	/// If the training session has not completed an epoch yet, the snapshot
	///   will have most fields set to 0.
	/// </summary>
	/// <remarks>
	/// Retrieving this data is thread safe. All data in the snapshot will be
	///   consistent with each other.
	/// </remarks>
	public MetricsSnapshot CurrentMetrics => _metrics.Any()
		? _metrics.Last()
		: new MetricsSnapshot()
		{
			CurrentEpoch = 0,
			TotalEpochs = Hyperparameters.Epochs,
			EpochDuration = TimeSpan.Zero,
			AverageEpochDuration = TimeSpan.Zero,
			TotalDuration = DateTime.UtcNow - _trainingStartTime,
			Accuracy = 0,
			Loss = 0
		};

	/// <summary>
	/// Hyperparameters used for the training session.
	/// </summary>
	public Hyperparameters Hyperparameters { get; }

	/// <summary>
	/// Model-specific hyperparameters used for the model.
	/// Each key-value pair in this dictionary will be the display name for the
	///   hyperparameter and the value for the hyperparameter.
	/// </summary>
	public IReadOnlyDictionary<string, string> ModelHyperparameters =>
		_model.ModelHyperparameters;

	/// <summary>
	/// Gets the metrics for each epoch that has been completed.
	/// </summary>
	/// <remarks>
	/// This will return a list that is guaranteed to be immutable even if
	///   training proceeds. However, this means that the list will not be
	///   updated with new metrics as training proceeds.
	/// </remarks>
	public IReadOnlyList<MetricsSnapshot> Metrics =>
		new List<MetricsSnapshot>(_metrics);

	/// <summary>
	/// Model being trained.
	/// </summary>
	private readonly IClassificationModelInstance _model;

	/// <summary>
	/// Task set once the training session has completed or been cancelled.
	/// </summary>
	private readonly Task _trainingTask;

	/// <summary>
	/// Cancellation token source used to cancel the training session.
	/// </summary>
	private readonly CancellationTokenSource _cts = new();

	/// <summary>
	/// Metrics for each epoch that has been completed.
	/// </summary>
	private readonly ConcurrentQueue<MetricsSnapshot> _metrics =
		new();

	/// <summary>
	/// Time that the training session started.
	/// </summary>
	private readonly DateTime _trainingStartTime;

	/// <summary>
	/// Time that the current epoch started.
	/// </summary>
	private DateTime _epochStartTime;

	/// <summary>
	/// Time at which training completed.
	/// </summary>
	private DateTime? _trainingEndTime;

	/// <summary>
	/// Whether or not the training session has completed.
	/// </summary>
	private bool _trainingComplete;

	/// <summary>
	/// Whether or not the training session has been disposed.
	/// </summary>
	private bool _isDisposed;

	/// <summary>
	/// Initializes the session and starts it.
	/// </summary>
	/// <param name="model">Model to train.</param>
	/// <param name="hyperparameters">Hyperparameters for the model.</param>
	public BackgroundTrainingSession(
		IClassificationModelInstance model,
		Hyperparameters hyperparameters)
	{
		SessionId = Guid.NewGuid();
		Hyperparameters = hyperparameters;
		_model = model;
		_trainingStartTime = DateTime.UtcNow;
		_epochStartTime = _trainingStartTime;

		// Hook into events
		_model.OnEpochComplete += NotifyOnEpochComplete;

		// Start training the model on a background thread
		_trainingTask = Task.Run(async () =>
		{
			await model.Train(hyperparameters, _cts.Token)
				.ConfigureAwait(false);
			_trainingComplete = true;
		}, _cts.Token);
	}

	/// <summary>
	/// Disposes of the training session.
	/// </summary>
	public void Dispose()
	{
		if (_isDisposed)
		{
			return;
		}

		_cts.Dispose();
		GC.SuppressFinalize(this);
		_isDisposed = true;
	}

	/// <summary>
	/// Stops the training session.
	/// Most training sessions will not stop immediately, but will instead
	///   stop at the end of the current epoch.
	/// </summary>
	public void Cancel()
	{
		_cts.Cancel();
	}

	/// <summary>
	/// Waits for the training session to complete.
	/// </summary>
	/// <returns>
	/// A task set once the training session has completed or been cancelled.
	/// </returns>
	public Task WaitAsync()
	{
		return _trainingTask;
	}

	/// <summary>
	/// Callback invoked when the model completes an epoch.
	/// </summary>
	/// <param name="sender">Object broadcasting the event.</param>
	/// <param name="args">Event args for the event.</param>
	private void NotifyOnEpochComplete(
		object? sender,
		OnEpochCompleteEventArgs args)
	{
		// Update the stored metrics
		_metrics.Enqueue(new()
		{
			CurrentEpoch = args.CurrentEpoch,
			TotalEpochs = Hyperparameters.Epochs,
			EpochDuration = DateTime.UtcNow - _epochStartTime,
			AverageEpochDuration = args.AverageEpochDuration,
			TotalDuration = DateTime.UtcNow - _trainingStartTime,
			Accuracy = args.Accuracy,
			Loss = args.Loss
		});

		// Update internal state
		var isLastEpoch = args.CurrentEpoch == Hyperparameters.Epochs;
		_epochStartTime = DateTime.UtcNow;
		if (isLastEpoch)
		{
			_trainingEndTime = _epochStartTime;
		}

		// Broadcast to events
		OnEpochComplete?.Invoke(this, args);
		if (isLastEpoch)
		{
			OnTrainingCompleted?.Invoke(this, EventArgs.Empty);
		}
	}
}
