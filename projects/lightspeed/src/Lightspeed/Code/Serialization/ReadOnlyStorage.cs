/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Training;
using static TorchSharp.torch;
using static TorchSharp.torch.nn;
namespace Lightspeed.Serialization;

/// <summary>
/// `IStorage` implementation that only allows model loading to occur.
/// Any attempts to save a model will be silently ignored.
/// </summary>
/// <remarks>
/// This class is meant for use when evaluating models. Models are not expected
///   to be saved back to disk when evaluating them, so this class will prevent
///   any accidental attempts to do so.
/// </remarks>
public class ReadOnlyStorage : IStorage
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
	public Module<Tensor, Tensor> LoadBest(
		Func<ModelMetrics, ModelMetrics, double> evaluator)
	{
		throw new NotImplementedException();
	}

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
	public Task<Module<Tensor, Tensor>> LoadBestAsync(
		Func<ModelMetrics, ModelMetrics, double> evaluator)
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// Loads the most recently saved model from the underlying storage location.
	/// </summary>
	/// <returns>
	/// The deserialized model initialized with the most recently saved weights.
	/// </returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown if no model has been saved yet.
	/// </exception>
	public Module<Tensor, Tensor> LoadMostRecent()
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// Loads the most recently saved model from the underlying storage location.
	/// </summary>
	/// <returns>
	/// The deserialized model initialized with the most recently saved weights.
	/// </returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown if no model has been saved yet.
	/// </exception>
	public Task<Module<Tensor, Tensor>> LoadMostRecentAsync()
	{
		throw new NotImplementedException();
	}

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
	public void Save(
		Module<Tensor, Tensor> model,
		int epoch,
		ModelMetrics trainingMetrics,
		ModelMetrics validationMetrics)
	{
		// Do nothing
	}

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
	public Task SaveAsync(
		Module<Tensor, Tensor> model,
		int epoch,
		ModelMetrics trainingMetrics,
		ModelMetrics validationMetrics)
	{
		// Do nothing
		return Task.CompletedTask;
	}
}
