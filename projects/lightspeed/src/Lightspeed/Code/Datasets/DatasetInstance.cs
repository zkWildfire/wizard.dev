/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using static TorchSharp.torch;
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
	/// Device that the dataset slice's data is on.
	/// </summary>
	Device Device { get; }

	/// <summary>
	/// Number of elements in the dataset instance.
	/// </summary>
	int Count { get; }

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

	/// <summary>
	/// Gets the dataset element at the given ID.
	/// Once a dataset instance has been created, the same element will always
	///   be returned for the same ID. Two different dataset instances created
	///   from the same dataset may return different elements for the same ID.
	/// </summary>
	/// <param name="id">The ID of the element to return.</param>
	/// <returns>A dataset element wrapping the target element.</returns>
	/// <exception cref="ArgumentOutOfRangeException">
	/// Thrown if the given ID is outside the range `[0, Count)`.
	/// </exception>
	/// <exception cref="InvalidOperationException">
	/// Thrown if the dataset is not downloaded.
	/// </exception>
	IDatasetElement this[int id] { get; }
}
