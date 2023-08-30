/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Events;
using static TorchSharp.torch;
namespace Lightspeed.Classification.Models;

/// <summary>
/// Interface for models that perform image classification.
/// </summary>
public interface IClassificationModelInstance
{
	/// <summary>
	/// Event that is fired when an epoch is completed.
	/// </summary>
	event EventHandler<OnEpochCompleteEventArgs>? OnEpochComplete;

	/// <summary>
	/// Device that the model is using.
	/// </summary>
	Device Device { get; }

	/// <summary>
	/// Model-specific hyperparameters used for the model.
	/// Each key-value pair in this dictionary will be the display name for the
	///   hyperparameter and the value for the hyperparameter.
	/// </summary>
	IReadOnlyDictionary<string, string> ModelHyperparameters { get; }

	/// <summary>
	/// Resets the model's internal data using the data previously saved to disk.
	/// </summary>
	/// <param name="loadBest">
	/// If `true`, the model will load the data from the best epoch. If `false`,
	///   the model will load the data from the most recent epoch.
	/// </param>
	/// <returns>A task set once the model's data has been updated.</returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown if the model is currently being trained.
	/// </exception>
	/// <exception cref="FileNotFoundException">
	/// Thrown if the model has not been saved to disk
	/// </exception>
	/// <exception cref="NotSupportedException">
	/// Thrown if the model type does not support loading data from disk.
	/// </exception>
	Task Load(bool loadBest = true);

	/// <summary>
	/// Saves model data to the path specified when the instance was created.
	/// </summary>
	/// <returns>A task set once the model's data has been saved.</returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown if the model is currently being trained.
	/// </exception>
	/// <exception cref="NotSupportedException">
	/// Thrown if the model type does not support saving data to disk.
	/// </exception>
	Task Save();

	/// <summary>
	/// Trains the model for the specified number of epochs.
	/// </summary>
	/// <param name="hyperparameters">
	/// Hyperparameters to use for training the model.
	/// </param>
	/// <param name="cancellationToken">
	/// Token allowing training to be cancelled early.
	/// </param>
	Task Train(
		Hyperparameters hyperparameters,
		CancellationToken cancellationToken
	);
}
