/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Training;
using Lightspeed.Services.Training;
namespace Lightspeed.Pages.Training;

/// <summary>
/// Page that displays status information about a training session.
/// This page handles both completed and active training sessions.
/// </summary>
public partial class TrainingSessionDashboard : ComponentBase
{
	/// <summary>
	/// ID of the training session that this page displays.
	/// </summary>
	[Parameter]
	public Guid SessionId { get; set; } = Guid.Empty;

	/// <summary>
	/// Navigation manager for the app.
	/// </summary>
	[Inject]
	private NavigationManager NavigationManager { get; set; } = null!;

	/// <summary>
	/// Service that manages all training sessions.
	/// </summary>
	[Inject]
	private IHostedTrainingService TrainingService { get; set; } = null!;

	/// <summary>
	/// Training session that this page displays.
	/// </summary>
	private ITrainingSession _trainingSession = null!;

	/// <summary>
	/// Initializes the page.
	/// </summary>
	protected override void OnInitialized()
	{
		base.OnInitialized();

		// Get the training session for this page
		if (!TrainingService.Sessions.TryGetValue(SessionId, out var session))
		{
			NavigationManager.NavigateTo("/");
			return;
		}
		_trainingSession = session;
	}
}
