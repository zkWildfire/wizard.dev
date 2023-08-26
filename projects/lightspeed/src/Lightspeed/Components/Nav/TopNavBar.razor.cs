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

	/// <summary>
	/// Navigation manager for the application.
	/// </summary>
	[Inject]
	private NavigationManager NavManager { get; set; } = null!;

	/// <summary>
	/// Checks whether the button is the active button.
	/// </summary>
	/// <param name="buttonAddress">Address that the button is for.</param>
	/// <returns>True if the button is the active button.</returns>
	private bool IsActiveButton(Uri buttonAddress)
	{
		// Handle the root address case separately
		if (buttonAddress == new Uri("/", UriKind.Relative))
		{
			return NavManager.Uri == NavManager.BaseUri;
		}

		// For all other pages, check if the button's address is a prefix of the
		//   current address
		var buttonUri = NavManager.BaseUri.TrimEnd('/') + buttonAddress;
		return NavManager.Uri.StartsWith(
			buttonUri,
			StringComparison.OrdinalIgnoreCase
		);
	}
}
