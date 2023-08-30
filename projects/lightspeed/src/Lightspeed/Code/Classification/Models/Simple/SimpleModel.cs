/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Validators;
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
	/// Path to the icon to display on the UI for the model type.
	/// </summary>
	public static readonly Uri ICON_PATH = new(
		"/img/models/classification/simple.png",
		UriKind.Relative
	);

	/// <summary>
	/// Internal unique identifier for the model type.
	/// </summary>
	public string Id => MODEL_ID;

	/// <summary>
	/// Name to display on the UI for the model type.
	/// </summary>
	public string DisplayName => "Simple 2-Layer Classification Model";

	/// <summary>
	/// Path to the icon to display on the UI for the model type.
	/// </summary>
	public Uri IconPath => ICON_PATH;

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
	/// List of hyperparameters in the order they should appear on the UI.
	/// </summary>
	public IReadOnlyList<IHyperparameterValidator> Hyperparameters { get; }

	/// <summary>
	/// Initializes the model instance.
	/// </summary>
	public SimpleModel()
	{
		Hyperparameters = new List<IHyperparameterValidator>()
		{
			new IntHyperparameterValidator(
				SimpleModelStatics.PARAMETER_HIDDEN_LAYER_SIZE,
				SimpleModelStatics.NAME_HIDDEN_LAYER_SIZE,
				"Size of the model's hidden layer.",
				new List<Func<int, bool>>()
				{
					x => x > 0
				},
				"Must be greater than 0.",
				50,
				false
			)
		};
	}

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
	public IClassificationModelInstance CreateInstance(
		Size inputSize,
		Size outputSize,
		Device device,
		string saveFolder,
		IReadOnlyDictionary<string, string?> hyperparameters)
	{
		var hiddenLayerSize = int.Parse(
			hyperparameters[SimpleModelStatics.PARAMETER_HIDDEN_LAYER_SIZE]!,
			CultureInfo.InvariantCulture
		);

		return new SimpleModelInstance(
			inputSize,
			hiddenLayerSize,
			outputSize,
			device,
			saveFolder
		);
	}
}
