/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Lightspeed.Components.Utils;

/// <summary>
/// Component that displays a pagination bar.
/// </summary>
public partial class PaginationBar : ComponentBase
{
	/// <summary>
	/// Pagination index of the current page.
	/// </summary>
	[Parameter]
	public int Index { get; set; }

	/// <summary>
	/// Total number of pages.
	/// </summary>
	[Parameter]
	public int Count { get; set; }

	/// <summary>
	/// Number of buttons to display in the pagination bar.
	/// </summary>
	/// <remarks>
	/// This does not include the first/last and previous/next buttons.
	/// </remarks>
	[Parameter]
	public int Width { get; set; }

	/// <summary>
	/// URL to use for pagination links.
	/// The pagination component will generate links in the form `RootUrl/Index`.
	/// </summary>
	[Parameter]
	public Uri RootUrl { get; set; } = new Uri("/", UriKind.Relative);

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
	/// Pagination index of the previous page.
	/// </summary>
	/// <remarks>
	/// This is only used when the current page is not the first page, so it
	///   doesn't need to handle the case where `_pageIndex - 1` is invalid.
	/// </remarks>
	private int PreviousPaginationIndex => Index - 1;

	/// <summary>
	/// Pagination index of the next page.
	/// </summary>
	/// <remarks>
	/// This is only used when the current page is not the last page, so it
	///   doesn't need to handle the case where `_pageIndex + 1` is invalid.
	/// </remarks>
	private int NextPaginationIndex => Index + 1;

	/// <summary>
	/// Index of the first page to list on the pagination bar.
	/// </summary>
	/// <remarks>
	/// This will always be in the range `[0, Count)`.
	/// </remarks>
	private int _startIndex;

	/// <summary>
	/// Index of the past-the-end page index for the pagination bar.
	/// </summary>
	/// <remarks>
	/// This will always be in the range `(0, Count]`.
	/// </remarks>
	private int _endIndex;

	/// <summary>
	/// Updates pagination elements.
	/// </summary>
	protected override void OnParametersSet()
	{
		base.OnParametersSet();
		var halfBarSize = Width / 2;

		// If the current page is near the start of the dataset, the page
		//   will not be centered on the pagination bar
		if (Index < halfBarSize)
		{
			_startIndex = 0;
			_endIndex = Math.Min(Width, Count);
		}
		// If the current page is near the end of the dataset, the page
		//   will not be centered on the pagination bar
		else if (Index >= Count - halfBarSize)
		{
			_startIndex = Math.Max(0, Count - Width);
			_endIndex = Count;
		}
		else
		{
			_startIndex = Index - halfBarSize;
			_endIndex = Index + halfBarSize;
		}
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
		var buttonCss = index == Index
			? PAGINATION_ACTIVE_CSS
			: PAGINATION_INACTIVE_CSS;
		return $"{PAGINATION_COMMON_CSS} {buttonCss}";
	}
}
