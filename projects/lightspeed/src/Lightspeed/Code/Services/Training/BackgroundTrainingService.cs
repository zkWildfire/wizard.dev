/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Models;
namespace Lightspeed.Services.Training;

/// <summary>
/// Service that runs model training on a background thread.
/// </summary>
public class BackgroundTrainingService : IHostedTrainingService
{
	/// <summary>
	/// Stores callbacks allowing training to be cancelled.
	/// </summary>
	private readonly ConcurrentDictionary<int, Action>
		_cancellationCallbacks = new();

	/// <summary>
	/// Next ID to assign to a training task.
	/// </summary>
	private int _nextId = 1;

	/// <summary>
	/// Initializes the service.
	/// </summary>
	/// <param name="cancellationToken">
	/// Token allowing for cancellation of the operation.
	/// </param>
	/// <returns>A task set once the service has been started.</returns>
	public Task StartAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}

	/// <summary>
	/// Stops the service.
	/// </summary>
	/// <param name="cancellationToken">
	/// Token allowing for cancellation of the operation.
	/// </param>
	/// <returns>A task set once the service has been stopped.</returns>
	public Task StopAsync(CancellationToken cancellationToken)
	{
		foreach (var (_, callback) in _cancellationCallbacks)
		{
			callback();
		}
		return Task.CompletedTask;
	}

	/// <summary>
	/// Trains the model using the given hyperparameters.
	/// </summary>
	/// <param name="model">
	/// Model to train.
	/// </param>
	/// <param name="hyperparameters">
	/// Hyperparameters to pass to the model.
	/// </param>
	/// <param name="cancellationCallback">
	/// Callback that allows the service to cancel training of the model.
	/// </param>
	/// <param name="cancellationToken">
	/// Token passed to the model allowing for cancellation of the training.
	/// </param>
	/// <returns>A task set once training is complete.</returns>
	public Task TrainModel(
		IClassificationModelInstance model,
		Hyperparameters hyperparameters,
		Action cancellationCallback,
		CancellationToken cancellationToken)
	{
		// Save the cancellation callback so that it can be called if the
		//   service is stopped
		var id = _nextId++;
		_cancellationCallbacks[id] = cancellationCallback;

		// Start training the model on a background thread
		return Task.Run(async () =>
		{
			await model.Train(hyperparameters, cancellationToken)
				.ConfigureAwait(false);

			// Remove the cancellation callback once training is complete
			_ = _cancellationCallbacks.TryRemove(id, out _);
		}, cancellationToken);
	}
}
