using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Lightspeed.Pages;

/// <summary>
/// Page model for the error page.
/// </summary>
[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
[IgnoreAntiforgeryToken]
public class ErrorModel : PageModel
{
	/// <summary>
	/// Request ID that caused the error.
	/// </summary>
	public string? RequestId { get; set; }

	/// <summary>
	/// Whether or not the request ID should be shown.
	/// </summary>
	public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

	/// <summary>
	/// Logger to write to.
	/// </summary>
	private readonly ILogger<ErrorModel> _logger;

	/// <summary>
	/// Initializes the error page model.
	/// </summary>
	/// <param name="logger">Logger to write to.</param>
	public ErrorModel(ILogger<ErrorModel> logger)
	{
		_logger = logger;
	}

	/// <summary>
	/// Called when the page is requested.
	/// </summary>
	public void OnGet()
	{
		RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
	}
}
