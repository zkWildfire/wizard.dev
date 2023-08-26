/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using static TorchSharp.torch;
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
	/// Device that the dataset slice's data is on.
	/// </summary>
	public Device Device => DatasetInstance.Device;

	/// <summary>
	/// Number of elements in the slice.
	/// </summary>
	public int Count => _data.Count;

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

	/// Dataset to get slice data from.
	private readonly Dataset _dataset;

	/// <summary>
	/// List of all tensors in the slice.
	/// </summary>
	private readonly IReadOnlyList<Tensor> _data;

	/// <summary>
	/// Method to invoke to convert tensors from the slice into dataset
	///   elements.
	/// </summary>
	private readonly Func<
		IDatasetSlice,
		long,
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
	/// <param name="parentDataset">Dataset that the slice is from.</param>
	/// <param name="parentDatasetInstance">
	/// Dataset instance that the slice is from.
	/// </param>
	/// <param name="dataset">Torch dataset object for the slice.</param>
	/// <param name="elementFactory">
	/// Method to invoke to convert tensors from the slice into dataset
	///   elements. The method will be passed the slice instance invoking the
	///   method, the ID of the element, and the tensor containing the element
	///   data.
	/// </param>
	public GenericDatasetSlice(
		IDataset parentDataset,
		IDatasetInstance parentDatasetInstance,
		Dataset dataset,
		Func<IDatasetSlice, long, Tensor, IDatasetElement> elementFactory)
	{
		Dataset = parentDataset;
		DatasetInstance = parentDatasetInstance;
		_dataset = dataset;
		_elementFactory = elementFactory;

		// Add all tensors into a single list for quick access
		var tensors = new List<Tensor>();
		for (var i = 0; i < dataset.Count; i++)
		{
			var dict = dataset.GetTensor(i);
			foreach (var (_, tensor) in dict)
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

		_dataset.Dispose();
		GC.SuppressFinalize(this);
		_isDisposed = true;
	}

	/// <summary>
	/// Creates a new dataloader instance for the slice's data.
	/// </summary>
	/// <param name="batchSize">Number of elements per batch.</param>
	/// <param name="shuffle">Whether to shuffle the data.</param>
	/// <returns>A new dataloader instance.</returns>
	public DataLoader GetDataLoader(
		int batchSize,
		bool shuffle)
	{
		return new DataLoader(
			_dataset,
			batchSize,
			device: Device,
			shuffle: shuffle
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
