/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Training;
using static TorchSharp.torch;
using static TorchSharp.torch.nn;
namespace Lightspeed.Serialization;

/// <summary>
/// Interface for classes that represent a storage location.
/// </summary>
public interface IStorage
{
	/// <summary>
	/// Loads the best model from the underlying storage location.
	/// </summary>
	/// <param name="evaluator">
	/// Functor to invoke on each model to get the metric to use for comparison.
	///   This functor will be passed the training metrics and validation
	///   metrics for each saved set of model weights, respectively. Whichever
	///   set of weights results in the highest value being returned by this
	///   functor will be loaded.
	/// </param>
	/// <returns>
	/// The deserialized model initialized with the weights that exhibited the
	///   best performance.
	/// </returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown if no model has been saved yet.
	/// </exception>
	Module<Tensor, Tensor> LoadBest(
		Func<ModelMetrics, ModelMetrics, double> evaluator
	);

	/// <summary>
	/// Loads the best model from the underlying storage location.
	/// </summary>
	/// <param name="evaluator">
	/// Functor to invoke on each model to get the metric to use for comparison.
	///   This functor will be passed the training metrics and validation
	///   metrics for each saved set of model weights, respectively. Whichever
	///   set of weights results in the highest value being returned by this
	///   functor will be loaded.
	/// </param>
	/// <returns>
	/// The deserialized model initialized with the weights that exhibited the
	///   best performance.
	/// </returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown if no model has been saved yet.
	/// </exception>
	Task<Module<Tensor, Tensor>> LoadBestAsync(
		Func<ModelMetrics, ModelMetrics, double> evaluator
	);

	/// <summary>
	/// Loads the most recently saved model from the underlying storage location.
	/// </summary>
	/// <returns>
	/// The deserialized model initialized with the most recently saved weights.
	/// </returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown if no model has been saved yet.
	/// </exception>
	Module<Tensor, Tensor> LoadMostRecent();

	/// <summary>
	/// Loads the most recently saved model from the underlying storage location.
	/// </summary>
	/// <returns>
	/// The deserialized model initialized with the most recently saved weights.
	/// </returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown if no model has been saved yet.
	/// </exception>
	Task<Module<Tensor, Tensor>> LoadMostRecentAsync();

	/// <summary>
	/// Saves the model to the underlying storage location.
	/// </summary>
	/// <param name="model">
	/// Model to save.
	/// </param>
	/// <param name="epoch">
	/// Epoch that was just completed.
	/// </param>
	/// <param name="trainingMetrics">
	/// Metrics from the most recent epoch for the training dataset.
	/// </param>
	/// <param name="validationMetrics">
	/// Metrics from the most recent epoch for the validation dataset.
	/// </param>
	void Save(
		Module<Tensor, Tensor> model,
		int epoch,
		ModelMetrics trainingMetrics,
		ModelMetrics validationMetrics
	);

	/// <summary>
	/// Saves the model to the underlying storage location.
	/// </summary>
	/// <param name="model">
	/// Model to save.
	/// </param>
	/// <param name="epoch">
	/// Epoch that was just completed.
	/// </param>
	/// <param name="trainingMetrics">
	/// Metrics from the most recent epoch for the training dataset.
	/// </param>
	/// <param name="validationMetrics">
	/// Metrics from the most recent epoch for the validation dataset.
	/// </param>
	/// <returns>
	/// A task set once the model has been saved.
	/// </returns>
	Task SaveAsync(
		Module<Tensor, Tensor> model,
		int epoch,
		ModelMetrics trainingMetrics,
		ModelMetrics validationMetrics
	);
}
