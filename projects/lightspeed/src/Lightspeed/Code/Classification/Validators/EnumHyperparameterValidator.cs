/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Lightspeed.Classification.Validators;

/// <summary>
/// Hyperparameter validator used for enum hyperparameters.
/// </summary>
public class EnumHyperparameterValidator<T>
	: StringSetHyperparameterValidator
	where T : struct, Enum
{
	/// <summary>
	/// Map of enum display names to their corresponding enum values.
	/// </summary>
	public IReadOnlyDictionary<string, T> EnumValues { get; }

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
	/// <param name="enumValues">
	/// List of valid enum values and their display names. Only enum values
	///   specified in this list will be considered valid. If an enum value
	///   appears more than once in this list, the display name provided by
	///   the last entry will be used. Must contain at least one value.
	/// </param>
	/// <param name="constraints">
	/// Description to display for constraints on the hyperparameter.
	/// </param>
	/// <param name="isOptional">
	/// Whether the hyperparameter is optional.
	/// </param>
	public EnumHyperparameterValidator(
		string id,
		string displayName,
		string description,
		IReadOnlyList<Tuple<T, string>> enumValues,
		string? constraints = null,
		bool isOptional = false)
		: base(
			id,
			displayName,
			description,
			new List<string>(enumValues.Select(x => x.Item2)),
			typeof(T),
			constraints,
			enumValues[0].Item2,
			isOptional
		)
	{
		EnumValues = new Dictionary<string, T>(
			enumValues.Select(x => new KeyValuePair<string, T>(x.Item2, x.Item1))
		);
	}
}
