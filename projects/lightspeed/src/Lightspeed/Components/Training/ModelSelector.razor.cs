/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Models;
using Lightspeed.Components.Validators;
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
	/// Dictionary storing the value of each model-specific hyperparameter.
	/// Each key will be the unique ID of the hyperparameter and the value will
	///   be the stringified value of the hyperparameter.
	/// </summary>
	public IReadOnlyDictionary<string, string?> Hyperparameters =>
		_hyperparameters;

	/// <summary>
	/// Whether the model is currently selected.
	/// This should be set to true by the component's parent component when the
	///   `OnModelSelected` event is broadcast to.
	/// </summary>
	[Parameter]
	public bool IsSelected { get; set; }

	/// <summary>
	/// Helper property used to bind to the hyperparameter components.
	/// </summary>
	private Hyperparameter HyperparameterComponent
	{
		set
		{
			// Note that this setter will be invoked *after* `OnParametersSet()`
			//   gets called
			_hyperparameters[value.Validator.Id] = value.Value;
			value.OnHyperparameterSet += (sender, args) =>
			{
				_hyperparameters[args.Validator.Id] = args.Value;
			};
		}
	}

	/// <summary>
	/// CSS to use for the button's main style.
	/// </summary>
	private string ButtonCss => IsSelected
		? "btn-success"
		: "btn-outline-success";

	/// <summary>
	/// Dictionary storing the value of each model-specific hyperparameter.
	/// Each key will be the unique ID of the hyperparameter and the value will
	///   be the stringified value of the hyperparameter.
	/// </summary>
	private readonly Dictionary<string, string?> _hyperparameters = new();

	/// <summary>
	/// Updates the component after parameters are set.
	/// </summary>
	protected override void OnParametersSet()
	{
		base.OnParametersSet();
		_hyperparameters.Clear();
	}

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
