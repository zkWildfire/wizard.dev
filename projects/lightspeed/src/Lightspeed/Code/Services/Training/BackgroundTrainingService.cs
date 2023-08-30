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
	/// Collection of all sessions, indexed by session ID.
	/// </summary>
	public IReadOnlyDictionary<Guid, ITrainingSession> Sessions => _sessions;

	/// <summary>
	/// Collection of all active sessions, indexed by session ID.
	/// </summary>
	public IReadOnlyDictionary<Guid, ITrainingSession> ActiveSessions =>
		_activeSessions;

	/// <summary>
	/// Set of all sessions.
	/// </summary>
	private readonly ConcurrentDictionary<Guid, ITrainingSession> _sessions =
		new();

	/// <summary>
	/// Set of all active sessions.
	/// </summary>
	private readonly ConcurrentDictionary<Guid, ITrainingSession>
		_activeSessions = new();

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
		foreach (var session in _activeSessions.Values)
		{
			session.Cancel();
		}

		Task.WaitAll(
			_activeSessions.Values.Select(s => s.WaitAsync()).ToArray(),
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
		var session = new BackgroundTrainingSession(
			model,
			hyperparameters
		);

		// Save the session
		var result = _sessions.TryAdd(session.SessionId, session);
		result &= _activeSessions.TryAdd(session.SessionId, session);
		Debug.Assert(result);

		// Remove the session from the active sessions once it completes
		session.OnTrainingCompleted += (sender, args) =>
		{
			_ = _activeSessions.TryRemove(session.SessionId, out _);
		};

		// In case the session already completed for some reason, check its
		//   status and remove it from the active sessions if necessary
		if (!session.IsActive)
		{
			_ = _activeSessions.TryRemove(session.SessionId, out _);
		}

		return session;
	}
}
