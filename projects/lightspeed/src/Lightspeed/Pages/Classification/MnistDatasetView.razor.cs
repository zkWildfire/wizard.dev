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
	/// Index in the dataset to start displaying elements from.
	/// </summary>
	[Parameter]
	public int StartIndex { get; set; }

	/// <summary>
	/// Number of elements to display.
	/// </summary>
	[Parameter]
	public int Count { get; set; }

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
	/// Default number of elements to display per page.
	/// </summary>
	private const int DEFAULT_ELEMENTS_PER_PAGE = 100;

	/// <summary>
	/// Past-the-end index for the range of elements to display.
	/// </summary>
	private int EndIndex => Math.Min(StartIndex + Count, _dataset.Count);

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

		// Handle default values
		// Note that since the default value for `StartIndex` is 0, nothing
		//   needs to be done for that parameter
		if (Count == 0)
		{
			Count = DEFAULT_ELEMENTS_PER_PAGE;
		}

		// Get the MNIST dataset
		_dataset = DatasetService.AvailableDatasets[MnistDataset.DATASET_ID];

		// If the dataset isn't downloaded or any parameters are invalid,
		//   redirect to the dataset selection page
		if (!_dataset.IsDownloaded ||
			StartIndex < 0 || StartIndex >= _dataset.Count ||
			Count < 0 || Count > _dataset.Count)
		{
			NavigationManager.NavigateTo("/datasets");
			return;
		}
	}
}
