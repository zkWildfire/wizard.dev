/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Models;
namespace Lightspeed.Services.Models;

/// <summary>
/// Service that provides access to available models.
/// </summary>
public interface IModelsService
{
	/// <summary>
	/// All available models, indexed by model ID.
	/// </summary>
	IReadOnlyDictionary<string, IClassificationModel> Models { get; }
}
