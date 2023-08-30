/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification;
using Lightspeed.Classification.Models;
using Lightspeed.Classification.Validators;
using Lightspeed.Components.Training;
using Lightspeed.Services.Datasets;
using Lightspeed.Services.Models;
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
	/// List of hyperparameter validators for generic hyperparameters.
	/// Hyperparameters are added to this list in the order they should appear
	///   on the UI.
	/// </summary>
	private readonly IReadOnlyList<IHyperparameterValidator>
		_genericHyperparameters;

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
	/// Initializes the component.
	/// </summary>
	public TrainConfiguration()
	{
		_optimizerTypeHyperparameter = new EnumHyperparameterValidator<EOptimizerType>(
			"optimizer",
			"Optimizer",
			"Optimizer to use for training the model.",
			Enum.GetValues<EOptimizerType>()
				.Select(x => new Tuple<EOptimizerType, string>(x, x.ToString()))
				.ToList()
		);
		_lossTypeHyperparameter = new EnumHyperparameterValidator<ELossType>(
			"loss",
			"Loss Function",
			"Loss function to use for training the model.",
			new List<Tuple<ELossType, string>>()
			{
				new(ELossType.CrossEntropy, "Cross Entropy"),
			}
		);
		_learningRateHyperparameter = new FloatHyperparameterValidator(
			"learning_rate",
			"Learning Rate",
			"Learning rate to use for training the model.",
			constraints: "Must be a positive number.",
			constraintFuncs: new List<Func<float, bool>>()
			{
				x => x > 0.0f,
			},
			defaultValue: 0.001f
		);
		_epochsHyperparameter = new IntHyperparameterValidator(
			"epochs",
			"Epochs",
			"Number of epochs to train the model for.",
			constraints: "Must be a positive number.",
			constraintFuncs: new List<Func<int, bool>>()
			{
				x => x > 0,
			},
			defaultValue: 10
		);
		_batchSizeHyperparameter = new IntHyperparameterValidator(
			"batch_size",
			"Batch Size",
			"Size to use for each batch of data.",
			constraints: "Must be a positive number.",
			constraintFuncs: new List<Func<int, bool>>()
			{
				x => x > 0,
			},
			defaultValue: 64
		);
		_saveEpochsHyperparameter = new IntHyperparameterValidator(
			"save_epochs",
			"Save Epochs",
			"Number of epochs between saves of the model data. If this is `0`, the model data will not be saved during training.",
			constraints: "Must be a non-negative number.",
			constraintFuncs: new List<Func<int, bool>>()
			{
				x => x >= 0,
			},
			defaultValue: 0,
			isOptional: true
		);
		_savePathHyperparameter = new StringHyperparameterValidator(
			"save_path",
			"Save Path",
			"Path to save the model data to. Relative paths will be " +
			"interpreted relative to the server's current working directory. " +
			"If the path already exists and the saved data is compatible with " +
			"the selected model, the model will be initialized with the saved " +
			"data. If this is not specified, the model data will not be saved " +
			"during training.",
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
}
