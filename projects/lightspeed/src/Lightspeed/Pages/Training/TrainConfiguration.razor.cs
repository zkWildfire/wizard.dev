/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification;
using Lightspeed.Classification.Models;
using Lightspeed.Classification.Validators;
using Lightspeed.Components.Training;
using Lightspeed.Components.Validators;
using Lightspeed.Services.Datasets;
using Lightspeed.Services.Models;
using Lightspeed.Services.Training;
using TorchSharp;
using static Lightspeed.Classification.Models.GenericHyperparameterStatics;
namespace Lightspeed.Pages.Training;

/// <summary>
/// Handles displaying the train configuration page.
/// This page allows users to specify the dataset, model, and model parameters
///   for a training session.
/// </summary>
public partial class TrainConfiguration : ComponentBase
{
	/// <summary>
	/// Service used to get all available datasets.
	/// </summary>
	[Inject]
	private IDatasetService DatasetService { get; set; } = null!;

	/// <summary>
	/// Service used to get all available models.
	/// </summary>
	[Inject]
	private IModelsService ModelsService { get; set; } = null!;

	/// <summary>
	/// Navigation manager for the app.
	/// </summary>
	[Inject]
	private NavigationManager NavigationManager { get; set; } = null!;

	/// <summary>
	/// Service that manages all training sessions.
	/// </summary>
	[Inject]
	private IHostedTrainingService TrainingService { get; set; } = null!;

	/// <summary>
	/// Helper property used to bind to dataset selector events.
	/// </summary>
	private DatasetSelector? DatasetSelectors
	{
		set
		{
			Debug.Assert(value != null);
			value.OnDatasetSelected += OnDatasetSelected;
		}
	}

	/// <summary>
	/// Helper property used to bind to model selector events.
	/// </summary>
	private ModelSelector? ModelSelectors
	{
		set
		{
			Debug.Assert(value != null);
			value.OnModelSelected += OnModelSelected;
			if (IsSelectedModel(value.Model))
			{
				_modelSelector = value;
			}
		}
	}

	/// <summary>
	/// Helper property used to bind to hyperparameter components.
	/// </summary>
	private Hyperparameter HyperparameterComponent
	{
		set
		{
			_genericHyperparameterValues[value.Validator.Id] = value.Value;
			value.OnHyperparameterSet += (sender, args) =>
			{
				_genericHyperparameterValues[args.Validator.Id] = args.Value;
			};
		}
	}

	/// <summary>
	/// Whether dataset shuffling should be enabled.
	/// </summary>
	private bool Shuffle
	{
		get
		{
			// Get the string representing the shuffle value
			var shuffleStr = _genericHyperparameterValues[
				_shuffleHyperparameter.Id
			];
			Debug.Assert(shuffleStr != null);
			return shuffleStr == "1";
		}
	}

	/// <summary>
	/// Gets the selected optimizer type.
	/// </summary>
	private EOptimizerType OptimizerType
	{
		get
		{
			// Get the string representing the optimizer type
			var displayName = _genericHyperparameterValues[
				_optimizerTypeHyperparameter.Id
			];
			Debug.Assert(displayName != null);
			return _optimizerTypeHyperparameter.EnumValues[
				displayName
			];
		}
	}

	/// <summary>
	/// Gets the selected loss function.
	/// </summary>
	private ELossType LossType
	{
		get
		{
			// Get the string representing the loss type
			var displayName = _genericHyperparameterValues[
				_lossTypeHyperparameter.Id
			];
			Debug.Assert(displayName != null);
			return _lossTypeHyperparameter.EnumValues[
				displayName
			];
		}
	}

	/// <summary>
	/// Gets the selected learning rate.
	/// </summary>
	private float LearningRate
	{
		get
		{
			// Get the string representing the learning rate
			var learningRateStr = _genericHyperparameterValues[
				_learningRateHyperparameter.Id
			];
			Debug.Assert(learningRateStr != null);
			return float.Parse(
				learningRateStr,
				CultureInfo.InvariantCulture
			);
		}
	}

	/// <summary>
	/// Gets the selected number of epochs.
	/// </summary>
	private int Epochs
	{
		get
		{
			// Get the string representing the number of epochs
			var epochsStr = _genericHyperparameterValues[
				_epochsHyperparameter.Id
			];
			Debug.Assert(epochsStr != null);
			return int.Parse(
				epochsStr,
				CultureInfo.InvariantCulture
			);
		}
	}

	/// <summary>
	/// Gets the selected batch size.
	/// </summary>
	private int BatchSize
	{
		get
		{
			// Get the string representing the batch size
			var batchSizeStr = _genericHyperparameterValues[
				_batchSizeHyperparameter.Id
			];
			Debug.Assert(batchSizeStr != null);
			return int.Parse(
				batchSizeStr,
				CultureInfo.InvariantCulture
			);
		}
	}

