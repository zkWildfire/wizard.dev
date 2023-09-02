/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Validators;
namespace Lightspeed.Components.Validators;

/// <summary>
/// Event args type broadcast when a hyperparameter is set.
/// </summary>
public class OnHyperparameterSetEventArgs : EventArgs
{
	/// <summary>
	/// Validator for the hyperparameter.
	/// </summary>
	public required IHyperparameterValidator Validator { get; init; }

	/// <summary>
	/// Value that the hyperparameter was set to.
	/// </summary>
	public required string? Value { get; init; }
}

/// <summary>
/// Allows a model-specific hyperparameter to be specified.
/// </summary>
public partial class Hyperparameter : ComponentBase
{
	/// <summary>
	/// Event broadcast to when the hyperparameter value is set.
	/// </summary>
	public event EventHandler<OnHyperparameterSetEventArgs>? OnHyperparameterSet;

	/// <summary>
	/// Value set for the hyperparameter on the UI.
	/// </summary>
	public string? Value { get; private set; }

	/// <summary>
	/// Validator for the hyperparameter the component is for.
	/// </summary>
	[Parameter]
	public IHyperparameterValidator Validator { get; set; } = null!;

	/// <summary>
	/// Helper property used to bind to boolean components.
	/// </summary>
	private BoolHyperparameter BoolComponent
	{
		set
		{
			// Convert to the "1" and "0" form expected by models
			Value = value.Value ? "1" : "0";
			value.OnHyperparameterSet += (sender, args) =>
			{
				Value = args.Value;
				OnHyperparameterSet?.Invoke(this, new()
				{
					Validator = Validator,
					Value = args.Value
				});
			};
		}
	}

	/// <summary>
	/// Helper property used to bind to int components.
	/// </summary>
	private IntHyperparameter IntComponent
	{
		set
		{
			Value = value.Value.ToString(CultureInfo.InvariantCulture);
			value.OnHyperparameterSet += (sender, args) =>
			{
				Value = args.Value;
				OnHyperparameterSet?.Invoke(this, new()
				{
					Validator = Validator,
					Value = args.Value
				});
			};
		}
	}

	/// <summary>
	/// Helper property used to bind to float components.
	/// </summary>
	private FloatHyperparameter FloatComponent
	{
		set
		{
			Value = value.Value.ToString(CultureInfo.InvariantCulture);
			value.OnHyperparameterSet += (sender, args) =>
			{
				Value = args.Value;
				OnHyperparameterSet?.Invoke(this, new()
				{
					Validator = Validator,
					Value = args.Value
				});
			};
		}
	}

	/// <summary>
	/// Helper property used to bind to string components.
	/// </summary>
	private StringHyperparameter StringComponent
	{
		set
		{
			Value = value.Value;
			value.OnHyperparameterSet += (sender, args) =>
			{
				Value = args.Value;
				OnHyperparameterSet?.Invoke(this, new()
				{
					Validator = Validator,
					Value = args.Value
				});
			};
		}
	}

	/// <summary>
	/// Helper property used to bind to enum components.
	/// </summary>
	private EnumHyperparameter EnumComponent
	{
		set
		{
			Value = value.Value;
			value.OnHyperparameterSet += (sender, args) =>
			{
				Value = args.Value;
				OnHyperparameterSet?.Invoke(this, new()
				{
					Validator = Validator,
					Value = args.Value
				});
			};
		}
	}
}
