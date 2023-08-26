/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using static TorchSharp.torch;
namespace Lightspeed.Datasets.Generic;

/// Dataset element that provides access to a grayscale image.
public class GrayscaleImageDatasetElement : IDatasetElement
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
	/// <param name="imageTensor">Tensor containing the image data.</param>
	public GrayscaleImageDatasetElement(
		IDataset parent,
		long id,
		Tensor imageTensor)
	{
		Dataset = parent;
		Id = id;

		// Convert the image data to a base64 string
		var imageData = imageTensor.to_type(ScalarType.Byte).data<byte>();
		ImageDataBase64 = Convert.ToBase64String(imageData.ToArray<byte>());
	}
}
