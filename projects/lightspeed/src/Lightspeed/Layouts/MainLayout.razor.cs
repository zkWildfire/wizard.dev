/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Components.Nav;
namespace Lightspeed.Layouts;

/// <summary>
/// Component that handles the default layout of the application.
/// </summary>
public partial class MainLayout : LayoutComponentBase
{
	/// <summary>
	/// Data for the buttons to display on the top navigation bar.
	/// </summary>
	private readonly IReadOnlyList<NavButtonData> _navButtons;

	/// <summary>
	/// Initializes a new instance of the class.
	/// </summary>
	public MainLayout()
	{
		_navButtons = new List<NavButtonData>
		{
			new NavButtonData
			{
				Text = "Dashboard",
				Address = new Uri("/", UriKind.Relative)
			},
			new NavButtonData
			{
				Text = "Datasets",
				Address = new Uri("/datasets", UriKind.Relative)
			},
			new NavButtonData
			{
				Text = "Models",
				Address = new Uri("/models", UriKind.Relative)
			},
			new NavButtonData
			{
				Text = "Playground",
				Address = new Uri("/playground", UriKind.Relative)
			}
		};
	}
}
