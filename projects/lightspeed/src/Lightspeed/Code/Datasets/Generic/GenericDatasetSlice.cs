/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using static TorchSharp.torch.utils.data;
namespace Lightspeed.Datasets;

/// Dataset slice implementation that works with most dataset types.
public class GenericDatasetSlice : IDatasetSlice
{
	/// Dataset that the slice is from.
	public IDataset Dataset { get; }

	/// Dataset instance that the slice is from.
	public IDatasetInstance DatasetInstance { get; }

	/// Number of elements in the slice.
	public long Count => Data.Count;

	/// Gets the Torch dataloader object for the slice.
	public DataLoader Data { get; }

	/// Gets the dataset element at the given ID.
	/// Two dataset slices that were created the same way must assign the same
	///   ID to each element. Dataset slices that were created differently may
	///   assign different IDs to the same element.
	public IDatasetElement this[int id] => null!;

	/// Initializes the slice.
	/// @param parentDataset Dataset that the slice is from.
	/// @param parentDatasetInstance Dataset instance that the slice is from.
	/// @param dataLoader Torch dataloader object for the slice.
	public GenericDatasetSlice(
		IDataset parentDataset,
		IDatasetInstance parentDatasetInstance,
		DataLoader dataLoader)
	{
		Dataset = parentDataset;
		DatasetInstance = parentDatasetInstance;
		Data = dataLoader;
	}
}
