/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Models;
namespace Lightspeed.Services.Training;

/// <summary>
/// Service that allows a model to be trained in the background.
/// </summary>
public interface IHostedTrainingService : IHostedService
{
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
	Task TrainModel(
		IClassificationModelInstance model,
		Hyperparameters hyperparameters,
		Action cancellationCallback,
		CancellationToken cancellationToken
	);
}
