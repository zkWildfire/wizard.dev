/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Services.Datasets;
namespace Lightspeed.Pages.Datasets;

/// <summary>
/// Page that displays the MNIST dataset.
/// </summary>
public partial class MnistDataset : ComponentBase
{
	/// <summary>
	/// Dataset service to retrieve the MNIST dataset from.
	/// </summary>
	[Inject]
	private IDatasetService DatasetService { get; set; } = null!;
}
