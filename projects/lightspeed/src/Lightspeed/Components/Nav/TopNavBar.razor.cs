/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Lightspeed.Components.Nav;

/// <summary>
/// Defines the data necessary to generate buttons on the top bar.
/// </summary>
public readonly record struct NavButtonData
{
	/// <summary>
	/// Text to display on the button.
	/// </summary>
	public required string Text { get; init; }

	/// <summary>
	/// Address to navigate to when the button is clicked.
	/// </summary>
	public required Uri Address { get; init; }
}

/// <summary>
/// Component that handles the top navigation bar in the default layout.
/// </summary>
public partial class TopNavBar : ComponentBase
{
	/// <summary>
	/// Buttons to display on the top navigation bar.
	/// </summary>
	[Parameter]
	public IReadOnlyList<NavButtonData> Buttons { get; set; } =
		new List<NavButtonData>();
}
