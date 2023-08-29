/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using static TorchSharp.torch;
namespace Lightspeed.Classification.Models.Simple;

/// <summary>
/// Simple, 2-layer model.
/// </summary>
public class SimpleModel : IClassificationModel
{
	/// <summary>
	/// Internal unique identifier for the model type.
	/// </summary>
	public const string MODEL_ID = "classification-simple";

	/// <summary>
	/// Internal unique identifier for the model type.
	/// </summary>
	public string Id => MODEL_ID;

	/// <summary>
	/// Name to display on the UI for the model type.
	/// </summary>
	public string DisplayName => "Simple 2-Layer Classification Model";

	/// <summary>
	/// Brief description to display on the UI for the model type.
	/// This will be shown on the UI in the model selection list and on the
	///   model's detailed information page.
	/// </summary>
	public string BriefDescription =>
		"A basic, 2 layer model used for testing purposes only.";

	/// <summary>
	/// Detailed description to display on the UI for the model type.
	/// This description will only be displayed on the model's detailed
	///   information page.
	/// </summary>
	public string DetailedDescription =>
		"The simple model is a basic 2-layer model used to test Lightspeed " +
		"code. It is not intended to be an accurate model nor is it meant " +
		"to be used for learning purposes.";

	/// <summary>
	/// Size to use for the model's hidden layer.
	/// </summary>
	private const int HIDDEN_LAYER_SIZE = 50;

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
	/// <returns>A new model instance.</returns>
	public IClassificationModelInstance CreateInstance(
		Size inputSize,
		Size outputSize,
		Device device,
		string saveFolder)
	{
		return new SimpleModelInstance(
			inputSize,
			HIDDEN_LAYER_SIZE,
			outputSize,
			device,
			saveFolder
		);
	}
}
