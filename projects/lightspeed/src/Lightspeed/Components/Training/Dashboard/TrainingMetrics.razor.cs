/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Training;
using Lightspeed.Components.Utils;
namespace Lightspeed.Components.Training.Dashboard;

/// <summary>
/// Component that displays the training metrics for a model.
/// </summary>
public partial class TrainingMetrics : ComponentBase
{
	/// <summary>
	/// Session that the component displays the progress of.
	/// </summary>
	[Parameter]
	public ITrainingSession TrainingSession { get; set; } = null!;

	/// <summary>
	/// Graph that displays the model's accuracy over time.
	/// </summary>
	private LineGraph AccuracyGraph { get; set; } = null!;

	/// <summary>
	/// Chart that displays the model's loss over time.
	/// </summary>
	private LineGraph LossGraph { get; set; } = null!;

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
	/// Labels displayed for each line in the accuracy graph.
	/// </summary>
	private readonly IReadOnlyList<string> _accuracyGraphLabels =
		new List<string>()
		{
			"Training Accuracy",
			"Validation Accuracy"
		};

	/// <summary>
	/// Labels displayed for each line in the loss graph.
	/// </summary>
	private readonly IReadOnlyList<string> _lossGraphLabels =
		new List<string>()
		{
			"Training Loss",
			"Validation Loss"
		};

	/// <summary>
	/// Metrics that are currently being displayed by the component.
	/// </summary>
	private IReadOnlyList<MetricsSnapshot> _currentMetrics =
		new List<MetricsSnapshot>();

	/// <summary>
	/// Initializes the component.
	/// </summary>
	/// <param name="firstRender">
	/// Whether or not this is the first time that the component is being
	///   rendered.
	/// </param>
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender)
			.ConfigureAwait(true);
		if (!firstRender)
		{
			return;
		}

		// Bind to the training session's `OnEpochComplete` event so that the
		//   component updates over time
		TrainingSession.OnEpochComplete +=
			async (_, args) => await UpdateState(TrainingSession.Metrics)
				.ConfigureAwait(true);

		// Force an update of the component's internal state so that it matches
		//   the training session state immediately, rather than only matching
		//   after the first epoch completion event is fired
		await UpdateState(TrainingSession.Metrics)
			.ConfigureAwait(true);
	}

	/// <summary>
	/// Updates the component's internal state.
	/// Callers are required to *not* hold the component's lock when calling
	///   this method.
	/// </summary>
	/// <param name="metrics">
	/// List of all metrics that have been recorded by the model.
	/// </param>
	private async Task UpdateState(IReadOnlyList<MetricsSnapshot> metrics)
	{
		lock (_lock)
		{
			// When this component is first being initialized, it's possible
			//   that this method could be invoked by the thread running the
			//   training session at the same time that the UI thread calls
			//   into this component. If that happens, make sure that only the
			//   call with the higher value causes a component update.
			if (_currentMetrics.Count > metrics.Count)
			{
				return;
			}
			_currentMetrics = metrics;
		}

		// It *should* be fine to run this outside a `lock()` block since
		//   the model should not be completing epochs so quickly that this
		//   code doesn't get a chance to finish executing before the next
		//   epoch completes. Having this code outside the `lock()` block is
		//   necessary to allow async calls to Blazor Charts methods to be
		//   made.
		var accuracyTask = UpdateAccuracyGraph();
		var lossTask = UpdateLossGraph();
		await accuracyTask.ConfigureAwait(true);
		await lossTask.ConfigureAwait(true);

		// This could be called from the training thread, so it has to be
		//   invoked using `InvokeAsync()`
		_ = InvokeAsync(StateHasChanged);
	}

	/// <summary>
	/// Updates the accuracy graph to match the current stored metrics.
	/// </summary>
	/// <returns>
	/// A task set once the graph has been updated.
	/// </returns>
	private Task UpdateAccuracyGraph()
	{
		return UpdateGraph(
			AccuracyGraph,
			new List<Func<MetricsSnapshot, double>>()
			{
				m => m.TrainingMetrics.Accuracy * 100.0,
				m => m.ValidationMetrics.Accuracy * 100.0
			}
		);
	}

	/// <summary>
	/// Updates the loss graph to match the current stored metrics.
	/// </summary>
	/// <returns>
	/// A task set once the graph has been updated.
	/// </returns>
	private Task UpdateLossGraph()
	{
		return UpdateGraph(
			LossGraph,
			new List<Func<MetricsSnapshot, double>>()
			{
				m => m.TrainingMetrics.Loss,
				m => m.ValidationMetrics.Loss
			}
		);
	}

	/// <summary>
	/// Updates a graph with the new metrics.
	/// </summary>
	/// <param name="graph">
	/// Graph to update.
	/// </param>
	/// <param name="selectors">
	/// List of functions that select the data to add to the graph from the
	///   metric objects. These functions must be specified in the same order
	///   as the line labels in the graph, e.g. the data returned by
	///   `selectors[n]` will be used to update the line with index `n` in the
	///   graph.
	/// </param>
	/// <returns>
	/// A task set once the graph has been updated.
	/// </returns>
	private async Task UpdateGraph(
		LineGraph graph,
		IReadOnlyList<Func<MetricsSnapshot, double>> selectors)
	{
		// Figure out how many new data points need to be added
		var currentDataPointsCount = _currentMetrics.Count;
		var graphDataPointsCount = graph.DataPointsCount;
		var metrics = _currentMetrics.Skip(graphDataPointsCount);

		foreach (var metric in metrics)
		{
			// Generate the list of data to add to the graph
			var data = selectors.Select(
				f => f(metric)
			).ToList();

			// Update the graph
			await graph.AddDataAsync(
				$"Epoch {metric.CurrentEpoch}",
				data
			).ConfigureAwait(true);
		}
	}
}
