/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using static TorchSharp.torch;
namespace Lightspeed.Classification;

/// <summary>
/// Base interface for dataset elements consisting of a single image.
/// </summary>
public interface IDatasetImageElement : IDatasetElement
{
	/// <summary>
	/// Tensor containing the input data for the element.
	/// </summary>
	Tensor DataTensor { get; }

	/// <summary>
	/// Tensor containing the label data for the element.
	/// </summary>
	Tensor LabelsTensor { get; }

	/// <summary>
	/// Gets the image data for the element as a base64 encoded string.
	/// </summary>
	string ImageDataBase64 { get; }
}
