/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Lightspeed.Classification.Models;

/// <summary>
/// Defines constants related to generic hyperparameters.
/// </summary>
public static class GenericHyperparameterStatics
{
	/// <summary>
	/// ID used for the shuffle hyperparameter.
	/// </summary>
	public const string PARAMETER_SHUFFLE = "shuffle";

	/// <summary>
	/// Display name for the shuffle hyperparameter.
	/// </summary>
	public const string NAME_SHUFFLE = "Shuffle";

	/// <summary>
	/// Description for the shuffle hyperparameter.
	/// </summary>
	public const string DESCRIPTION_SHUFFLE =
		"Whether or not to shuffle the training data before each epoch.";

	/// <summary>
	/// ID used for the optimizer type hyperparameter.
	/// </summary>
	public const string PARAMETER_OPTIMIZER_TYPE = "optimizer";

	/// <summary>
	/// Display name for the optimizer type hyperparameter.
	/// </summary>
	public const string NAME_OPTIMIZER_TYPE = "Optimizer Type";

	/// <summary>
	/// Description for the optimizer type hyperparameter.
	/// </summary>
	public const string DESCRIPTION_OPTIMIZER_TYPE =
		"Optimizer to use when training the model.";

	/// <summary>
	/// ID used for the loss type hyperparameter.
	/// </summary>
	public const string PARAMETER_LOSS_TYPE = "loss";

	/// <summary>
	/// Display name for the loss type hyperparameter.
	/// </summary>
	public const string NAME_LOSS_TYPE = "Loss Type";

	/// <summary>
	/// Description for the loss type hyperparameter.
	/// </summary>
	public const string DESCRIPTION_LOSS_TYPE =
		"Loss function to use when training the model.";

	/// <summary>
	/// ID used for the learning rate hyperparameter.
	/// </summary>
	public const string PARAMETER_LEARNING_RATE = "learning_rate";

	/// <summary>
	/// Display name for the learning rate hyperparameter.
	/// </summary>
	public const string NAME_LEARNING_RATE = "Learning Rate";

	/// <summary>
	/// Description for the learning rate hyperparameter.
	/// </summary>
	public const string DESCRIPTION_LEARNING_RATE =
		"Learning rate to use when training the model.";

	/// <summary>
	/// ID used for the training data loader hyperparameter.
	/// </summary>
	public const string PARAMETER_EPOCHS = "epochs";

	/// <summary>
	/// Display name for the training data loader hyperparameter.
	/// </summary>
	public const string NAME_EPOCHS = "Epochs";

	/// <summary>
	/// Description for the training data loader hyperparameter.
	/// </summary>
	public const string DESCRIPTION_EPOCHS =
		"Number of epochs to train the model for.";

	/// <summary>
	/// ID used for the training data loader hyperparameter.
	/// </summary>
	public const string PARAMETER_BATCH_SIZE = "batch_size";

	/// <summary>
	/// Display name for the training data loader hyperparameter.
	/// </summary>
	public const string NAME_BATCH_SIZE = "Batch Size";

	/// <summary>
	/// Description for the training data loader hyperparameter.
	/// </summary>
	public const string DESCRIPTION_BATCH_SIZE =
		"Size to use for each batch of data.";

	/// <summary>
	/// ID used for the training data loader hyperparameter.
	/// </summary>
	public const string PARAMETER_SAVE_INTERVAL = "save_interval";

	/// <summary>
	/// Display name for the training data loader hyperparameter.
	/// </summary>
	public const string NAME_SAVE_INTERVAL = "Save Interval";

	/// <summary>
	/// Description for the training data loader hyperparameter.
	/// </summary>
	public const string DESCRIPTION_SAVE_INTERVAL =
		"Number of epochs between saves of the model data. If this is 0, the " +
		"model data will not be saved during training.";

	/// <summary>
	/// ID used for the training data loader hyperparameter.
	/// </summary>
	public const string PARAMETER_SAVE_PATH = "save_path";

	/// <summary>
	/// Display name for the training data loader hyperparameter.
	/// </summary>
	public const string NAME_SAVE_PATH = "Save Path";

	/// <summary>
	/// Description for the training data loader hyperparameter.
	/// </summary>
	public const string DESCRIPTION_SAVE_PATH =
		"Path to save the model data to. Relative paths will be " +
		"interpreted relative to the server's current working directory. " +
		"If the path already exists and the saved data is compatible with " +
		"the selected model, the model will be initialized with the saved " +
		"data. If this is not specified, the model data will not be saved " +
		"during training.";
}
