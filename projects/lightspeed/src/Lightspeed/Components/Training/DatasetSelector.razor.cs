/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification;
namespace Lightspeed.Components.Training;

/// <summary>
/// Event arguments for when a dataset is selected.
/// </summary>
public class OnDatasetSelectedEventArgs : EventArgs
{
	/// <summary>
	/// Dataset that was selected.
	/// </summary>
	public required IDataset Dataset { get; init; }
}

/// <summary>
/// Displays a dataset that the user can select.
/// </summary>
public partial class DatasetSelector : ComponentBase
{
	/// <summary>
	/// Event broadcast to when the dataset is selected.
	/// </summary>
	public event EventHandler<OnDatasetSelectedEventArgs>? OnDatasetSelected;

	/// <summary>
	/// Dataset that the component displays.
	/// </summary>
	[Parameter]
	public IDataset Dataset { get; set; } = null!;

	/// <summary>
	/// Whether the dataset is currently selected.
	/// This should be set to true by the component's parent component when the
	///   `OnDatasetSelected` event is broadcast to.
	/// </summary>
	[Parameter]
	public bool IsSelected { get; set; }

	/// <summary>
	/// CSS to use for the button's main style.
	/// </summary>
	private string ButtonCss => IsSelected
		? "btn-success"
		: "btn-outline-success";

	/// <summary>
	/// Callback invoked when the "Select" button is clicked.
	/// </summary>
	private void OnSelectClicked()
	{
		OnDatasetSelected?.Invoke(
			this,
			new OnDatasetSelectedEventArgs
			{
				Dataset = Dataset
			}
		);
	}
}
