/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Datasets.Generic;
using TorchSharp;
using static TorchSharp.torch.utils.data;
namespace Lightspeed.Datasets.Mnist;

/// Dataset implementation that provides access to the MNIST dataset.
public sealed class MnistDataset : IDataset
{
	/// <summary>
	/// Name to display for the dataset on the UI.
	/// </summary>
	public string DisplayName { get; }

	/// <summary>
	/// Gets the number of elements in the dataset.
	/// If the dataset hasn't been downloaded yet, this will be -1.
	/// </summary>
	public int Count => _data.Count == 0 ? -1 : _data.Count;

	/// <summary>
	/// Path to the folder on disk containing the dataset's files.
	/// If the dataset hasn't been downloaded yet, this path may not exist.
	/// </summary>
	public string SaveLocation { get; }

	/// <summary>
	/// Whether the dataset has been downloaded.
	/// </summary>
	/// <remarks>
	/// If either the training or test dataset is not null, the other dataset
	///   should also be not-null.
	/// </remarks>
	public bool IsDownloaded => _trainDataset != null && _testDataset != null;

	/// <summary>
	/// Gets the size of the dataset on disk in bytes.
	/// </summary>
	public long SizeOnDiskBytes { get; private set; }

	/// <summary>
	/// Gets the size of the dataset on disk in kilobytes.
	/// </summary>
	public double SizeOnDiskKB => SizeOnDiskBytes / 1024f;

	/// <summary>
	/// Gets the size of the dataset on disk in megabytes.
	/// </summary>
	public double SizeOnDiskMB => SizeOnDiskKB / 1024f;

	/// <summary>
	/// Gets the size of the dataset on disk in gigabytes.
	/// </summary>
	public double SizeOnDiskGB => SizeOnDiskMB / 1024f;

	/// <summary>
	/// Mean value for the MNIST dataset.
	/// </summary>
	private const double MNIST_MEAN = 0.1307;

	/// <summary>
	/// Standard deviation value for the MNIST dataset.
	/// </summary>
	private const double MNIST_STDDEV = 0.3081;

	/// <summary>
	/// Training dataset for the MNIST dataset.
	/// </summary>
	private Dataset? _trainDataset;

	/// <summary>
	/// Test dataset for the MNIST dataset.
	/// </summary>
	private Dataset? _testDataset;

	/// <summary>
	/// List of all tensors in the slice.
	/// </summary>
	private IReadOnlyList<torch.Tensor> _data;

	/// <summary>
	/// Whether the dataset has been disposed.
	/// </summary>
	private bool _isDisposed;

	/// <summary>
	/// Initializes the dataset.
	/// </summary>
	/// <param name="saveLocation">
	/// Path to the folder on disk that the dataset's files should be saved to.
	/// </param>
	/// <param name="displayName">
	/// Name to display on the UI for the dataset.
	/// </param>
	public MnistDataset(
		string saveLocation,
		string displayName = "MNIST")
	{
		DisplayName = displayName;
		SaveLocation = saveLocation;
		_data = new List<torch.Tensor>();

		// If the dataset is downloaded, load the dataset from disk
		if (Directory.Exists(SaveLocation))
		{
			// Load the dataset from disk
			_trainDataset = torchvision.datasets.MNIST(
				SaveLocation,
				train: true,
				download: false
			);
			_testDataset = torchvision.datasets.MNIST(
				SaveLocation,
				train: false,
				download: false
			);

			LoadData();
		}
	}

	/// <summary>
	/// Disposes of the dataset.
	/// </summary>
	public void Dispose()
	{
		if (_isDisposed)
		{
			return;
		}

		_trainDataset?.Dispose();
		_testDataset?.Dispose();

		GC.SuppressFinalize(this);
		_isDisposed = true;
	}

