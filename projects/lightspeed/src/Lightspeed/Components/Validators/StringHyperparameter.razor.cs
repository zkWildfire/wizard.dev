/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Validators;
namespace Lightspeed.Components.Validators;

/// <summary>
/// Allows a model-specific string hyperparameter to be specified.
/// </summary>
public partial class StringHyperparameter : ComponentBase
{
	/// <summary>
	/// Event broadcast to when the hyperparameter value is set.
	/// </summary>
	public event EventHandler<OnHyperparameterSetEventArgs>? OnHyperparameterSet;

	/// <summary>
	/// Validator for the hyperparameter the component is for.
	/// </summary>
	[Parameter]
	public StringHyperparameterValidator Validator { get; set; } = null!;

	/// <summary>
	/// Value set for the hyperparameter on the UI.
	/// This value may or may not be a valid value. Parameter validation is only
	///   done once the user clicks the "Train" button.
	/// </summary>
	public string Value
	{
		get => _value;
		private set
		{
			_value = value;
			OnHyperparameterSet?.Invoke(
				this,
				new OnHyperparameterSetEventArgs()
				{
					Validator = Validator,
					Value = value
				}
			);
		}
	}

	/// <summary>
	/// Field backing the <see cref="Value"/> property.
	/// </summary>
	private string _value = string.Empty;

	/// <summary>
	/// Initializes the component.
	/// </summary>
	protected override void OnInitialized()
	{
		base.OnInitialized();
		Value = Validator.DefaultValue ?? "";
	}
}
