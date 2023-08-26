/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using static TorchSharp.torch.utils.data;
namespace Lightspeed.Datasets;

/// Dataset slice implementation that works with most dataset types.
public class GenericDatasetSlice : IDatasetSlice
{
	/// <summary>
	/// Dataset that the slice is from.
	/// </summary>
	public IDataset Dataset { get; }

	/// <summary>
	/// Dataset instance that the slice is from.
	/// </summary>
	public IDatasetInstance DatasetInstance { get; }

	/// <summary>
	/// Number of elements in the slice.
	/// </summary>
	public long Count => Data.Count;

	/// <summary>
	/// Gets the Torch dataloader object for the slice.
	/// </summary>
	public DataLoader Data { get; }

	/// <summary>
	/// Gets the dataset element at the given ID.
	/// Two dataset slices that were created the same way must assign the same
	///   ID to each element. Dataset slices that were created differently may
	///   assign different IDs to the same element.
	/// </summary>
	/// <param name="id">ID of the element to return.</param>
	/// <returns>
	/// A dataset element instance wrapping the target element.
	/// </returns>
	public IDatasetElement this[int id] => null!;

	/// <summary>
	/// Initializes the slice.
	/// </summary>
	/// <param name="parentDataset">Dataset that the slice is from.</param>
	/// <param name="parentDatasetInstance">
	/// Dataset instance that the slice is from.
	/// </param>
	/// <param name="dataLoader">Torch dataloader object for the slice.</param>
	public GenericDatasetSlice(
		IDataset parentDataset,
		IDatasetInstance parentDatasetInstance,
		DataLoader dataLoader)
	{
		Dataset = parentDataset;
		DatasetInstance = parentDatasetInstance;
		Data = dataLoader;

		// Count up the number of elements in the slice
		long count = 0;
		foreach (var d in Data)
		{
			count += d.Count;
		}
	}
}
