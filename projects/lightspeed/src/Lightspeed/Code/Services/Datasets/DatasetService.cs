/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification;
namespace Lightspeed.Services.Datasets;

/// <summary>
/// Event arguments for the `OnDatasetDownloaded` event.
/// </summary>
public class OnDatasetDownloadedEventArgs : EventArgs
{
	/// <summary>
	/// The dataset that was downloaded.
	/// </summary>
	public required IDataset Dataset { get; init; }
}

/// <summary>
/// Interface for services that provide access to available datasets.
/// All datasets made available by instances of this interface will be owned by
///   the implementing service. The service will be responsible for calling
///   `Dispose()` on all datasets it owns when it is disposed.
/// </summary>
public interface IDatasetService : IDisposable
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
	event EventHandler<OnDatasetDownloadedEventArgs>? OnDatasetDownloaded;

	/// <summary>
	/// All available datasets, indexed by dataset ID.
	/// </summary>
	IReadOnlyDictionary<string, IDataset> AvailableDatasets { get; }

	/// <summary>
	/// All downloaded datasets, indexed by dataset ID.
	/// </summary>
	IReadOnlyDictionary<string, IDataset> DownloadedDatasets { get; }
}
