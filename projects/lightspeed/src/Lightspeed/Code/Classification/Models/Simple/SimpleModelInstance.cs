/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Events;
using TorchSharp;
using static TorchSharp.torch;
using static TorchSharp.torch.nn;
using static TorchSharp.torch.optim;
using static TorchSharp.torch.utils.data;
namespace Lightspeed.Classification.Models.Simple;

/// <summary>
/// Instance class for the simple model.
/// </summary>
public sealed class SimpleModelInstance : IClassificationModelInstance
{
	/// <summary>
	/// TorchSharp module that represents the model.
	/// </summary>
	private sealed class Model : Module<Tensor, Tensor>
	{
		/// <summary>
		/// First layer of the model.
		/// </summary>
		private readonly Module<Tensor, Tensor> _layer1;

		/// <summary>
		/// ReLU layer for the first layer of the model.
		/// </summary>
		private readonly Module<Tensor, Tensor> _relu1;

		/// <summary>
		/// Second layer of the model.
		/// </summary>
		private readonly Module<Tensor, Tensor> _layer2;

		/// <summary>
		/// LogSoftmax layer used to process the final model output.
		/// </summary>
		private readonly Module<Tensor, Tensor> _logsm;

		/// <summary>
		/// Initializes the model.
		/// </summary>
		/// <param name="inputSize">Size to use for the input layer.</param>
		/// <param name="hiddenSize">Size to use for the hidden layer.</param>
		/// <param name="outputSize">Size to use for the output layer.</param>
		/// <param name="device">Device that the model should use.</param>
		public Model(
			long inputSize,
			long hiddenSize,
			long outputSize,
			Device device)
			: base("simple")
		{
			_layer1 = Linear(inputSize, hiddenSize);
			_relu1 = ReLU();
			_layer2 = Linear(hiddenSize, outputSize);
			_logsm = LogSoftmax(1);

			RegisterComponents();
			_ = this.to(device);
		}

		/// <summary>
		/// Cleans up the model.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);

			if (disposing)
			{
				_layer1.Dispose();
				_relu1.Dispose();
				_layer2.Dispose();
				_logsm.Dispose();
				ClearModules();
			}
		}

		/// <summary>
		/// Runs a forward pass of the model.
		/// </summary>
		/// <param name="input">Tensor to feed through the model.</param>
		/// <returns>The output tensor from the model.</returns>
		public override Tensor forward(Tensor input)
		{
			input = input.reshape(-1, 784);
			var x = _layer1.forward(input);
			x = _relu1.forward(x);
			x = _layer2.forward(x);
			return _logsm.forward(x);
		}
	}

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
	/// Model instance being trained.
	/// </summary>
	private readonly Model _model;

	/// <summary>
	/// Folder to save model data to.
	/// </summary>
	private readonly string _saveFolder;

	/// <summary>
	/// Whether or not the model has been disposed.
	/// </summary>
	private bool _isDisposed;

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
		_saveFolder = saveFolder;

		// Set up the model
		var flattenedInputSize = inputSize.Aggregate(1L, (a, b) => a * b);
		var flattenedOutputSize = outputSize.Aggregate(1L, (a, b) => a * b);
		_model = new(
			flattenedInputSize,
			hiddenSize,
			flattenedOutputSize,
			device
		);
	}

	/// <summary>
	/// Cleans up the model.
	/// </summary>
	public void Dispose()
	{
		if (_isDisposed)
		{
			return;
		}

		_model.Dispose();
		GC.SuppressFinalize(this);
		_isDisposed = true;
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
		// TODO: Check if the model is currently being trained
		return Task.Run(() => _model.load(_saveFolder));
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
		// TODO: Check if the model is currently being trained
		return Task.Run(() => _model.save(_saveFolder));
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
		// Create the optimizer
		Optimizer optimizer = hyperparameters.Optimizer switch
		{
			EOptimizerType.Adam => Adam(
				_model.parameters(),
				hyperparameters.LearningRate
			),
			_ => throw new UnreachableException()
		};

		// Create the loss function
		Loss<Tensor, Tensor, Tensor> loss = hyperparameters.Loss switch
		{
			ELossType.CrossEntropy => CrossEntropyLoss(),
			_ => throw new UnreachableException()
		};

		var startTime = DateTime.UtcNow;
		for (var i = 0; i < hyperparameters.Epochs; i++)
		{
			// Check if the training has been cancelled
			if (cancellationToken.IsCancellationRequested)
			{
				break;
			}

			// Train the model for a single epoch
			TrainEpoch(
				optimizer,
				loss,
				hyperparameters.Dataset.TrainingSet.GetDataLoader(
					hyperparameters.BatchSize
				),
				i + 1,
				hyperparameters.Epochs,
				startTime
			);
		}

		return Task.CompletedTask;
	}

	/// <summary>
	/// Trains the model for a single epoch.
	/// </summary>
	/// <param name="optimizer">
	/// Optimizer to use when training the model.
	/// </param>
	/// <param name="loss">
	/// Loss function to use when training the model.
	/// </param>
	/// <param name="dataLoader">
	/// Data Loader to get training data from.
	/// </param>
	/// <param name="epoch">
	/// Index of the epoch this particular training loop is for. This should
	///   be a value in the range `(0, totalEpochs]`.
	/// </param>
	/// <param name="totalEpochs">
	/// Total number of epochs that will be trained.
	/// </param>
	/// <param name="trainingStartTime">
	/// Time that the training started at.
	/// </param>
	private void TrainEpoch(
		Optimizer optimizer,
		Loss<Tensor, Tensor, Tensor> loss,
		DataLoader dataLoader,
		int epoch,
		int totalEpochs,
		DateTime trainingStartTime)
	{
		using var scope = NewDisposeScope();

		// Keep track of metrics for the epoch
		var startTime = DateTime.UtcNow;
		var totalCount = 0L;
		var correctCount = 0L;
		var totalLoss = 0.0;

		// Run the training loop
		foreach (var data in dataLoader)
		{
			optimizer.zero_grad();

			// Invoke the model and loss function
			var target = data["label"];
			var prediction = _model.call(data["data"]);
			var output = loss.call(prediction, target);

			// Backpropagate the loss
			output.backward();
			_ = optimizer.step();

			// Update metrics
			totalCount++;
			totalLoss += output.flatten().sum().data<float>().First();
			var results = prediction.argmax(1) == target;
			correctCount += results.sum().data<long>().First();

			scope.DisposeEverything();
		}

		var endTime = DateTime.UtcNow;
		var totalDuration = endTime - trainingStartTime;
		OnEpochComplete?.Invoke(
			this,
			new()
			{
				CurrentEpoch = epoch,
				TotalEpochs = totalEpochs,
				EpochDuration = endTime - startTime,
				TotalDuration = totalDuration,
				AverageEpochDuration = totalDuration / epoch,
				Accuracy = correctCount / (double)totalCount,
				Loss = totalLoss
			}
		);
	}
}
