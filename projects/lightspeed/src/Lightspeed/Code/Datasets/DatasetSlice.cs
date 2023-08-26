/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using static TorchSharp.torch;
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
	/// Device that the dataset slice's data is on.
	/// </summary>
	Device Device { get; }

	/// <summary>
	/// Number of elements in the slice.
	/// </summary>
	int Count { get; }

	/// <summary>
	/// Gets the dataset element at the given ID.
	/// Two dataset slices that were created the same way must assign the same
	///   ID to each element. Dataset slices that were created differently may
	///   assign different IDs to the same element.
	/// </summary>
	IDatasetElement this[int id] { get; }

	/// <summary>
	/// Creates a new dataloader instance for the slice's data.
	/// </summary>
	/// <param name="batchSize">Number of elements per batch.</param>
	/// <param name="shuffle">Whether to shuffle the data.</param>
	/// <returns>A new dataloader instance.</returns>
	DataLoader GetDataLoader(int batchSize, bool shuffle);
}
