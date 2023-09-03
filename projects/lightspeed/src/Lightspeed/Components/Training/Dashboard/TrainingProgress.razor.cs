/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Training;
namespace Lightspeed.Components.Training.Dashboard;

/// <summary>
/// Component that displays the progress of a training session.
/// </summary>
public partial class TrainingProgress : ComponentBase
{
	/// <summary>
	/// Session that the component displays the progress of.
	/// </summary>
	[Parameter]
	public ITrainingSession TrainingSession { get; set; } = null!;

	/// <summary>
	/// State data for the component.
	/// This is stored in a struct so that the entire set of state data can be
	///   easily captured in a local variable when generating the component's
	///   HTML. This ensures that even if the training session completes an
	///   epoch while the component is being rendered, the component will not
	///   display the wrong state.
	/// </summary>
	private readonly record struct State
	{
		/// <summary>
		/// Whether or not the training session is currently running.
		/// </summary>
		/// <remarks>
		/// This is implemented by checking `CurrentEpoch` and `TotalEpoch`
		///   instead of the training session properties to avoid having some
		///   edge case where a race condition causes the component to display
		///   the wrong state.
		/// </remarks>
		public bool IsActive => CurrentEpoch < TotalEpochs;

		/// <summary>
		/// Total number of epochs that will be completed.
		/// </summary>
		public required int TotalEpochs { get; init; }

		/// <summary>
		/// Epoch that the training session is currently on.
		/// This will be a value in the range `(0, TotalEpochs]`.
		/// </summary>
		/// <remarks>
		/// This is implemented as a property so that its naming convention matches
		///   the naming convention of the `TotalEpochs` property. The only reason
		///   for this is to avoid having code that looks like
		///   `_currentEpoch &lt; TotalEpochs` in the component.
		/// </remarks>
		public required int CurrentEpoch { get; init; }

		/// <summary>
		/// Time since the training session started.
		/// </summary>
		public required TimeSpan ElapsedTime { get; init; }

		/// <summary>
		/// Estimated time remaining for the training session.
		/// </summary>
		public required TimeSpan EstimatedTimeRemaining { get; init; }

		/// <summary>
		/// CSS classes to apply to the progress bar.
		/// </summary>
		public string ProgressBarCss => IsActive
			? $"{PROGRESS_BAR_CSS_COMMON} {PROGRESS_BAR_CSS_ACTIVE}"
			: $"{PROGRESS_BAR_CSS_COMMON} {PROGRESS_BAR_CSS_INACTIVE}";

		/// <summary>
		/// Width, in percent, to use for the progress bar.
		/// </summary>
		public double ProgressBarWidth =>
			Math.Round((double)CurrentEpoch / TotalEpochs * 100.0, 2);
	}

	/// <summary>
	/// Lock used to synchronize access to the component's internal state.
	/// In general, this lock will only be acquired by the training thread as
	///   a result of broadcasting to the `OnEpochComplete` event. However,
	///   when the component is being created, the thread initializing the
	///   dashboard components will also call into this component to set the
	///   initial state. Since it's possible that the training thread will
	///   also call into this component at the same time, this lock is required
	///   to synchronize access to the component's internal state.
	/// </summary>
	private readonly object _lock = new();

	/// <summary>
	/// CSS classes always applied to the progress bar.
	/// </summary>
	private const string PROGRESS_BAR_CSS_COMMON =
		"progress-bar progress-bar-striped";

	/// <summary>
	/// CSS classes applied to the progress bar when the training session is
	///   active.
	/// </summary>
	private const string PROGRESS_BAR_CSS_ACTIVE =
		"bg-primary progress-bar-animated";

	/// <summary>
	/// CSS classes applied to the progress bar when the training session is
	///   inactive.
	/// </summary>
	private const string PROGRESS_BAR_CSS_INACTIVE = "bg-success";

	/// <summary>
	/// Time at which the training session was started.
	/// </summary>
	private string StartTime => ToDateString(TrainingSession.StartTime);

	/// <summary>
	/// Time at which the training session ended.
	/// </summary>
	private string EndTime => TrainingSession.EndTime.HasValue
		? ToDateString(TrainingSession.EndTime.Value)
		: "N/A";

	/// <summary>
	/// State data for the component.
	/// </summary>
	private State _state = new()
	{
		TotalEpochs = 0,
		CurrentEpoch = 0,
		ElapsedTime = TimeSpan.Zero,
		EstimatedTimeRemaining = TimeSpan.Zero
	};

	/// <summary>
	/// Initializes the component.
	/// </summary>
	protected override void OnInitialized()
	{
		base.OnInitialized();

		// Bind to the training session's `OnEpochComplete` event so that the
		//   component updates over time
		TrainingSession.OnEpochComplete +=
			(_, args) => UpdateState(args.ToMetricsSnapshot());

		// Force an update of the component's internal state so that it matches
		//   the training session state immediately, rather than only matching
		//   after the first epoch completion event is fired
		UpdateState(TrainingSession.CurrentMetrics);
	}

	/// <summary>
	/// Updates the component's internal state.
	/// Callers are required to *not* hold the component's lock when calling
	///   this method.
	/// </summary>
	/// <param name="data">
	/// Snapshot from the training session's most recently completed epoch.
	/// </param>
	private void UpdateState(MetricsSnapshot data)
	{
		lock (_lock)
		{
			// When this component is first being initialized, it's possible
			//   that this method could be invoked by the thread running the
			//   training session at the same time that the UI thread calls
			//   into this component. If that happens, make sure that only the
			//   call with the higher value causes a component update.
			if (_state.CurrentEpoch > data.CurrentEpoch)
			{
				return;
			}

			var remainingEpochs = data.TotalEpochs - data.CurrentEpoch;
			_state = new()
			{
				TotalEpochs = data.TotalEpochs,
				CurrentEpoch = data.CurrentEpoch,
				ElapsedTime = data.TotalDuration,
				EstimatedTimeRemaining =
					data.AverageEpochDuration * remainingEpochs
			};

			// This could be called from the training thread, so it has to be
			//   invoked using `InvokeAsync()`
			_ = InvokeAsync(StateHasChanged);
		}
	}

	/// <summary>
	/// Converts a `DateTime` object to a string.
	/// </summary>
	/// <param name="dateTime">
	/// `DateTime` object to convert.
	/// </param>
	/// <returns>
	/// The string representation of the `DateTime` object.
	/// </returns>
	private static string ToDateString(DateTime dateTime)
	{
		var localTime = dateTime.ToLocalTime();
		var timeZone = TimeZoneInfo.Local;
		return localTime.ToString(CultureInfo.CurrentCulture) + " " +
			$"({timeZone.StandardName})";
	}
}
