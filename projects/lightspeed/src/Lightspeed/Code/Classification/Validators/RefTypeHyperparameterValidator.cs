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
/// <typeparam name="T">
/// The type of the hyperparameter.
/// This may only be one of the following types:
/// - bool
/// - any integral or floating point type
/// - string
/// - any enum type
/// </typeparam>
public abstract class IRefTypeHyperparameterValidator<T>
	: IHyperparameterValidator
	where T : class
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
	public Type ParameterType => typeof(T);

	/// <summary>
	/// Function to invoke to parse the value specified on the UI.
	/// This function will be passed the value specified on the UI and should
	///   return the parsed value. It should not perform any validation of
	///   the value. If the value cannot be converted, it should throw an
	///   `ArgumentException`. If this occurs, the exception's message will
	///   be displayed to the user.
	/// </summary>
	private readonly Func<string, T> _parseFunc;

	/// <summary>
	/// Functions to invoke to validate the hyperparameter.
	/// Each function will be passed the value specified on the UI and should
	///   return `true` if the value is valid or `false` if the value is
	///   invalid.
	/// </summary>
	private readonly IReadOnlyList<Func<T, bool>> _constraintFuncs;

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
	/// <param name="parseFunc">
	/// Function to invoke to parse the value specified on the UI.
	/// This function will be passed the value specified on the UI and should
	///   return the parsed value. It should not perform any validation of
	///   the value. If the value cannot be converted, it should throw an
	///   `ArgumentException`. If this occurs, the exception's message will
	///   be displayed to the user.
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
	protected IRefTypeHyperparameterValidator(
		string id,
		string displayName,
		string description,
		Func<string, T> parseFunc,
		IReadOnlyList<Func<T, bool>>? constraintFuncs = null,
		string? constraints = null,
		T? defaultValue = null,
		bool isOptional = false)
	{
		Id = id;
		DisplayName = displayName;
		Description = description;
		Constraints = constraints;
		DefaultValue = defaultValue?.ToString();
		IsOptional = isOptional;
		_parseFunc = parseFunc;
		_constraintFuncs = constraintFuncs ?? new List<Func<T, bool>>();
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
		// Make sure the value is a valid integer
		var parsedValue = default(T);
		try
		{
			parsedValue = _parseFunc.Invoke(value);
		}
		catch (ArgumentException e)
		{
			errorMessage = e.Message;
			return false;
		}

		// Make sure the value is within the constraints
		foreach (var constraintFunc in _constraintFuncs)
		{
			if (!constraintFunc(parsedValue))
			{
				errorMessage = "Value is outside of the allowed range.";
				return false;
			}
		}

		errorMessage = null;
		return true;
	}
}
