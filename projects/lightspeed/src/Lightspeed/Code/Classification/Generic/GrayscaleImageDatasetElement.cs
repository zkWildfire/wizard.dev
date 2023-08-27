/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Utils;
using static TorchSharp.torch;
namespace Lightspeed.Classification.Generic;

/// Dataset element that provides access to a grayscale image.
public class GrayscaleImageDatasetElement : IDatasetImageElement
{
	/// <summary>
	/// Dataset that the element is from.
	/// </summary>
	public IDataset Dataset { get; }

	/// <summary>
	/// ID of the element within the dataset.
	/// </summary>
	public long Id { get; }

	/// <summary>
	/// Tensor containing the input data for the element.
	/// </summary>
	public Tensor DataTensor { get; }

	/// <summary>
	/// Tensor containing the label data for the element.
	/// </summary>
	public Tensor LabelsTensor { get; }

	/// <summary>
	/// String to display as the label of the element.
	/// </summary>
	public string Label { get; }

	/// <summary>
	/// Gets the image data for the element as a base64 encoded string.
	/// </summary>
	public string ImageDataBase64 { get; }

	/// <summary>
	/// Initializes the dataset element.
	/// </summary>
	/// <param name="parent">Dataset that the element is from.</param>
	/// <param name="id">
	/// ID of the element within the dataset. If the element is created by a
	///   dataset slice, this will be the ID of the element within the slice.
	/// </param>
	/// <param name="data">Tensor containing the input data.</param>
	/// <param name="labels">Tensor containing the labels data.</param>
	public GrayscaleImageDatasetElement(
		IDataset parent,
		long id,
		Tensor data,
		Tensor labels)
	{
		Dataset = parent;
		Id = id;
		DataTensor = data;
		LabelsTensor = labels;
		Label = labels.data<long>()[0].ToString(CultureInfo.InvariantCulture);

		// Input tensors are expected to be 3D tensors of the format
		//   [channels, height, width]
		Debug.Assert(data.dim() == 3);
		var imageHeight = (int)data.size(1);
		var imageWidth = (int)data.size(2);

		// In the bitmap to be generated, each pixel will be represented by
		//   a single [0, 255] byte. However, currently, the data is in float32
		//   format and in the range [0, 1]. Scale the data to the range
		//   [0, 255], then convert it to a byte array.
		var byteData = (data.flatten() * 255)
			.to_type(ScalarType.Byte)
			.data<byte>()
			.ToArray<byte>();

		// Get the bitmap data as a base64 string
		var bitmap = new GrayscaleBitmap(imageWidth, imageHeight, byteData);
		ImageDataBase64 = bitmap.ToBase64String();
	}
}
