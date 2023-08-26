/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using static TorchSharp.torch.utils.data;
namespace Lightspeed.Datasets;

/// Represents a subset of elements in a dataset.
public interface IDatasetSlice : IDisposable, IEnumerable<IDatasetElement>
{
	/// <summary>
	/// Dataset that the slice is from.
	/// </summary>
	IDataset Dataset { get; }

	/// <summary>
	/// Dataset instance that the slice is from.
	/// </summary>
	IDatasetInstance DatasetInstance { get; }

	/// <summary>
	/// Number of elements in the slice.
	/// </summary>
	long Count { get; }

	/// <summary>
	/// Gets the Torch dataloader object for the slice.
	/// </summary>
	DataLoader Data { get; }

	/// <summary>
	/// Gets the dataset element at the given ID.
	/// Two dataset slices that were created the same way must assign the same
	///   ID to each element. Dataset slices that were created differently may
	///   assign different IDs to the same element.
	/// </summary>
	IDatasetElement this[int id] { get; }
}
