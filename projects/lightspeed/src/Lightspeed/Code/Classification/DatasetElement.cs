/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using static TorchSharp.torch;
namespace Lightspeed.Classification;

/// <summary>
/// Represents a single element in a dataset.
/// Dataset element instances are not meant for use in training or evaluation.
///   Rather, they're designed for use by Lightspeed's UI code to display
///   information about the dataset.
/// </summary>
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
	/// Tensor containing the input data for the element.
	/// </summary>
	Tensor DataTensor { get; }

	/// <summary>
	/// Tensor containing the label data for the element.
	/// </summary>
	Tensor LabelsTensor { get; }

	/// <summary>
	/// String to display as the label of the element.
	/// </summary>
	string Label { get; }
}
