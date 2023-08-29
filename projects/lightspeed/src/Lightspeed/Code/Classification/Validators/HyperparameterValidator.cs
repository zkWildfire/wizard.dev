/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Lightspeed.Classification.Validators;

/// <summary>
/// Interface for code that validates hyperparameters specified on the UI.
/// Hyperparameter validators are required to be stateless, allowing a single
///   validator instance to be used any number of times.
/// </summary>
public interface IHyperparameterValidator
{
	/// <summary>
	/// Model specific unique ID for the hyperparameter.
	/// </summary>
	string Id { get; }

	/// <summary>
	/// Name to display on the UI for the hyperparameter.
	/// </summary>
	string DisplayName { get; }

	/// <summary>
	/// Brief description to display on the UI for the hyperparameter.
	/// </summary>
	string Description { get; }

	/// <summary>
	/// Description of the constraints for the hyperparameter, if any.
	/// </summary>
	string? Constraints { get; }

	/// <summary>
	/// Optional default value to use for the hyperparameter.
	/// </summary>
	string? DefaultValue { get; }

	/// <summary>
	/// Whether the hyperparameter is optional.
	/// </summary>
	bool IsOptional { get; }

	/// <summary>
	/// Type of the hyperparameter.
	/// This may only be one of the following types:
	/// - bool
	/// - any integral or floating point type
	/// - string
	/// - any enum type
	/// </summary>
	Type ParameterType { get; }

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
	bool Validate(string value, out string? errorMessage);
}
