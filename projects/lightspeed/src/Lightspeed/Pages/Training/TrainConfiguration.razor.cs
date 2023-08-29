/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification;
using Lightspeed.Components.Training;
using Lightspeed.Services.Datasets;
using Lightspeed.Services.Models;
namespace Lightspeed.Pages.Training;

/// <summary>
/// Handles displaying the train configuration page.
/// This page allows users to specify the dataset, model, and model parameters
///   for a training session.
/// </summary>
public partial class TrainConfiguration : ComponentBase
{
	/// <summary>
	/// Service used to get all available datasets.
	/// </summary>
	[Inject]
	private IDatasetService DatasetService { get; set; } = null!;

	/// <summary>
	/// Service used to get all available models.
	/// </summary>
	[Inject]
	private IModelsService ModelsService { get; set; } = null!;

	/// <summary>
	/// Helper property used to bind to dataset selector events.
	/// </summary>
	private DatasetSelector? DatasetSelectors
	{
		set
		{
			Debug.Assert(value != null);
			value.OnDatasetSelected += OnDatasetSelected;
		}
	}

	/// <summary>
	/// Dataset currently selected by the user.
	/// </summary>
	private IDataset? _selectedDataset;

	/// <summary>
	/// Whether the dataset is the currently selected dataset.
	/// </summary>
	/// <param name="dataset">Dataset to check.</param>
	/// <returns>
	/// Whether the dataset is the currently selected dataset.
	/// </returns>
	private bool IsSelectedDataset(IDataset dataset)
	{
		return _selectedDataset == dataset;
	}

	/// <summary>
	/// Callback invoked when a dataset is selected.
	/// </summary>
	/// <param name="sender">Object that broadcast the event.</param>
	/// <param name="e">Arguments for the event.</param>
	private void OnDatasetSelected(
		object? sender,
		OnDatasetSelectedEventArgs e)
	{
		_selectedDataset = e.Dataset;
		StateHasChanged();
	}
}
