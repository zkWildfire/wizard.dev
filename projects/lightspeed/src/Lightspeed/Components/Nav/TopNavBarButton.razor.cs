/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Lightspeed.Components.Nav;

/// <summary>
/// Event arguments for when a button's state changes.
/// </summary>
public class OnButtonStateChanged : EventArgs
{
	/// <summary>
	/// State the button is now in.
	/// </summary>
	public required bool NewState { get; init; }

	/// <summary>
	/// State the button was previously in.
	/// </summary>
	public bool OldState => !NewState;
}

/// <summary>
/// Component that handles a single button in the top navigation bar.
/// </summary>
public partial class TopNavBarButton : ComponentBase
{
	/// <summary>
	/// Event broadcast to when the button's state changes.
	/// </summary>
	public event EventHandler<OnButtonStateChanged>? OnButtonStateChanged;

	/// <summary>
	/// Event broadcast to when the button is clicked.
	/// </summary>
	public event EventHandler? OnClicked;

	/// <summary>
	/// Controls whether the button is displayed with active or inactive styling.
	/// </summary>
	[Parameter]
	public bool IsActive { get; set; }

	/// <summary>
	/// Text to display on the button.
	/// </summary>
	[Parameter]
	public string Text { get; set; } = string.Empty;

	/// <summary>
	/// Uri to navigate to when the button is clicked.
	/// </summary>
	[Parameter]
	public Uri Address { get; set; } = new Uri("/", UriKind.Relative);

	/// <summary>
	/// Css applied to the button in all states.
	/// </summary>
	[Parameter]
	public string CommonCss { get; set; } =
		"btn btn-outline-light border-0 rounded-0 text-light h-100";

	/// <summary>
	/// Css applied to the button when it is active.
	/// </summary>
	[Parameter]
	public string ActiveCss { get; set; } = string.Empty;

	/// <summary>
	/// Css applied to the button when it is inactive.
	/// </summary>
	[Parameter]
	public string InactiveCss { get; set; } = string.Empty;

	/// <summary>
	/// CSS classes to apply to the button.
	/// </summary>
	private string CurrentCss =>
		$"{CommonCss} {(IsActive ? ActiveCss : InactiveCss)}";
}
