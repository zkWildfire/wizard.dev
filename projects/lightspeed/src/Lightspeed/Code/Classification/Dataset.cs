/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Lightspeed.Classification;

/// <summary>
/// Represents a dataset used for training and evaluation.
/// </summary>
public interface IDataset : IDisposable, IEnumerable<IDatasetElement>
{
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
	event EventHandler? OnDownloaded;

	/// <summary>
	/// Lightspeed internal unique ID for the dataset.
	/// </summary>
	string Id { get; }

	/// <summary>
	/// Name to display for the dataset on the UI.
	/// </summary>
	string DisplayName { get; }

	/// <summary>
	/// Gets the number of elements in the dataset.
	/// If the dataset hasn't been downloaded yet, this will be -1.
	/// </summary>
	int Count { get; }

	/// <summary>
	/// Path to the folder on disk containing the dataset's files.
	/// If the dataset hasn't been downloaded yet, this path may not exist.
	/// </summary>
	string SaveLocation { get; }

	/// <summary>
	/// Whether the dataset has been downloaded.
	/// </summary>
	/// <remarks>
	/// If either the training or test dataset is not null, the other dataset
	///   should also be not-null.
	/// </remarks>
	bool IsDownloaded { get; }

	/// <summary>
	/// Gets the size of the dataset on disk in bytes.
	/// </summary>
	long SizeOnDiskBytes { get; }

	/// <summary>
	/// Gets the size of the dataset on disk in kilobytes.
	/// </summary>
	double SizeOnDiskKB { get; }

	/// <summary>
	/// Gets the size of the dataset on disk in megabytes.
	/// </summary>
	double SizeOnDiskMB { get; }

	/// <summary>
	/// Gets the size of the dataset on disk in gigabytes.
	/// </summary>
	double SizeOnDiskGB { get; }

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
	IDatasetElement this[int id] { get; }

	/// <summary>
	/// Downloads the dataset to disk.
	/// If the dataset is already downloaded, this method will be a no-op.
	/// </summary>
	void Download();

	/// <summary>
	/// Downloads the dataset to disk.
	/// If the dataset is already downloaded, this method will be a no-op.
	/// </summary>
	/// <returns>A task set once the download has completed.</returns>
	Task DownloadAsync();

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
	IDatasetInstance CreateTrainingInstance(bool shuffle = true);
}
