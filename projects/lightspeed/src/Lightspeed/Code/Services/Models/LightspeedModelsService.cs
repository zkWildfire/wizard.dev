/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Models;
using Lightspeed.Classification.Models.Simple;
namespace Lightspeed.Services.Models;

/// <summary>
/// Default implementation of the models service.
/// </summary>
public class LightspeedModelsService : IModelsService
{
	/// <summary>
	/// All available models, indexed by model ID.
	/// </summary>
	public IReadOnlyDictionary<string, IClassificationModel> Models { get; }

	/// <summary>
	/// Initializes the service.
	/// </summary>
	public LightspeedModelsService()
	{
		Models = new Dictionary<string, IClassificationModel>
		{
			{ SimpleModel.MODEL_ID, new SimpleModel() }
		};
	}
}
