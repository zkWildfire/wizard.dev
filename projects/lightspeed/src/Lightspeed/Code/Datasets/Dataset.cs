/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Lightspeed.Datasets;

/// Represents a dataset used for training and evaluation.
public interface IDataset : IDisposable, IEnumerable<IDatasetElement>
{
	/// Name to display for the dataset on the UI.
	string DisplayName { get; }

	/// Gets the number of elements in the dataset.
	/// If the dataset hasn't been downloaded yet, this will be -1.
	long Count { get; }

	/// Path to the folder on disk containing the dataset's files.
	/// If the dataset hasn't been downloaded yet, this path may not exist.
	string SaveLocation { get; }

	/// Whether the dataset has been downloaded.
	bool IsDownloaded { get; }

	/// Gets the size of the dataset on disk in bytes.
	long SizeOnDiskBytes { get; }

	/// Gets the size of the dataset on disk in kilobytes.
	double SizeOnDiskKB { get; }

	/// Gets the size of the dataset on disk in megabytes.
	double SizeOnDiskMB { get; }

	/// Gets the size of the dataset on disk in gigabytes.
	double SizeOnDiskGB { get; }

	/// Gets the dataset element at the given ID.
	/// Datasets should always assign the same ID to each element no matter
	///   how many times the dataset is loaded and unloaded from memory. This
	///   ensures that the UI is "stable", e.g. the same element is always
	///   displayed at the same position in the UI.
	/// @throws ArgumentOutOfRangeException If the given ID is outside the range
	///   `[0, Count)`.
	/// @throws InvalidOperationException If the dataset is not downloaded.
	IDatasetElement this[int id] { get; }

	/// Downloads the dataset to disk.
	/// If the dataset is already downloaded, this method will be a no-op.
	void Download();

	/// Downloads the dataset to disk.
	/// If the dataset is already downloaded, this method will be a no-op.
	Task DownloadAsync();

	/// Creates a new instance of the dataset.
	/// This will create a dataset instance where the dataset elements are
	///   divided between training, validation, and test sets.
	/// @param shuffle Whether to shuffle the dataset elements before creating
	///   the training, validation, and test sets.
	/// @returns A new dataset instance.
	IDatasetInstance CreateTrainingInstance(bool shuffle = true);
}
