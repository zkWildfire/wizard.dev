/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Training;
using System.Text.Json;
namespace Lightspeed.Serialization;

/// <summary>
/// Metadata class that saves model metadata in JSON format.
/// </summary>
public class JsonModelMetadata : IModelMetadata
{
	/// <summary>
	/// JSON-serializable struct used to save metadata.
	/// </summary>
	private struct Metadata
	{
		/// <summary>
		/// Epoch that the model's metrics were generated from.
		/// </summary>
		public int Epoch { get; set; }

		/// <summary>
		/// Metrics from the epoch for the training dataset.
		/// </summary>
		public ModelMetrics TrainingMetrics { get; set; }

		/// <summary>
		/// Metrics from the epoch for the validation dataset.
		/// </summary>
		public ModelMetrics ValidationMetrics { get; set; }
	}

	/// <summary>
	/// Name used for the metadata file.
	/// </summary>
	private const string METADATA_FILE_NAME = "metadata.json";

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
	public bool IsValidDirectory(string directoryPath)
	{
		return LoadMetadata(directoryPath) != null;
	}

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
	public ModelMetrics? LoadTrainingMetrics(string directoryPath)
	{
		var metadata = LoadMetadata(directoryPath);
		return metadata?.TrainingMetrics;
	}

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
	public async Task<ModelMetrics?> LoadTrainingMetricsAsync(
		string directoryPath)
	{
		var metadata = await LoadMetadataAsync(directoryPath)
			.ConfigureAwait(false);
		return metadata?.TrainingMetrics;
	}

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
	public ModelMetrics? LoadValidationMetrics(string directoryPath)
	{
		var metadata = LoadMetadata(directoryPath);
		return metadata?.ValidationMetrics;
	}

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
	public async Task<ModelMetrics?> LoadValidationMetricsAsync(
		string directoryPath)
	{
		var metadata = await LoadMetadataAsync(directoryPath)
			.ConfigureAwait(false);
		return metadata?.ValidationMetrics;
	}

	/// <summary>
	/// Loads the metadata from the given directory's metadata file.
	/// </summary>
	/// <param name="directoryPath">
	/// Directory to load metadata from.
	/// </param>
	/// <returns>
	/// Returns the metadata from the directory, if it exists. If the directory
	///   does not exist or does not contain a valid metadata file, null is
	///   returned.
	/// </returns>
	private static Metadata? LoadMetadata(string directoryPath)
	{
		// Check if the directory exists
		if (!Directory.Exists(directoryPath))
		{
			return null;
		}

		// Check if the metadata file exists
		var metadataFilePath = Path.Combine(
			directoryPath,
			METADATA_FILE_NAME
		);
		if (!File.Exists(metadataFilePath))
		{
			return null;
		}

		// Attempt to load the metadata file
		try
		{
			return JsonSerializer.Deserialize<Metadata>(
				File.ReadAllText(metadataFilePath)
			);
		}
		catch (JsonException)
		{
			return null;
		}
	}

	/// <summary>
	/// Loads the metadata from the given directory's metadata file.
	/// </summary>
	/// <param name="directoryPath">
	/// Directory to load metadata from.
	/// </param>
	/// <returns>
	/// Returns the metadata from the directory, if it exists. If the directory
	///   does not exist or does not contain a valid metadata file, null is
	///   returned.
	/// </returns>
	private static async Task<Metadata?> LoadMetadataAsync(string directoryPath)
	{
		// Check if the directory exists
		if (!Directory.Exists(directoryPath))
		{
			return null;
		}

		// Check if the metadata file exists
		var metadataFilePath = Path.Combine(
			directoryPath,
			METADATA_FILE_NAME
		);
		if (!File.Exists(metadataFilePath))
		{
			return null;
		}

		// Attempt to load the metadata file
		try
		{
			using var stream = File.OpenRead(metadataFilePath);
			return await JsonSerializer.DeserializeAsync<Metadata>(
				stream
			).ConfigureAwait(false);
		}
		catch (JsonException)
		{
			return null;
		}
	}
}
