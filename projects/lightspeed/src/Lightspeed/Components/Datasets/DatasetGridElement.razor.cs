/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Lightspeed.Components.Datasets;

/// <summary>
/// Component that displays information about a single dataset in a grid.
/// </summary>
public partial class DatasetGridElement : ComponentBase
{
	/// <summary>
	/// Display name of the dataset.
	/// </summary>
	[Parameter]
	public string Name { get; set; } = string.Empty;

	/// <summary>
	/// Address of the dataset image.
	/// </summary>
	[Parameter]
	public Uri Image { get; set; } = new Uri("/", UriKind.Relative);

	/// <summary>
	/// Address of the dataset information page for the dataset.
	/// </summary>
	[Parameter]
	public Uri Address { get; set; } = new Uri("/", UriKind.Relative);

	/// <summary>
	/// Description of the dataset.
	/// </summary>
	[Parameter]
	public string Description { get; set; } = string.Empty;
}
