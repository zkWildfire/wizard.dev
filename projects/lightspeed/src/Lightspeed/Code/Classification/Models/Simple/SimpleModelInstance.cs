/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Events;
using TorchSharp.Modules;
using static TorchSharp.torch;
namespace Lightspeed.Classification.Models.Simple;

/// <summary>
/// Instance class for the simple model.
/// </summary>
public class SimpleModelInstance : IClassificationModelInstance
{
	/// <summary>
	/// Event that is fired when an epoch is completed.
	/// </summary>
	public event EventHandler<OnEpochCompleteEventArgs>? OnEpochComplete;

	/// <summary>
	/// Device that the model is using.
	/// </summary>
	public Device Device { get; }

	/// <summary>
	/// Model-specific hyperparameters used for the model.
	/// Each key-value pair in this dictionary will be the display name for the
	///   hyperparameter and the value for the hyperparameter.
	/// </summary>
	public IReadOnlyDictionary<string, string> ModelHyperparameters { get; }

	/// <summary>
	/// First layer of the model.
	/// </summary>
	private readonly Linear _layer1;

	/// <summary>
	/// Second layer of the model.
	/// </summary>
	private readonly Linear _layer2;

	/// <summary>
	/// Folder to save model data to.
	/// </summary>
	private readonly string _saveFolder;

	/// <summary>
	/// Initializes the model.
	/// </summary>
	/// <param name="inputSize">Size of each input tensor.</param>
	/// <param name="hiddenSize">
	/// Size of the 1D tensors used by the hidden layer.
	/// </param>
	/// <param name="outputSize">Size of each output tensor.</param>
	/// <param name="device">Device that the model should use.</param>
	/// <param name="saveFolder">
	/// Folder to write model data to. If this folder already exists and has
	///   model data, the saved data will be used to initialize the model.
	/// </param>
	/// <exception cref="ArgumentException">
	/// Thrown if the input or output size is invalid.
	/// </exception>
	/// <exception cref="InvalidOperationException">
	/// Thrown if the save folder's data is for a different model type.
	/// </exception>
	public SimpleModelInstance(
		Size inputSize,
		long hiddenSize,
		Size outputSize,
		Device device,
		string saveFolder)
	{
		Device = device;
		ModelHyperparameters = new Dictionary<string, string>()
		{
			{
				SimpleModelStatics.NAME_HIDDEN_LAYER_SIZE,
				hiddenSize.ToString(CultureInfo.InvariantCulture)
			}
		};
		var flattenedInputSize = inputSize.Aggregate(1L, (a, b) => a * b);
		var flattenedOutputSize = outputSize.Aggregate(1L, (a, b) => a * b);

		_layer1 = nn.Linear(flattenedInputSize, hiddenSize);
		_layer2 = nn.Linear(hiddenSize, flattenedOutputSize);
		_saveFolder = saveFolder;
	}

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
	public Task Load(bool loadBest = true)
	{
		throw new NotImplementedException();
	}

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
	public Task Save()
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// Trains the model for the specified number of epochs.
	/// </summary>
	/// <param name="hyperparameters">
	/// Hyperparameters to use for training the model.
	/// </param>
	/// <param name="cancellationToken">
	/// Token allowing training to be cancelled early.
	/// </param>
	public Task Train(
		Hyperparameters hyperparameters,
		CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}
