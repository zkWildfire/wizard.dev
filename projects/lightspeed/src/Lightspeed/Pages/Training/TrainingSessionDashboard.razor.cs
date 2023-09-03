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
	/// CSS classes to apply to all pagination elements.
	/// </summary>
	private const string PAGINATION_COMMON_CSS = "page-link rounded-0";

	/// <summary>
	/// CSS classes to apply to the active pagination element.
	/// </summary>
	private const string PAGINATION_ACTIVE_CSS = "bg-primary text-light";

	/// <summary>
	/// CSS classes to apply to inactive pagination elements.
	/// </summary>
	private const string PAGINATION_INACTIVE_CSS = "bg-dark";

	/// <summary>
	/// Number of classes in the dataset.
	/// </summary>
	private int ClassCount => _trainingSession.ClassCount;

	/// <summary>
	/// Training session that this page displays.
	/// </summary>
	private ITrainingSession _trainingSession = null!;

	/// <summary>
	/// Index of the class to display class-specific metrics for.
	/// </summary>
	private int _selectedClassIndex;

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

	/// <summary>
	/// Gets the CSS classes to use for the pagination element.
	/// </summary>
	/// <param name="index">
	/// Pagination page index that the element is for.
	/// </param>
	/// <returns>The CSS classes to use for the element.</returns>
	private string GetPaginationCss(int index)
	{
		var buttonCss = index == _selectedClassIndex
			? PAGINATION_ACTIVE_CSS
			: PAGINATION_INACTIVE_CSS;
		return $"{PAGINATION_COMMON_CSS} {buttonCss}";
	}

	/// <summary>
	/// Callback invoked when the user selects a class to display metrics for.
	/// </summary>
	/// <param name="index">
	/// Index of the class to display metrics for.
	/// </param>
	private void OnClassSelected(int index)
	{
		_selectedClassIndex = index;
		StateHasChanged();
	}
}
