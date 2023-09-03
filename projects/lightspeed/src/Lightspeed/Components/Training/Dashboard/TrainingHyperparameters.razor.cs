/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Models;
namespace Lightspeed.Components.Training.Dashboard;

/// <summary>
/// Component that displays the hyperparameters used by a training session.
/// </summary>
public partial class TrainingHyperparameters : ComponentBase
{
	/// <summary>
	/// Generic hyperparameters used for the training session.
	/// </summary>
	[Parameter]
	public Hyperparameters GenericHyperparameters { get; set; } = null!;

	/// <summary>
	/// Model-specific hyperparameters used for the training session.
	/// </summary>
	[Parameter]
	public IReadOnlyDictionary<string, string> ModelHyperparameters
	{
		get;
		set;
	} = new Dictionary<string, string>();
}