	/// <summary>
	/// Gets the dataset element at the given ID.
	/// Datasets should always assign the same ID to each element no matter
	///   how many times the dataset is loaded and unloaded from memory. This
	///   ensures that the UI is "stable", e.g. the same element is always
	///   displayed at the same position in the UI.
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
			if (!IsDownloaded)
			{
				throw new InvalidOperationException(
					"Cannot get dataset element from a dataset that is " +
					"not downloaded."
				);
			}
			else if (id < 0 || id >= _data.Count)
			{
				throw new ArgumentOutOfRangeException(
					nameof(id),
					id,
					$"ID must be in the range [0, {Count})."
				);
			}

			return new GrayscaleImageDatasetElement(
				this,
				id,
				_data[id]
			);
		}
	}

	/// <summary>
	/// Gets an enumerator for the dataset.
	/// </summary>
	/// <exception cref="InvalidOperationException">
	/// Thrown if the dataset is not downloaded.
	/// </exception>
	/// <returns>An enumerator for the dataset.</returns>
	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	/// <summary>
	/// Gets an enumerator for the dataset.
	/// </summary>
	/// <exception cref="InvalidOperationException">
	/// If the dataset is not downloaded.
	/// </exception>
	/// <returns>An enumerator for the dataset.</returns>
	public IEnumerator<IDatasetElement> GetEnumerator()
	{
		if (!IsDownloaded)
		{
			throw new InvalidOperationException(
				"Cannot get enumerator for a dataset that is not " +
				"downloaded."
			);
		}

		Debug.Assert(_trainDataset != null);
		Debug.Assert(_testDataset != null);

		for (var i = 0; i < _data.Count; i++)
		{
			yield return new GrayscaleImageDatasetElement(
				this,
				i,
				_data[i]
			);
		}
	}

	/// <summary>
	/// Downloads the dataset to disk.
	/// If the dataset is already downloaded, this method will be a no-op.
	/// </summary>
	public void Download()
	{
		// If the dataset is already downloaded, do nothing
		if (IsDownloaded)
		{
			return;
		}

		// Download and normalize the dataset
		var normImage = torchvision.transforms.Normalize(
			new double[] { MNIST_MEAN },
			new double[] { MNIST_STDDEV }
		);
		_trainDataset = torchvision.datasets.MNIST(
			SaveLocation,
			train: true,
			download: true,
			target_transform: normImage
		);
		_testDataset = torchvision.datasets.MNIST(
			SaveLocation,
			train: false,
			download: true,
			target_transform: normImage
		);

		LoadData();
	}

	/// <summary>
	/// Downloads the dataset to disk.
	/// If the dataset is already downloaded, this method will be a no-op.
	/// </summary>
	/// <returns>A task set once the download has completed.</returns>
	public async Task DownloadAsync()
	{
		await Task.Run(Download)
			.ConfigureAwait(false);
	}

	/// <summary>
	/// Creates a new instance of the dataset.
	/// This will create a dataset instance where the dataset elements are
	///   divided between training, validation, and test sets.
	/// </summary>
	/// <param name="shuffle">
	/// Whether to shuffle the dataset elements before creating the training,
	///   validation, and test sets.
	/// </param>
	/// <returns>A new dataset instance.</returns>
	public IDatasetInstance CreateTrainingInstance(bool shuffle = true)
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// Initializes class members using the dataset's data.
	/// This may only be called once `_trainDataset` and `_testDataset` have
	///   been initialized.
	/// </summary>
	private void LoadData()
	{
		Debug.Assert(_trainDataset != null);
		Debug.Assert(_testDataset != null);
		var tensors = new List<torch.Tensor>();

		// Add the tensors from each dataset into a single list
		for (var i = 0; i < _trainDataset.Count; i++)
		{
			var dict = _trainDataset.GetTensor(i);
			foreach (var (_, tensor) in dict)
			{
				tensors.Add(tensor);
			}
		}

		for (var i = 0; i < _testDataset.Count; i++)
		{
			var dict = _testDataset.GetTensor(i);
			foreach (var (_, tensor) in dict)
			{
				tensors.Add(tensor);
			}
		}

		_data = tensors;

		// Calculate and save the size of the dataset on disk
		SizeOnDiskBytes = Directory.GetFiles(
			SaveLocation,
			"*",
			SearchOption.AllDirectories)
			.Sum(path => new FileInfo(path).Length);
	}
}
