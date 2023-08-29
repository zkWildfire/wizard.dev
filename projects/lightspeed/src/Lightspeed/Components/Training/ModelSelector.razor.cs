/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Models;
namespace Lightspeed.Components.Training;

/// <summary>
/// Event arguments for when a model is selected.
/// </summary>
public class OnModelSelectedEventArgs : EventArgs
{
	/// <summary>
	/// Model that was selected.
	/// </summary>
	public required IClassificationModel Model { get; init; }
}

/// <summary>
/// Displays a model that are available for training.
/// </summary>
public partial class ModelSelector : ComponentBase
{
	/// <summary>
	/// Event broadcast to when the model is selected.
	/// </summary>
	public event EventHandler<OnModelSelectedEventArgs>? OnModelSelected;

	/// <summary>
	/// Model that the component should display.
	/// </summary>
	[Parameter]
	public IClassificationModel Model { get; set; } = null!;

	/// <summary>
	/// Whether the model is currently selected.
	/// This should be set to true by the component's parent component when the
	///   `OnModelSelected` event is broadcast to.
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
		OnModelSelected?.Invoke(
			this,
			new OnModelSelectedEventArgs
			{
				Model = Model
			}
		);
	}
}
