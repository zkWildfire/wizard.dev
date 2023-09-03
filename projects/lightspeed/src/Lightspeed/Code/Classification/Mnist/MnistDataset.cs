/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Generic;
using TorchSharp;
using static TorchSharp.torch;
using static TorchSharp.torch.utils.data;
namespace Lightspeed.Classification.Mnist;

/// Dataset implementation that provides access to the MNIST dataset.
public sealed class MnistDataset : IDataset
{
	/// <summary>
	/// Lightspeed internal unique ID for the dataset.
	/// </summary>
	public const string DATASET_ID = "mnist";

	/// <summary>
	/// Name to display for the dataset on the UI.
	/// </summary>
	public const string DISPLAY_NAME = "MNIST";

	/// <summary>
	/// Brief description of the dataset.
	/// </summary>
	public const string BRIEF_DESCRIPTION =
		"MNIST dataset of handwritten digits.";

	/// <summary>
	/// Detailed description of the dataset.
	/// </summary>
	public const string DETAILED_DESCRIPTION =
		"A large database of handwritten digits created " +
		"by the National Institute of Standards and Technology. " +
		"The dataset consists of 60,000 training images and " +
		"10,000 testing images.";

	/// <summary>
	/// Path to the dataset's icon.
	/// </summary>
	public static readonly Uri DATASET_ICON = new(
		$"/img/datasets/{DATASET_ID}.png",
		UriKind.Relative
	);

	/// <summary>
	/// Event broadcast to when the dataset has been downloaded.
	/// The sender of the event will be the dataset object for which the
	///   underlying data has been downloaded.
	/// </summary>
	/// <remarks>
	/// If the dataset was previously downloaded, e.g. `IsDownloaded` is true
	///   immediately after the constructor finishes executing, this event will
	///   never be broadcast to.
	/// </remarks>
	public event EventHandler? OnDownloaded;

	/// <summary>
	/// Lightspeed internal unique ID for the dataset.
	/// </summary>
	public string Id => DATASET_ID;

	/// <summary>
	/// Name to display for the dataset on the UI.
	/// </summary>
	public string DisplayName => DISPLAY_NAME;

	/// <summary>
	/// Path to the image to display as the dataset's icon.
	/// </summary>
	public Uri IconPath => DATASET_ICON;

	/// <summary>
	/// Brief description of the dataset.
	/// </summary>
	public string BriefDescription => BRIEF_DESCRIPTION;

	/// <summary>
	/// Detailed description of the dataset.
	/// </summary>
	public string DetailedDescription => DETAILED_DESCRIPTION;

	/// <summary>
	/// Gets the number of elements in the dataset.
	/// If the dataset hasn't been downloaded yet, this will be -1.
	/// </summary>
	public int TotalCount => (_trainDataset, _testDataset) switch
	{
		(null, null) => -1,
		(Dataset train, Dataset test) => (int)(train.Count + test.Count),
		_ => throw new InvalidOperationException(
			"Dataset is in an invalid state."
		)
	};

	/// <summary>
	/// Gets the total number of elements in the dataset's training set.
	/// If the dataset hasn't been downloaded yet, this will be -1.
	/// </summary>
	public int TrainingCount => (int)(_trainDataset?.Count ?? -1);

	/// <summary>
	/// Gets the total number of elements in the dataset's test set.
	/// If the dataset hasn't been downloaded yet, this will be -1.
	/// </summary>
	public int TestCount => (int)(_testDataset?.Count ?? -1);

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
	/// Key that input tensors are stored under in the dataset's dictionary.
	/// </summary>
	private const string KEY_INPUT = "data";

	/// <summary>
	/// Key that label tensors are stored under in the dataset's dictionary.
	/// </summary>
	private const string KEY_LABELS = "label";

	/// <summary>
	/// Training dataset for the MNIST dataset.
	/// </summary>
	private Dataset? _trainDataset;

