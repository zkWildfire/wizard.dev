/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using static TorchSharp.torch.utils.data;
namespace Lightspeed.Datasets;

/// Represents a subset of elements in a dataset.
public interface IDatasetSlice : IDisposable, IEnumerable<IDatasetElement>
{
	/// Dataset that the slice is from.
	IDataset Dataset { get; }

	/// Dataset instance that the slice is from.
	IDatasetInstance DatasetInstance { get; }

	/// Number of elements in the slice.
	long Count { get; }

	/// Gets the Torch dataloader object for the slice.
	DataLoader Data { get; }

	/// Gets the dataset element at the given ID.
	/// Two dataset slices that were created the same way must assign the same
	///   ID to each element. Dataset slices that were created differently may
	///   assign different IDs to the same element.
	IDatasetElement this[int id] { get; }
}
