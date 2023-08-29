/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification;
using Lightspeed.Classification.Mnist;
namespace Lightspeed.Services.Datasets;

/// <summary>
/// Service that provides access to built-in Torch datasets.
/// </summary>
public sealed class TorchDatasetsService : IDatasetService
{
	/// <summary>
	/// Event broadcast to when a dataset has been downloaded.
	/// The sender of the event will be the dataset service instance.
	/// </summary>
	/// <remarks>
	/// If a dataset was previously downloaded, e.g. `dataset.IsDownloaded` is
	///   true immediately after the constructor finishes executing, this event
	///   will never be broadcast to for that dataset.
	/// </remarks>
	public event EventHandler<OnDatasetDownloadedEventArgs>? OnDatasetDownloaded;

	/// <summary>
	/// All available datasets, indexed by dataset ID.
	/// </summary>
	public IReadOnlyDictionary<string, IDataset> AvailableDatasets { get; }

	/// <summary>
	/// All downloaded datasets, indexed by dataset ID.
	/// </summary>
	public IReadOnlyDictionary<string, IDataset> DownloadedDatasets =>
		_downloadedDatasets;

	/// <summary>
	/// Dictionary of all datasets that have been downloaded.
	/// </summary>
	private readonly Dictionary<string, IDataset> _downloadedDatasets = new();

	/// <summary>
	/// Whether the service has been disposed.
	/// </summary>
	private bool _isDisposed;

	/// <summary>
	/// Initializes the service.
	/// </summary>
	/// <param name="saveLocation">
	/// Path to the folder where datasets should be saved.
	/// </param>
	public TorchDatasetsService(string saveLocation)
	{
		AvailableDatasets = new Dictionary<string, IDataset>()
		{
			// Torch will append the name of the dataset to the path passed
			//   to it, so it isn't necessary to add the dataset name to the
			//   save path for each dataset
			{
				MnistDataset.DATASET_ID,
				new MnistDataset(
					saveLocation
				)
			}
		};

		// If any dataset is already downloaded, add it to the downloaded
		//   datasets dictionary
		foreach (var dataset in AvailableDatasets.Values)
		{
			// Also bind to the dataset's `OnDownloaded` event so that the
			//   if the dataset is deleted, it will be re-added to the
			//   downloaded datasets dictionary when it is downloaded again
			dataset.OnDownloaded += (_, _) => NotifyDatasetDownloaded(dataset);
			if (dataset.IsDownloaded)
			{
				_downloadedDatasets.Add(dataset.Id, dataset);
			}
		}
	}

	/// <summary>
	/// Disposes of all datasets owned by the service.
	/// </summary>
	public void Dispose()
	{
		if (_isDisposed)
		{
			return;
		}

		// `AvailableDatasets` is a superset of `DownloadedDatasets`, so it
		//   isn't necessary to iterate over both dictionaries
		foreach (var dataset in AvailableDatasets.Values)
		{
			dataset.Dispose();
		}
		_isDisposed = true;
	}

	/// <summary>
	/// Callback invoked when a dataset has been downloaded.
	/// </summary>
	/// <param name="dataset">The dataset that was downloaded.</param>
	private void NotifyDatasetDownloaded(IDataset dataset)
	{
		_downloadedDatasets.Add(dataset.Id, dataset);
		OnDatasetDownloaded?.Invoke(
			this,
			new OnDatasetDownloadedEventArgs
			{
				Dataset = dataset
			}
		);
	}
}
