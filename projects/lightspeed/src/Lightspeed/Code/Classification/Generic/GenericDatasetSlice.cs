/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using static TorchSharp.torch;
using static TorchSharp.torch.utils.data;
namespace Lightspeed.Classification.Generic;

/// Dataset slice implementation that works with most dataset types.
public sealed class GenericDatasetSlice : IDatasetSlice
{
	/// <summary>
	/// Dataset that the slice is from.
	/// </summary>
	public IDataset Dataset { get; }

	/// <summary>
	/// Device that the dataset slice's data is on.
	/// </summary>
	public Device Device { get; }

	/// <summary>
	/// Number of elements in the slice.
	/// </summary>
	public int Count => (int)_dataset.Count;

	/// <summary>
	/// Whether data should be shuffled when getting data from the dataset.
	/// </summary>
	public bool Shuffle { get; }

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
			var data = _dataset.GetTensor(id);
			// TODO: Get these in a more generic way
			var input = data["data"];
			var label = data["label"];
			return _elementFactory(this, id, input, label);
		}
	}

	/// Dataset to get slice data from.
	private readonly Dataset _dataset;

	/// <summary>
	/// Method to invoke to convert tensors from the slice into dataset
	///   elements. The method will be passed the slice instance invoking the
	///   method, the ID of the element, and the tensors containing input data
	///   and label data, respectively.
	/// </summary>
	private readonly Func<
		IDatasetSlice,
		long,
		Tensor,
		Tensor,
		IDatasetElement
	> _elementFactory;

	/// <summary>
	/// Whether the slice has been disposed.
	/// </summary>
	private bool _isDisposed;

	/// <summary>
	/// Initializes the slice.
	/// </summary>
	/// <param name="parentDataset">
	/// Dataset that the slice is from.
	/// </param>
	/// <param name="device">
	/// Device that the dataset data is on.
	/// </param>
	/// <param name="dataset">
	/// Torch dataset object for the slice.
	/// </param>
	/// <param name="elementFactory">
	/// Method to invoke to convert tensors from the slice into dataset
	///   elements. The method will be passed the slice instance invoking the
	///   method, the ID of the element, and the tensors containing input data
	///   and label data, respectively.
	/// </param>
	/// <param name="shuffle">
	/// Whether data should be shuffled when getting data from the dataset.
	/// </param>
	public GenericDatasetSlice(
		IDataset parentDataset,
		Device device,
		Dataset dataset,
		Func<IDatasetSlice, long, Tensor, Tensor, IDatasetElement> elementFactory,
		bool shuffle)
	{
		Dataset = parentDataset;
		Device = device;
		Shuffle = shuffle;
		_dataset = dataset;
		_elementFactory = elementFactory;
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

		_dataset.Dispose();
		GC.SuppressFinalize(this);
		_isDisposed = true;
	}

	/// <summary>
	/// Creates a new dataloader instance for the slice's data.
	/// </summary>
	/// <param name="batchSize">Number of elements per batch.</param>
	/// <returns>A new dataloader instance.</returns>
	public DataLoader GetDataLoader(
		int batchSize)
	{
		return new DataLoader(
			_dataset,
			batchSize,
			device: Device,
			shuffle: Shuffle
		);
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
