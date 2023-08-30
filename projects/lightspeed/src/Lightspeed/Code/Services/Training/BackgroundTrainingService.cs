/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Models;
using Lightspeed.Classification.Training;
namespace Lightspeed.Services.Training;

/// <summary>
/// Service that runs model training on a background thread.
/// </summary>
public class BackgroundTrainingService : IHostedTrainingService
{
	/// <summary>
	/// Set of all active sessions.
	/// </summary>
	private readonly ConcurrentBag<ITrainingSession> _sessions = new();

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
		foreach (var session in _sessions)
		{
			session.Cancel();
		}

		Task.WaitAll(
			_sessions.Select(s => s.WaitAsync()).ToArray(),
			cancellationToken
		);
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
	/// <returns>
	/// An object that allows the training session to be monitored and
	///   controlled.
	/// </returns>
	public ITrainingSession TrainModel(
		IClassificationModelInstance model,
		Hyperparameters hyperparameters)
	{
		return new BackgroundTrainingSession(
			model,
			hyperparameters
		);
	}
}
