/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Validators;
using static TorchSharp.torch;
namespace Lightspeed.Classification.Models;

/// <summary>
/// Interface for model types designed for image classification.
/// `IClassificationModel` instances should be thought of as factory types for
///   the objects that perform the actual classification. This allows the same
///   model design to be instantiated for different datasets. See the
///   <see cref="IClassificationModelInstance"/> for the interface that
///   objects performing the classification operation must implement.
/// </summary>
public interface IClassificationModel
{
	/// <summary>
	/// Internal unique identifier for the model type.
	/// </summary>
	string Id { get; }

	/// <summary>
	/// Name to display on the UI for the model type.
	/// </summary>
	string DisplayName { get; }

	/// <summary>
	/// Brief description to display on the UI for the model type.
	/// This will be shown on the UI in the model selection list and on the
	///   model's detailed information page.
	/// </summary>
	string BriefDescription { get; }

	/// <summary>
	/// Detailed description to display on the UI for the model type.
	/// This description will only be displayed on the model's detailed
	///   information page.
	/// </summary>
	string DetailedDescription { get; }

	/// <summary>
	/// List of hyperparameters in the order they should appear on the UI.
	/// </summary>
	IReadOnlyList<IHyperparameterValidator> Hyperparameters { get; }

	/// <summary>
	/// Constructs a new instance of the model type.
	/// </summary>
	/// <param name="inputSize">
	/// Size of the tensors used as input to the model.
	/// </param>
	/// <param name="device">
	/// Device to use for the model.
	/// </param>
	/// <param name="outputSize">
	/// Size of the tensors used as output from the model.
	/// </param>
	/// <param name="saveFolder">
	/// Folder to save the model to. If the folder already exists and has data,
	///   the saved data will be used to initialize the model.
	/// </param>
	/// <param name="hyperparameters">
	/// Hyperparameters specified on the UI. Each hyperparameter will be indexed
	///   by the unique ID provided by the hyperparameter validator the value
	///   corresponds to. All string values provided will have been validated
	///   by the corresponding hyperparameter validator.
	/// With the exception of string hyperparameters, all values will be
	///   provided as numbers in string form. Boolean parameters will always
	///   be provided as either "0" or "1". Enum parameters will be provided
	///   as the index of the enum value in string form.
	/// </param>
	/// <returns>A new model instance.</returns>
	IClassificationModelInstance CreateInstance(
		Size inputSize,
		Size outputSize,
		Device device,
		string saveFolder,
		IReadOnlyDictionary<string, string> hyperparameters
	);
}
