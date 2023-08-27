/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Generic;
namespace Lightspeed.Components.Datasets;

/// <summary>
/// Displays an element from a dataset as a grayscale image.
/// </summary>
public partial class GrayscaleImageElement : ComponentBase
{
	/// <summary>
	/// Data for the element to display.
	/// </summary>
	[Parameter]
	public GrayscaleImageDatasetElement Element { get; set; } = null!;

	/// <summary>
	/// Gets the image data for the element as a base64 encoded string.
	/// </summary>
	private string ImageData =>
		$"data:image/png;base64,{Element.ImageDataBase64}";

	/// <summary>
	/// Initializes the component.
	/// </summary>
	protected override void OnInitialized()
	{
		base.OnInitialized();
		Debug.Assert(Element != null);
	}
}
