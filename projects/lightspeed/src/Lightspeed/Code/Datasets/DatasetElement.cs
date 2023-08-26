/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Lightspeed.Datasets;

/// Represents a single element in a dataset.
/// Dataset element instances are not meant for use in training or evaluation.
///   Rather, they're designed for use by Lightspeed's UI code to display
///   information about the dataset.
public interface IDatasetElement
{
	/// <summary>
	/// Dataset that the element is from.
	/// </summary>
	IDataset Dataset { get; }

	/// <summary>
	/// ID of the element within the dataset.
	/// </summary>
	long Id { get; }

	/// <summary>
	/// Gets the image data for the element as a base64 encoded string.
	/// </summary>
	string ImageDataBase64 { get; }
}
