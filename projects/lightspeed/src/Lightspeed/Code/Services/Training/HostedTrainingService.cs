/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Models;
using Lightspeed.Classification.Training;
namespace Lightspeed.Services.Training;

/// <summary>
/// Service that allows a model to be trained in the background.
/// </summary>
public interface IHostedTrainingService : IHostedService
{
	/// <summary>
	/// Collection of all sessions, indexed by session ID.
	/// </summary>
	IReadOnlyDictionary<Guid, ITrainingSession> Sessions { get; }

	/// <summary>
	/// Collection of all active sessions, indexed by session ID.
	/// </summary>
	IReadOnlyDictionary<Guid, ITrainingSession> ActiveSessions { get; }

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
	ITrainingSession TrainModel(
		IClassificationModelInstance model,
		Hyperparameters hyperparameters
	);
}