	/// <summary>
	/// Gets the selected number of epochs between saves.
	/// </summary>
	private int SaveEpochs
	{
		get
		{
			// Get the string representing the number of epochs between saves
			var saveEpochsStr = _genericHyperparameterValues[
				_saveEpochsHyperparameter.Id
			];
			Debug.Assert(saveEpochsStr != null);
			return int.Parse(
				saveEpochsStr,
				CultureInfo.InvariantCulture
			);
		}
	}

	/// <summary>
	/// Gets the path to save the model data to.
	/// </summary>
	private string SavePath
	{
		get
		{
			// Get the string representing the save path
			var savePath = _genericHyperparameterValues[
				_savePathHyperparameter.Id
			];
			Debug.Assert(savePath != null);
			return savePath;
		}
	}

	/// <summary>
	/// Dataset currently selected by the user.
	/// </summary>
	private IDataset? _selectedDataset;

	/// <summary>
	/// Model currently selected by the user.
	/// </summary>
	private IClassificationModel? _selectedModel;

	/// <summary>
	/// Model selector corresponding to the selected model.
	/// </summary>
	private ModelSelector? _modelSelector;

	/// <summary>
	/// List of hyperparameter validators for generic hyperparameters.
	/// Hyperparameters are added to this list in the order they should appear
	///   on the UI.
	/// </summary>
	private readonly IReadOnlyList<IHyperparameterValidator>
		_genericHyperparameters;

	/// <summary>
	/// Hyperparameter validator for the shuffle hyperparameter.
	/// </summary>
	private readonly BoolHyperparameterValidator _shuffleHyperparameter;

	/// <summary>
	/// Hyperparameter validator for the optimizer hyperparameter.
	/// </summary>
	private readonly EnumHyperparameterValidator<EOptimizerType>
		_optimizerTypeHyperparameter;

	/// <summary>
	/// Hyperparameter validator for the loss hyperparameter.
	/// </summary>
	private readonly EnumHyperparameterValidator<ELossType>
		_lossTypeHyperparameter;

	/// <summary>
	/// Hyperparameter validator for the learning rate hyperparameter.
	/// </summary>
	private readonly FloatHyperparameterValidator _learningRateHyperparameter;

	/// <summary>
	/// Hyperparameter validator for the number of epochs hyperparameter.
	/// </summary>
	private readonly IntHyperparameterValidator _epochsHyperparameter;

	/// <summary>
	/// Hyperparameter validator for the batch size hyperparameter.
	/// </summary>
	private readonly IntHyperparameterValidator _batchSizeHyperparameter;

	/// <summary>
	/// Hyperparameter validator for the number of epochs between saves.
	/// </summary>
	private readonly IntHyperparameterValidator _saveEpochsHyperparameter;

	/// <summary>
	/// Hyperparameter validator for the save path hyperparameter.
	/// </summary>
	private readonly StringHyperparameterValidator _savePathHyperparameter;

	/// <summary>
	/// Dictionary storing the value of each generic hyperparameter.
	/// Each key will be the unique ID of the hyperparameter and the value will
	///   be the stringified value of the hyperparameter.
	/// </summary>
	private readonly Dictionary<string, string?> _genericHyperparameterValues =
		new();

