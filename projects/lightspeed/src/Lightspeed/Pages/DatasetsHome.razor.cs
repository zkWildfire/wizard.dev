/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Lightspeed.Pages;

/// <summary>
/// Stores information about each available dataset to be displayed on the UI.
/// </summary>
public readonly record struct DatasetInfo
{
	/// <summary>
	/// Display name of the dataset.
	/// </summary>
	public required string Name { get; init; }

	/// <summary>
	/// Address of the image to display for the dataset.
	/// </summary>
	public required Uri Image { get; init; }

	/// <summary>
	/// Address of the dataset information page for the dataset.
	/// </summary>
	public required Uri Address { get; init; }

	/// <summary>
	/// Description of the dataset.
	/// </summary>
	public required string Description { get; init; }
}

/// <summary>
/// Page that displays all available datasets.
/// </summary>
public partial class DatasetsHome : ComponentBase
{
	/// <summary>
	/// Information about all available datasets.
	/// </summary>
	private readonly IReadOnlyList<DatasetInfo> _datasets;

	/// <summary>
	/// Initializes the page.
	/// </summary>
	public DatasetsHome()
	{
		_datasets = new List<DatasetInfo>
		{
			new DatasetInfo
			{
				Name = "MNIST",
				Image = new Uri("/img/datasets/mnist.png", UriKind.Relative),
				Address = new Uri("/datasets/mnist", UriKind.Relative),
				Description="A large database of handwritten digits created " +
					"by the National Institute of Standards and Technology. " +
					"The dataset consists of 60,000 training images and " +
					"10,000 testing images."
			}
		};
	}
}
