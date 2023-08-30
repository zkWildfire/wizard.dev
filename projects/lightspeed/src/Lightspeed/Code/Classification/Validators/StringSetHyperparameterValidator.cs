/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Lightspeed.Classification.Validators;

/// <summary>
/// Helper non-generic hyperparameter validator used for enum hyperparameters.
/// </summary>
public class StringSetHyperparameterValidator
	: IHyperparameterValidator
{
	/// <summary>
	/// Model specific unique ID for the hyperparameter.
	/// </summary>
	public string Id { get; }

	/// <summary>
	/// Name to display on the UI for the hyperparameter.
	/// </summary>
	public string DisplayName { get; }

	/// <summary>
	/// Brief description to display on the UI for the hyperparameter.
	/// </summary>
	public string Description { get; }

	/// <summary>
	/// Description of the constraints for the hyperparameter, if any.
	/// </summary>
	public string? Constraints { get; }

	/// <summary>
	/// Optional default value to use for the hyperparameter.
	/// </summary>
	public string? DefaultValue { get; }

	/// <summary>
	/// Whether the hyperparameter is optional.
	/// </summary>
	public bool IsOptional { get; }

	/// <summary>
	/// Type of the hyperparameter.
	/// This may only be one of the following types:
	/// - bool
	/// - any integral or floating point type
	/// - string
	/// - any enum type
	/// </summary>
	public Type ParameterType { get; }

	/// <summary>
	/// List of valid values in the order they should appear on the UI.
	/// </summary>
	public IReadOnlyList<string> OrderedValues { get; }

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
	/// <param name="allowedValues">
	/// List of valid values the hyperparameter is allowed to take. The order
	///   that values appear in this list is the order they will appear on the
	///   UI.
	/// </param>
	/// <param name="parameterType">
	/// Type of the underlying enum hyperparameter.
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
	public StringSetHyperparameterValidator(
		string id,
		string displayName,
		string description,
		IReadOnlyList<string> allowedValues,
		Type parameterType,
		string? constraints = null,
		string? defaultValue = null,
		bool isOptional = false)
	{
		Id = id;
		DisplayName = displayName;
		Description = description;
		Constraints = constraints;
		DefaultValue = defaultValue;
		IsOptional = isOptional;
		ParameterType = parameterType;
		OrderedValues = allowedValues;
	}

	/// <summary>
	/// Called to validate the value specified for the hyperparameter.
	/// </summary>
	/// <param name="value">Value specified on the UI.</param>
	/// <param name="errorMessage">
	/// Message explaining why the value is invalid, if any. This will be set
	///   to null if the value is valid and will be a non-empty string if the
	///   value is invalid.
	/// </param>
	/// <returns>True if the parameter is valid.</returns>
	public bool Validate(string value, out string? errorMessage)
	{
		if (!OrderedValues.Contains(value))
		{
			errorMessage = $"Invalid value '{value}'. Allowed values are: " +
				string.Join(", ", OrderedValues);
			return false;
		}

		errorMessage = null;
		return true;
	}
}
