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
	/// Validator for the hyperparameter the component is for.
	/// </summary>
	[Parameter]
	public IHyperparameterValidator Validator { get; set; } = null!;
}
