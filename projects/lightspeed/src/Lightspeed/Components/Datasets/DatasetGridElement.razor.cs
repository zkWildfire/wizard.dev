/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification;
using Lightspeed.Services.Datasets;
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
	/// ID of the dataset.
	/// </summary>
	[Parameter]
	public string Id { get; set; } = string.Empty;

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

	/// <summary>
	/// Dataset service to retrieve the dataset this element represents from.
	/// </summary>
	[Inject]
	private IDatasetService DatasetService { get; set; } = null!;

	/// <summary>
	/// Dataset this element represents.
	/// </summary>
	private IDataset _dataset = null!;

	/// <summary>
	/// Whether the dataset is currently being downloaded.
	/// </summary>
	private bool _isDownloading;

	/// <summary>
	/// Initializes the component.
	/// </summary>
	protected override void OnInitialized()
	{
		base.OnInitialized();

		// Look up the dataset this element represents
		_dataset = DatasetService.AvailableDatasets
			.Single(dataset => dataset.Key == Id).Value;
		_dataset.OnDownloaded += OnDownloaded;
	}

	/// <summary>
	/// Callback invoked when the download button is clicked.
	/// </summary>
	private async Task OnDownloadClicked()
	{
		_isDownloading = true;
		await _dataset.DownloadAsync().ConfigureAwait(true);
	}

	/// <summary>
	/// Callback invoked when the dataset is downloaded.
	/// </summary>
	/// <param name="sender">Object that invoked the event.</param>
	/// <param name="e">
	/// Event arguments. For this event type, the event args should always be
	/// empty event args.
	/// </param>
	private void OnDownloaded(object? sender, EventArgs e)
	{
		_isDownloading = false;
		_ = InvokeAsync(StateHasChanged);
	}
}
