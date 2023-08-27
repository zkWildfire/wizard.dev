/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Lightspeed.Classification;

/// <summary>
/// Base interface for dataset elements consisting of a single image.
/// </summary>
public interface IDatasetImageElement : IDatasetElement
{
	/// <summary>
	/// Gets the image data for the element as a base64 encoded string.
	/// </summary>
	string ImageDataBase64 { get; }
}
