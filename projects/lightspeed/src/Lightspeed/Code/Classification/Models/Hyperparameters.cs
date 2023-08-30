/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Lightspeed.Classification.Models;

/// <summary>
/// Generic struct-like class used for passing hyperparameters to models.
/// Models may implement their own hyperparameter types that inherit from this
///   type as necessary.
/// </summary>
/// <remarks>
/// This class stretches the term "hyperparameter" somewhat from its traditional
///   meaning. In this case, a hyperparameter is any value or object that will
///   remain constant for the entire duration of a model's training. This
///   includes things like the learning rate, but also includes things like the
///   optimizer and loss function objects.
/// </remarks>
public record class Hyperparameters
{
	/// <summary>
	/// Type of optimizer to use when training the model.
	/// </summary>
	public required EOptimizerType Optimizer { get; init; }

	/// <summary>
	/// Type of loss function to use when training the model.
	/// </summary>
	public required ELossType Loss { get; init; }

	/// <summary>
	/// Dataset to use when training.
	/// </summary>
	public required IDatasetInstance Dataset { get; init; }

	/// <summary>
	/// Learning rate to use for training the model.
	/// </summary>
	public required float LearningRate { get; init; }

	/// <summary>
	/// Number of epochs to train the model for.
	/// </summary>
	public required int Epochs { get; init; }

	/// <summary>
	/// Size to use for each batch of data.
	/// </summary>
	public required int BatchSize { get; init; }

	/// <summary>
	/// Number of epochs between saves of the model data. If this is `0`, the
	///   model data will not be saved during training.
	/// </summary>
	public required int SaveInterval { get; init; }
}
