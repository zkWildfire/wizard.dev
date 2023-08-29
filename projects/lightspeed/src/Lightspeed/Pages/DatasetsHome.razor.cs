/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Mnist;
using Lightspeed.Services.Datasets;
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
	/// ID of the dataset.
	/// </summary>
	public required string Id { get; init; }

	/// <summary>
	/// Path to the icon for the dataset.
	/// </summary>
	public required Uri Icon { get; init; }

	/// <summary>
	/// Description of the dataset.
	/// </summary>
	public required string Description { get; init; }

	/// <summary>
	/// Address of the dataset information page for the dataset.
	/// </summary>
	public Uri Address => new(
		$"/datasets/{MnistDataset.DATASET_ID}",
		UriKind.Relative
	);
}

/// <summary>
/// Page that displays all available datasets.
/// </summary>
public partial class DatasetsHome : ComponentBase
{
	/// <summary>
	/// Service used to get all available datasets.
	/// </summary>
	[Inject]
	private IDatasetService DatasetsService { get; set; } = null!;

	/// <summary>
	/// Information about all available datasets.
	/// </summary>
	private IReadOnlyList<DatasetInfo> _datasets = null!;

	/// <summary>
	/// Initializes the page.
	/// </summary>
	protected override void OnInitialized()
	{
		base.OnInitialized();
		var datasets = new List<DatasetInfo>();
		foreach (var (_, dataset) in DatasetsService.AvailableDatasets)
		{
			datasets.Add(new DatasetInfo
			{
				Name = dataset.DisplayName,
				Id = dataset.Id,
				Icon = dataset.IconPath,
				Description = dataset.DetailedDescription
			});
		}

		_datasets = datasets;
	}
}