	/// <summary>
	/// Initializes the component.
	/// </summary>
	public TrainConfiguration()
	{
		_shuffleHyperparameter = new BoolHyperparameterValidator(
			PARAMETER_SHUFFLE,
			NAME_SHUFFLE,
			DESCRIPTION_SHUFFLE,
			defaultValue: true,
			isOptional: true
		);
		_optimizerTypeHyperparameter =
			new EnumHyperparameterValidator<EOptimizerType>(
			PARAMETER_OPTIMIZER_TYPE,
			NAME_OPTIMIZER_TYPE,
			DESCRIPTION_OPTIMIZER_TYPE,
			Enum.GetValues<EOptimizerType>()
				.Select(x => new Tuple<EOptimizerType, string>(x, x.ToString()))
				.ToList()
		);
		_lossTypeHyperparameter = new EnumHyperparameterValidator<ELossType>(
			PARAMETER_LOSS_TYPE,
			NAME_LOSS_TYPE,
			DESCRIPTION_LOSS_TYPE,
			new List<Tuple<ELossType, string>>()
			{
				new(ELossType.CrossEntropy, "Cross Entropy"),
			}
		);
		_learningRateHyperparameter = new FloatHyperparameterValidator(
			PARAMETER_LEARNING_RATE,
			NAME_LEARNING_RATE,
			DESCRIPTION_LEARNING_RATE,
			constraints: "Must be a positive number.",
			constraintFuncs: new List<Func<float, bool>>()
			{
				x => x > 0.0f,
			},
			defaultValue: 0.001f
		);
		_epochsHyperparameter = new IntHyperparameterValidator(
			PARAMETER_EPOCHS,
			NAME_EPOCHS,
			DESCRIPTION_EPOCHS,
			constraints: "Must be a positive number.",
			constraintFuncs: new List<Func<int, bool>>()
			{
				x => x > 0,
			},
			defaultValue: 10
		);
		_batchSizeHyperparameter = new IntHyperparameterValidator(
			PARAMETER_BATCH_SIZE,
			NAME_BATCH_SIZE,
			DESCRIPTION_BATCH_SIZE,
			constraints: "Must be a positive number.",
			constraintFuncs: new List<Func<int, bool>>()
			{
				x => x > 0,
			},
			defaultValue: 64
		);
		_saveEpochsHyperparameter = new IntHyperparameterValidator(
			PARAMETER_SAVE_INTERVAL,
			NAME_SAVE_INTERVAL,
			DESCRIPTION_SAVE_INTERVAL,
			constraints: "Must be a non-negative number.",
			constraintFuncs: new List<Func<int, bool>>()
			{
				x => x >= 0,
			},
			defaultValue: 0,
			isOptional: true
		);
		_savePathHyperparameter = new StringHyperparameterValidator(
			PARAMETER_SAVE_PATH,
			NAME_SAVE_PATH,
			DESCRIPTION_SAVE_PATH,
			constraints: "Must be a valid file path.",
			constraintFuncs: new List<Func<string, bool>>()
			{
				x => !string.IsNullOrWhiteSpace(x),
			},
			defaultValue: null,
			isOptional: true
		);

		_genericHyperparameters = new List<IHyperparameterValidator>()
		{
			_shuffleHyperparameter,
			_optimizerTypeHyperparameter,
			_lossTypeHyperparameter,
			_learningRateHyperparameter,
			_epochsHyperparameter,
			_batchSizeHyperparameter,
			_saveEpochsHyperparameter,
			_savePathHyperparameter,
		};
	}

	/// <summary>
	/// Whether the dataset is the currently selected dataset.
	/// </summary>
	/// <param name="dataset">Dataset to check.</param>
	/// <returns>
	/// Whether the dataset is the currently selected dataset.
	/// </returns>
	private bool IsSelectedDataset(IDataset dataset)
	{
		return _selectedDataset == dataset;
	}

	/// <summary>
	/// Whether the model is the currently selected model.
	/// </summary>
	/// <param name="model">Model to check.</param>
	/// <returns>
	/// Whether the model is the currently selected model.
	/// </returns>
	private bool IsSelectedModel(IClassificationModel model)
	{
		return _selectedModel == model;
	}

	/// <summary>
	/// Callback invoked when a dataset is selected.
	/// </summary>
	/// <param name="sender">Object that broadcast the event.</param>
	/// <param name="e">Arguments for the event.</param>
	private void OnDatasetSelected(
		object? sender,
		OnDatasetSelectedEventArgs e)
	{
		_selectedDataset = e.Dataset;
		StateHasChanged();
	}

	/// <summary>
	/// Callback invoked when a model is selected.
	/// </summary>
	/// <param name="sender">Object that broadcast the event.</param>
	/// <param name="e">Arguments for the event.</param>
	private void OnModelSelected(
		object? sender,
		OnModelSelectedEventArgs e)
	{
		_selectedModel = e.Model;
		StateHasChanged();
	}

	/// <summary>
	/// Callback invoked when the "Train" button is clicked.
	/// </summary>
	private void OnTrainClicked()
	{
		// TODO: Display an error message if any of these checks fail
		if (_selectedDataset == null)
		{
			return;
		}
		if (_selectedModel == null)
		{
			return;
		}

		// It shouldn't be possible for this to be null if `_selectedModel` is
		//   not null
		Debug.Assert(_modelSelector != null);

		// TODO: Validate the hyperparameters

		// Set up the hyperparameter objects
		var hyperparameters = new Hyperparameters()
		{
			Optimizer = OptimizerType,
			Loss = LossType,
			Dataset = _selectedDataset.CreateTrainingInstance(
				Shuffle
			),
			LearningRate = LearningRate,
			Epochs = Epochs,
			BatchSize = BatchSize,
			SaveInterval = SaveEpochs
		};
		var modelHyperparameters = _modelSelector.Hyperparameters;

		// Create the training session
		var trainingSession = TrainingService.TrainModel(
			_selectedModel.CreateInstance(
				hyperparameters.Dataset.InputSize,
				hyperparameters.Dataset.OutputSize,
				torch.CUDA,
				SavePath,
				modelHyperparameters
			),
			hyperparameters
		);
		NavigationManager.NavigateTo(
			$"/train/session/{trainingSession.SessionId}"
		);
	}
}
