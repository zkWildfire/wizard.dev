/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using TorchSharp;
using static TorchSharp.torch.utils.data;
namespace Lightspeed.Datasets;

/// Dataset slice implementation that works with most dataset types.
public sealed class GenericDatasetSlice : IDatasetSlice
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
	public long Count => _data.Count;

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
	public IDatasetElement this[int id]
	{
		get
		{
			Debug.Assert(!_isDisposed);
			var tensor = _data[id];
			return _elementFactory(this, id, tensor);
		}
	}

	/// <summary>
	/// List of all tensors in the slice.
	/// </summary>
	private readonly IReadOnlyList<torch.Tensor> _data;

	/// <summary>
	/// Method to invoke to convert tensors from the slice into dataset
	///   elements.
	/// </summary>
	private readonly Func<
		IDatasetSlice,
		long,
		torch.Tensor,
		IDatasetElement
	> _elementFactory;

	/// <summary>
	/// Whether the slice has been disposed.
	/// </summary>
	private bool _isDisposed;

	/// <summary>
	/// Initializes the slice.
	/// </summary>
	/// <param name="parentDataset">Dataset that the slice is from.</param>
	/// <param name="parentDatasetInstance">
	/// Dataset instance that the slice is from.
	/// </param>
	/// <param name="dataLoader">Torch dataloader object for the slice.</param>
	/// <param name="elementFactory">
	/// Method to invoke to convert tensors from the slice into dataset
	///   elements. The method will be passed the slice instance invoking the
	///   method, the ID of the element, and the tensor containing the element
	///   data.
	/// </param>
	public GenericDatasetSlice(
		IDataset parentDataset,
		IDatasetInstance parentDatasetInstance,
		DataLoader dataLoader,
		Func<IDatasetSlice, long, torch.Tensor, IDatasetElement> elementFactory)
	{
		Dataset = parentDataset;
		DatasetInstance = parentDatasetInstance;
		Data = dataLoader;
		_elementFactory = elementFactory;

		// Add all tensors into a single list for quick access
		var tensors = new List<torch.Tensor>();
		foreach (var batch in dataLoader)
		{
			foreach (var (_, tensor) in batch)
			{
				tensors.Add(tensor);
			}
		}
		_data = tensors;
	}

	/// <summary>
	/// Disposes the slice.
	/// </summary>
	public void Dispose()
	{
		if (_isDisposed)
		{
			return;
		}

		Data.Dispose();
		GC.SuppressFinalize(this);
		_isDisposed = true;
	}

	/// <summary>
	/// Gets an enumerator for the slice.
	/// </summary>
	/// <returns>An enumerator for the slice.</returns>
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	/// <summary>
	/// Gets an enumerator for the slice.
	/// </summary>
	/// <returns>An enumerator for the slice.</returns>
	public IEnumerator<IDatasetElement> GetEnumerator()
	{
		for (var i = 0; i < Count; i++)
		{
			Debug.Assert(!_isDisposed);
			yield return this[i];
		}
	}
}
