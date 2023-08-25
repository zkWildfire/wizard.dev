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
	/// Dataset that the element is from.
	IDataset Dataset { get; }

	/// ID of the element within the dataset.
	long Id { get; }

	/// Gets the image data for the element as a base64 encoded string.
	string ImageDataBase64 { get; }
}
