/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Training;
namespace Lightspeed.Serialization;

/// <summary>
/// Interface for types that provide access to metadata saved on disk.
/// </summary>
public interface IModelMetadata
{
	/// <summary>
	/// Checks if the given directory path contains a valid metadata file.
	/// </summary>
	/// <param name="directoryPath">
	/// Path to the directory to check.
	/// </param>
	/// <returns>
	/// True if the given directory path contains a valid metadata file, false
	///   otherwise.
	/// </returns>
	bool IsValidDirectory(string directoryPath);

	/// <summary>
	/// Loads the training metrics from the given directory's metadata file.
	/// </summary>
	/// <param name="directoryPath">
	/// Path to the directory to load metadata from.
	/// </param>
	/// <returns>
	/// The training metrics data from the metadata file. If the directory does
	///   not contain a valid metadata file or the metadata file does not
	///   contain training metrics, null is returned.
	/// </returns>
	ModelMetrics? LoadTrainingMetrics(string directoryPath);

	/// <summary>
	/// Loads the training metrics from the given directory's metadata file.
	/// </summary>
	/// <param name="directoryPath">
	/// Path to the directory to load metadata from.
	/// </param>
	/// <returns>
	/// The training metrics data from the metadata file. If the directory does
	///   not contain a valid metadata file or the metadata file does not
	///   contain training metrics, null is returned.
	/// </returns>
	Task<ModelMetrics?> LoadTrainingMetricsAsync(string directoryPath);

	/// <summary>
	/// Loads the validation metrics from the given directory's metadata file.
	/// </summary>
	/// <param name="directoryPath">
	/// Path to the directory to load metadata from.
	/// </param>
	/// <returns>
	/// The validation metrics data from the metadata file. If the directory
	///   does not contain a valid metadata file or the metadata file does not
	///   contain validation metrics, null is returned.
	/// </returns>
	ModelMetrics? LoadValidationMetrics(string directoryPath);

	/// <summary>
	/// Loads the validation metrics from the given directory's metadata file.
	/// </summary>
	/// <param name="directoryPath">
	/// Path to the directory to load metadata from.
	/// </param>
	/// <returns>
	/// The validation metrics data from the metadata file. If the directory
	///   does not contain a valid metadata file or the metadata file does not
	///   contain validation metrics, null is returned.
	/// </returns>
	Task<ModelMetrics?> LoadValidationMetricsAsync(string directoryPath);
}