	/// <summary>
	/// Test dataset for the MNIST dataset.
	/// </summary>
	private Dataset? _testDataset;

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
	public MnistDataset(string saveLocation)
	{
		SaveLocation = saveLocation;

		// If the dataset is downloaded, load the dataset from disk
		try
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
		catch (DirectoryNotFoundException)
		{
			// The dataset is not downloaded
			_trainDataset = null;
			_testDataset = null;
		}
		catch (FileNotFoundException)
		{
			// One or more required dataset files are missing
			_trainDataset = null;
			_testDataset = null;
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
			else if (id < 0 || id >= TotalCount)
			{
				throw new ArgumentOutOfRangeException(
					nameof(id),
					id,
					$"ID must be in the range [0, {TotalCount})."
				);
			}

			// These should be true if `IsDownloaded` is true
			Debug.Assert(_trainDataset != null);
			Debug.Assert(_testDataset != null);

			// Get the dataset that the element is in
			var dataset = id < _trainDataset.Count
				? _trainDataset
				: _testDataset;
			var elementId = id < _trainDataset.Count
				? id
				: id - _trainDataset.Count;
			var dict = dataset.GetTensor(elementId);
			return new GrayscaleImageDatasetElement(
				this,
				id,
				dict[KEY_INPUT],
				dict[KEY_LABELS]
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

		for (var i = 0; i < TotalCount; i++)
		{
			yield return this[i];
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

		// There's currently a bug in TorchSharp (as of 2023-08-26) where
		//   downloaded files are always saved to the current working directory
		//   instead of the path that was specified. Until this is fixed,
		//   work around the issue by catching the file not found exception
		//   caused by the files getting downloaded to the wrong location,
		//   moving the files to the proper location, then re-trying the load
		//   operation.
		// These files will be created by the download process and will be
		//   placed in the current working directory. The expected location for
		//   these files is the within the dataset's folder.
		string[] DOWNLOADED_FILES = {
			"train-images-idx3-ubyte.gz",
			"train-labels-idx1-ubyte.gz",
			"t10k-images-idx3-ubyte.gz",
			"t10k-labels-idx1-ubyte.gz"
		};
		try
		{
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
		}
		catch (FileNotFoundException)
		{
			var srcFolder = Directory.GetCurrentDirectory();
			var dstFolder = Path.Combine(
				SaveLocation,
				// Based on the code for TorchSharp, `DownloadFile()` should
				//   end up putting the downloaded file in `[root]/mnist`, where
				//   `[root]` is the directory passed to the ctor.
				// Comment indicating that files should be placed in the
				//   directory passed to the ctor:
				//   https://github.com/dotnet/TorchSharp/blob/d7be25b4c5946c426920d0aef539fccd4979d5a9/src/TorchVision/dsets/MNIST.cs#L24
				// Call to `DownloadFile()`:
				//   https://github.com/dotnet/TorchSharp/blob/d7be25b4c5946c426920d0aef539fccd4979d5a9/src/TorchVision/dsets/MNIST.cs#L174
				// `DownloadFile()` implementation:
				//   https://github.com/dotnet/TorchSharp/blob/d7be25b4c5946c426920d0aef539fccd4979d5a9/src/TorchVision/dsets/CIFAR.cs#L86
				// Note: At the time of writing this comment, commit
				//   d7be25b4c5946c426920d0aef539fccd4979d5a9 is the most recent
				//   commit on the `main` branch of TorchSharp. Commit
				//   66f58fc5ab629df6ad28d8427f3a8a0a06bf7502 is the commit
				//   that TorchSharp v0.100.4 is based on.
				"mnist"
			);

			// Move the downloaded files to the proper location
			foreach (var file in DOWNLOADED_FILES)
			{
				var srcPath = Path.Combine(srcFolder, file);
				var dstPath = Path.Combine(dstFolder, file);

				// If the file already exists, delete it
				if (File.Exists(dstPath))
				{
					File.Delete(dstPath);
				}

				// Move the file to the proper location
				File.Move(srcPath, dstPath);
			}
		}
		finally
		{
			// `download: true` is needed here so that the download process is
			//   retried. The files will be re-downloaded into the wrong
			//   location a second time, but the rest of the code will work
			//   because the first set of downloaded files were moved to the
			//   proper location.
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

			// Delete the downloaded files that were placed in the current
			//   working directory
			foreach (var file in DOWNLOADED_FILES)
			{
				var path = Path.Combine(Directory.GetCurrentDirectory(), file);
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
		}

		LoadData();
		OnDownloaded?.Invoke(this, EventArgs.Empty);
	}

	/// <summary>
	/// Downloads the dataset to disk.
	/// If the dataset is already downloaded, this method will be a no-op.
	/// </summary>
	/// <returns>A task set once the download has completed.</returns>
	public async Task DownloadAsync()
	{
		await Task.Run(Download)
			.ConfigureAwait(true);
	}

	/// <summary>
	/// Creates a new instance of the dataset.
	/// This will create a dataset instance where the dataset elements are
	///   divided between training, validation, and test sets.
	/// </summary>
	/// <param name="device">
	/// Device to create the dataset instance on.
	/// </param>
	/// <param name="shuffle">
	/// Whether to shuffle the dataset elements before creating the training,
	///   validation, and test sets.
	/// </param>
	/// <returns>A new dataset instance.</returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown if the dataset is not downloaded.
	/// </exception>
	public IDatasetInstance CreateTrainingInstance(
		Device device,
		bool shuffle = true)
	{
		if (!IsDownloaded)
		{
			throw new InvalidOperationException(
				"Cannot create a dataset instance from a dataset that is " +
				"not downloaded."
			);
		}

		// These should be true if `IsDownloaded` is true
		Debug.Assert(_trainDataset != null);
		Debug.Assert(_testDataset != null);

		return new GenericDatasetInstance(
			this,
			device,
			new GenericDatasetSlice(
				this,
				device,
				_trainDataset,
				(slice, id, input, label) => new GrayscaleImageDatasetElement(
					slice.Dataset,
					id,
					input,
					label
				),
				shuffle
			),
			// TODO: Figure out how to divide the training dataset into train
			//   and validation sets
			new GenericDatasetSlice(
				this,
				device,
				_testDataset,
				(slice, id, input, label) => new GrayscaleImageDatasetElement(
					slice.Dataset,
					id,
					input,
					label
				),
				shuffle
			),
			new GenericDatasetSlice(
				this,
				device,
				_testDataset,
				(slice, id, input, label) => new GrayscaleImageDatasetElement(
					slice.Dataset,
					id,
					input,
					label
				),
				shuffle
			),
			new Size(new int[]
				{ 1, 28, 28 }
			),
			new Size(new int[]
				{ 10 }
			)
		);
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

		// Calculate and save the size of the dataset on disk
		SizeOnDiskBytes = Directory.GetFiles(
			SaveLocation,
			"*",
			SearchOption.AllDirectories)
			.Sum(path => new FileInfo(path).Length);
	}
}
