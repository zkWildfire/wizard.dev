/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Training;
namespace Lightspeed.Serialization;

// Warning IDE0060: Remove unused parameter 'trainingMetrics' if it is not part
//   of a shipped public API
// This warning is being disabled because the methods defined by this class
//   match the signature of functor parameters in the `IStorage` interface's
//   `Load()` methods. This means that some or most methods in the class may
//   have parameters that are not used but are still needed to make the method
//   compatible with the `Load()` methods' parameters.
#pragma warning disable IDE0060

/// <summary>
/// Helper class that defines methods used for selecting models.
/// The methods defined in this class are passed to the `IStorage` interface's
///   `Load()` methods to select which model to load.
/// </summary>
public static class ModelSelectorStatics
{
	/// <summary>
	/// Selects the model with the highest accuracy on the validation dataset.
	/// </summary>
	/// <param name="trainingMetrics">
	/// Metrics that were calculated for the training dataset.
	/// </param>
	/// <param name="validationMetrics">
	/// Metrics that were calculated for the validation dataset.
	/// </param>
	/// <returns>
	/// The accuracy of the model on the validation dataset.
	/// </returns>
	public static double SelectByValidationAccuracy(
		ModelMetrics trainingMetrics,
		ModelMetrics validationMetrics)
	{
		return validationMetrics.Accuracy;
	}
}

#pragma warning restore IDE0060
