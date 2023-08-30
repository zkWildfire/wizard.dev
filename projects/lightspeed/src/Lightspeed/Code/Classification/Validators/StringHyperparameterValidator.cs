/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Lightspeed.Classification.Validators;

/// <summary>
/// Hyperparameter validator used for string hyperparameters.
/// </summary>
public class StringHyperparameterValidator
	: IRefTypeHyperparameterValidator<string>
{
	/// <summary>
	/// Initializes the hyperparameter validator.
	/// </summary>
	/// <param name="id">
	/// Unique ID for the validator's parameter.
	/// This is only required to be unique per-model.
	/// </param>
	/// <param name="displayName">
	/// Name to display for the hyperparameter.
	/// </param>
	/// <param name="description">
	/// Description to display for the hyperparameter.
	/// </param>
	/// <param name="constraintFuncs">
	/// Functions to invoke to validate the hyperparameter. Each function will
	///   be passed the value specified on the UI and should return `true` if
	///   the value is valid or `false` if the value is invalid.
	/// </param>
	/// <param name="constraints">
	/// Description to display for constraints on the hyperparameter.
	/// </param>
	/// <param name="defaultValue">
	/// Optional default value to use for the hyperparameter.
	/// </param>
	/// <param name="isOptional">
	/// Whether the hyperparameter is optional.
	/// </param>
	public StringHyperparameterValidator(
		string id,
		string displayName,
		string description,
		IReadOnlyList<Func<string, bool>>? constraintFuncs = null,
		string? constraints = null,
		string? defaultValue = null,
		bool isOptional = false)
		: base(
			id,
			displayName,
			description,
			x => x,
			constraintFuncs,
			constraints,
			defaultValue,
			isOptional
		)
	{
	}
}
