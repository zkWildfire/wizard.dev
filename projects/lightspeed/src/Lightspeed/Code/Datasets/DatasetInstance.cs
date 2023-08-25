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
	/// Dataset that the instance was created by.
	IDataset Dataset { get; }

	/// Gets the data to use for training.
	/// @warning This object will be disposed of by the dataset instance.
	///   External code should not dispose of this object.
	IDatasetSlice TrainingSet { get; }

	/// Gets the data to use for validation.
	/// @warning This object will be disposed of by the dataset instance.
	///   External code should not dispose of this object.
	IDatasetSlice ValidationSet { get; }

	/// Gets the data to use for testing.
	/// @warning This object will be disposed of by the dataset instance.
	///   External code should not dispose of this object.
	IDatasetSlice TestSet { get; }
}
