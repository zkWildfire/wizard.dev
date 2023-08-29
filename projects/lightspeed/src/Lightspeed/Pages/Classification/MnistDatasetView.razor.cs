/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification;
using Lightspeed.Classification.Mnist;
using Lightspeed.Services.Datasets;
namespace Lightspeed.Pages.Classification;

/// <summary>
/// Page that displays the MNIST dataset.
/// </summary>
public partial class MnistDatasetView : ComponentBase
{
	/// <summary>
	/// Pagination index for the current page.
	/// </summary>
	[Parameter]
	public int StartIndex { get; set; }

	/// <summary>
	/// Dataset service to retrieve the MNIST dataset from.
	/// </summary>
	[Inject]
	private IDatasetService DatasetService { get; set; } = null!;

	/// <summary>
	/// Navigation manager for the app.
	/// </summary>
	[Inject]
	private NavigationManager NavigationManager { get; set; } = null!;

	/// <summary>
	/// Number of elements to display per page.
	/// </summary>
	private const int ELEMENTS_PER_PAGE = 100;

	/// <summary>
	/// Number of buttons to display in the pagination bar.
	/// </summary>
	private const int PAGINATION_BAR_SIZE = 10;

	/// <summary>
	/// Total number of pages that may be displayed.
	/// </summary>
	private int PAGINATION_PAGES => (int)Math.Ceiling(
		_dataset.TotalCount / (double)ELEMENTS_PER_PAGE
	);

	/// <summary>
	/// URL that the page is located at.
	/// </summary>
	private readonly Uri PAGE_URL = new(
		"/datasets/mnist",
		UriKind.Relative
	);

	/// <summary>
	/// Past-the-end index for the range of elements to display.
	/// </summary>
	private int EndIndex => Math.Min(
		StartIndex + ELEMENTS_PER_PAGE,
		_dataset.TotalCount
	);

	/// <summary>
	/// Dataset instance for the MNIST dataset.
	/// </summary>
	private IDataset _dataset = null!;

	/// <summary>
	/// Initializes the component.
	/// </summary>
	protected override void OnInitialized()
	{
		base.OnInitialized();

		// Get the MNIST dataset
		_dataset = DatasetService.AvailableDatasets[MnistDataset.DATASET_ID];

		// If the dataset isn't downloaded or any parameters are invalid,
		//   redirect to the dataset selection page
		if (!_dataset.IsDownloaded ||
			StartIndex < 0 ||
			StartIndex >= _dataset.TotalCount / ELEMENTS_PER_PAGE)
		{
			NavigationManager.NavigateTo("/datasets");
			return;
		}
	}
}
