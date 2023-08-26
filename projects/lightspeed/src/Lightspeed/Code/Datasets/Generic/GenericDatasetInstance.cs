/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using static TorchSharp.torch;

namespace Lightspeed.Datasets.Generic;

/// <summary>
/// Generic implementation of the `IDatasetInstance` interface.
/// </summary>
public sealed class GenericDatasetInstance : IDatasetInstance
{
	/// <summary>
	/// Dataset that the instance was created by.
	/// </summary>
	public IDataset Dataset { get; }

	/// <summary>
	/// Device that the dataset slice's data is on.
	/// </summary>
	public Device Device { get; }

	/// <summary>
	/// Number of elements in the dataset instance.
	/// </summary>
	public int Count => TrainingSet.Count + ValidationSet.Count + TestSet.Count;

	/// <summary>
	/// Gets the data to use for training.
	/// </summary>
	/// <remarks>
	/// This object will be disposed of by the dataset instance.
	///   External code should not dispose of this object.
	/// </remarks>
	public IDatasetSlice TrainingSet { get; }

	/// <summary>
	/// Gets the data to use for validation.
	/// </summary>
	/// <remarks>
	/// @warning This object will be disposed of by the dataset instance.
	///   External code should not dispose of this object.
	/// </remarks>
	public IDatasetSlice ValidationSet { get; }

	/// <summary>
	/// Gets the data to use for testing.
	/// </summary>
	/// <remarks>
	/// @warning This object will be disposed of by the dataset instance.
	///   External code should not dispose of this object.
	/// </remarks>
	public IDatasetSlice TestSet { get; }

	/// <summary>
	/// Whether the dataset instance has been disposed of.
	/// </summary>
	private bool _isDisposed;

	/// <summary>
	/// Initializes the dataset instance.
	/// </summary>
	/// <param name="dataset">Dataset that the instance was created by.</param>
	/// <param name="device">Device to place the dataset slice's data on.</param>
	/// <param name="trainingSet">Slice to use for training.</param>
	/// <param name="validationSet">Slice to use for validation.</param>
	/// <param name="testSet">Slice to use for testing.</param>
	public GenericDatasetInstance(
		IDataset dataset,
		Device device,
		IDatasetSlice trainingSet,
		IDatasetSlice validationSet,
		IDatasetSlice testSet)
	{
		Dataset = dataset;
		Device = device;
		TrainingSet = trainingSet;
		ValidationSet = validationSet;
		TestSet = testSet;
	}

	/// <summary>
	/// Disposes of the dataset instance.
	/// </summary>
	public void Dispose()
	{
		if (_isDisposed)
		{
			return;
		}

		TrainingSet.Dispose();
		ValidationSet.Dispose();
		TestSet.Dispose();
		GC.SuppressFinalize(this);
		_isDisposed = true;
	}

	/// <summary>
	/// Gets an enumerator for the dataset instance.
	/// </summary>
	/// <returns>An enumerator for the dataset instance.</returns>
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	/// <summary>
	/// Gets an enumerator for the dataset instance.
	/// </summary>
	/// <returns>An enumerator for the dataset instance.</returns>
	public IEnumerator<IDatasetElement> GetEnumerator()
	{
		foreach (var element in TrainingSet)
		{
			yield return element;
		}
		foreach (var element in ValidationSet)
		{
			yield return element;
		}
		foreach (var element in TestSet)
		{
			yield return element;
		}
	}

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
	public IDatasetElement this[int id]
	{
		get
		{
			return id switch
			{
				_ when id < 0 || id >= Count =>
					throw new ArgumentOutOfRangeException(
						nameof(id),
						id,
						$"ID must be in the range [0, {Count})."
					),
				_ when id < TrainingSet.Count => TrainingSet[id],
				_ when id < TrainingSet.Count + ValidationSet.Count =>
					ValidationSet[id - TrainingSet.Count],
				_ => TestSet[id - TrainingSet.Count - ValidationSet.Count]
			};
		}
	}
}
