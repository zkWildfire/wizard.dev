/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Lightspeed.Datasets;

/// Represents a transient instance of a dataset.
/// A dataset instance is used to keep track of metadata for a dataset that
///   needs to persist for some time, but is not inherently part of the dataset.
///   For example, a dataset instance can be created to split the dataset
///   elements into a training, validation, and test set. This split needs to
///   persist until all model training and evaluation is complete, but it is
///   not part of the dataset itself.
public interface IDatasetInstance : IDisposable, IEnumerable<IDatasetElement>
{
	/// <summary>
	/// Dataset that the instance was created by.
	/// </summary>
	IDataset Dataset { get; }

	/// <summary>
	/// Gets the data to use for training.
	/// </summary>
	/// <remarks>
	/// This object will be disposed of by the dataset instance.
	///   External code should not dispose of this object.
	/// </remarks>
	IDatasetSlice TrainingSet { get; }

	/// <summary>
	/// Gets the data to use for validation.
	/// </summary>
	/// <remarks>
	/// @warning This object will be disposed of by the dataset instance.
	///   External code should not dispose of this object.
	/// </remarks>
	IDatasetSlice ValidationSet { get; }

	/// <summary>
	/// Gets the data to use for testing.
	/// </summary>
	/// <remarks>
	/// @warning This object will be disposed of by the dataset instance.
	///   External code should not dispose of this object.
	/// </remarks>
	IDatasetSlice TestSet { get; }
}
