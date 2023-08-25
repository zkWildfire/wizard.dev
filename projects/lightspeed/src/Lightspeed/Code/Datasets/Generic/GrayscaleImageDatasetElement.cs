/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using static TorchSharp.torch;
namespace Lightspeed.Datasets.Generic;

/// Dataset element that provides access to a grayscale image.
public class GrayscaleImageDatasetElement : IDatasetElement
{
	/// Dataset that the element is from.
	public IDataset Dataset { get; }

	/// ID of the element within the dataset.
	public long Id { get; }

	/// Gets the image data for the element as a base64 encoded string.
	public string ImageDataBase64 { get; }

	/// Initializes the dataset element.
	/// @param parent Dataset that the element is from.
	/// @param id ID of the element within the dataset.
	/// @param imageTensor Tensor containing the image data.
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
