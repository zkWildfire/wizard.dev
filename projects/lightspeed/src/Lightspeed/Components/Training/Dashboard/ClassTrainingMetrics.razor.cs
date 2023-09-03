/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Training;
using Lightspeed.Components.Utils;
namespace Lightspeed.Components.Training.Dashboard;

/// <summary>
/// Component that displays the class-specific training metrics for a model.
/// </summary>
public partial class ClassTrainingMetrics : ComponentBase
{
	/// <summary>
	/// Session that the component displays the progress of.
	/// </summary>
	[Parameter]
	public ITrainingSession TrainingSession { get; set; } = null!;

	/// <summary>
	/// Index of the class that the component should display metrics for.
	/// </summary>
	[Parameter]
	public int ClassIndex { get; set; }

	/// <summary>
	/// Name to display for the classification class.
	/// </summary>
	[Parameter]
	public string ClassName { get; set; } = string.Empty;

	/// <summary>
	/// Chart that displays the model's overall true positives rate over time.
	/// </summary>
	private LineGraph TruePositivesGraph { get; set; } = null!;

	/// <summary>
	/// Chart that displays the model's overall false positives rate over time.
	/// </summary>
	private LineGraph FalsePositivesGraph { get; set; } = null!;

	/// <summary>
	/// Chart that displays the model's overall true negatives rate over time.
	/// </summary>
	private LineGraph TrueNegativesGraph { get; set; } = null!;

	/// <summary>
	/// Chart that displays the model's overall false negatives rate over time.
	/// </summary>
	private LineGraph FalseNegativesGraph { get; set; } = null!;

	/// <summary>
	/// Chart that displays the model's overall precision over time.
	/// </summary>
	private LineGraph PrecisionGraph { get; set; } = null!;

	/// <summary>
	/// Chart that displays the model's overall recall over time.
	/// </summary>
	private LineGraph RecallGraph { get; set; } = null!;

	/// <summary>
	/// Chart that displays the model's overall F1 score over time.
	/// </summary>
	private LineGraph F1Graph { get; set; } = null!;

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
	/// Labels displayed for each line in the true positives graph.
	/// </summary>
	private readonly IReadOnlyList<string> _tpGraphLabels =
		GetLabels("True Positive Rate");

	/// <summary>
	/// Labels displayed for each line in the false positives graph.
	/// </summary>
	private readonly IReadOnlyList<string> _fpGraphLabels =
		GetLabels("False Positive Rate");

	/// <summary>
	/// Labels displayed for each line in the true negatives graph.
	/// </summary>
	private readonly IReadOnlyList<string> _tnGraphLabels =
		GetLabels("True Negative Rate");

	/// <summary>
	/// Labels displayed for each line in the false negatives graph.
	/// </summary>
	private readonly IReadOnlyList<string> _fnGraphLabels =
		GetLabels("False Negative Rate");

	/// <summary>
	/// Labels displayed for each line in the precision graph.
	/// </summary>
	private readonly IReadOnlyList<string> _precisionGraphLabels =
		GetLabels("Precision");

	/// <summary>
	/// Labels displayed for each line in the recall graph.
	/// </summary>
	private readonly IReadOnlyList<string> _recallGraphLabels =
		GetLabels("Recall");

	/// <summary>
	/// Labels displayed for each line in the F1 graph.
	/// </summary>
	private readonly IReadOnlyList<string> _f1GraphLabels =
		GetLabels("F1 Score");

	/// <summary>
	/// Metrics that are currently being displayed by the component.
	/// </summary>
	/// <remarks>
	/// This class uses a concurrent queue instead of the `IReadOnlyList` used
	///   by the general training metrics component since the user could click
	///   on a new class right when the model completes an epoch. In this case,
	///   the training thread would be updating the metrics list at the same
	///   time that the UI thread is trying to read from it.
	/// </remarks>
	private ConcurrentQueue<MetricsSnapshot> _currentMetrics = new();

	/// <summary>
	/// Epoch that the graphs were last updated at.
	/// This is used to ensure that even if the training thread triggers an
	///   update at the same time the user is switching classes, the graphs
	///   will show the latest data.
	/// </summary>
	/// <remarks>
	/// Updates are always run on the UI thread, so it isn't necessary to use
	///   a lock or any other synchronization mechanism to access this field.
	/// </remarks>
	private int _lastUpdateEpoch = -1;

	/// <summary>
	/// Index of the class that the graphs were last updated for.
	/// This is used to ensure that even if the training thread triggers an
	///   update at the same time the user is switching classes, the graphs
	///   will show the latest data.
	/// </summary>
	/// <remarks>
	/// Updates are always run on the UI thread, so it isn't necessary to use
	///   a lock or any other synchronization mechanism to access this field.
	/// </remarks>
	private int _lastClassIndex = -1;

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
	/// Updates the component in response to parameters changing.
	/// </summary>
	/// <returns>
	/// A task set once the component has been updated.
	/// </returns>
	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync()
			.ConfigureAwait(true);

		// If this is the first time the parameters are getting set, then the
		//   graph references may not have been set yet
		if (TruePositivesGraph == null)
		{
			return;
		}

		await UpdateGraphs().ConfigureAwait(true);
	}

	/// <summary>
	/// Updates the component's internal state.
	/// Callers are required to *not* hold the component's lock when calling
	///   this method.
	/// </summary>
	/// <param name="metrics">
	/// List of all metrics that have been recorded by the model.
	/// </param>
	private Task UpdateState(IReadOnlyList<MetricsSnapshot> metrics)
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
				return Task.CompletedTask;
			}
			_currentMetrics = new ConcurrentQueue<MetricsSnapshot>(metrics);
		}

		// It *should* be fine to run this outside a `lock()` block since
		//   the model should not be completing epochs so quickly that this
		//   code doesn't get a chance to finish executing before the next
		//   epoch completes. Having this code outside the `lock()` block is
		//   necessary to allow async calls to Blazor Charts methods to be
		//   made.
		return InvokeAsync(UpdateGraphs);
	}

	/// <summary>
	/// Updates each graph to the latest data.
	/// This method *must* be called from the UI thread.
	/// </summary>
	private async Task UpdateGraphs()
	{
		// Check if the graphs need to be updated
		var shouldUpdate = false;
		var currentEpoch = _currentMetrics.Count;
		if (_lastUpdateEpoch < _currentMetrics.Count)
		{
			shouldUpdate = true;
		}
		else if (_lastClassIndex != ClassIndex)
		{
			shouldUpdate = true;
		}
		if (!shouldUpdate)
		{
			return;
		}

		// Capture a snapshot of the metrics list to ensure that each graph
		//   gets the same data
		var metrics = _currentMetrics.ToList();

		// Get the method to use to update each graph
		Func<LineGraph, Func<ClassMetrics, double>, Task> updateFunc =
			_lastClassIndex != ClassIndex
				? (graph, selector) =>
					ReplaceGraph(metrics, ClassIndex, graph, selector)
				: (graph, selector) =>
					UpdateGraph(metrics, ClassIndex, graph, selector);

		var tasks = new List<Task>()
		{
			updateFunc(
				TruePositivesGraph,
				m => m.TruePositiveRate * 100.0
			),
			updateFunc(
				FalsePositivesGraph,
				m => m.FalsePositiveRate * 100.0
			),
			updateFunc(
				TrueNegativesGraph,
				m => m.TrueNegativeRate * 100.0
			),
			updateFunc(
				FalseNegativesGraph,
				m => m.FalseNegativeRate * 100.0
			),
			updateFunc(
				PrecisionGraph,
				m => m.Precision * 100.0
			),
			updateFunc(
				RecallGraph,
				m => m.Recall * 100.0
			),
			updateFunc(
				F1Graph,
				m => m.F1Score * 100.0
			)
		};
		foreach (var task in tasks)
		{
			await task.ConfigureAwait(true);
		}

		// Update internal state to reflect this update
		// Note that `_lastUpdateEpoch` is intentionally updated from the local
		//   variable that was captured at the start of this method. This is
		//   to ensure that if two calls to this method are made at around the
		//   same time, the second call will correctly call into the update
		//   methods again if necessary.
		_lastUpdateEpoch = currentEpoch;
		_lastClassIndex = ClassIndex;

		// This could be called from the training thread, so it has to be
		//   invoked using `InvokeAsync()`
		_ = InvokeAsync(StateHasChanged);
	}

	/// <summary>
	/// Replaces a graph's data with new data.
	/// This method must be used when switching between classes, since the
	///   existing data in the graph must be replaced.
	/// </summary>
	/// <param name="metrics">
	/// List of metrics to use to update the graph.
	/// </param>
	/// <param name="index">
	/// Index of the class metrics to display.
	/// </param>
	/// <param name="graph">
	/// Graph to update.
	/// </param>
	/// <param name="selector">
	/// Functor that selects the data to add to the graph from the metrics.
	/// </param>
	/// <returns>
	/// A task set once the graph has been updated.
	/// </returns>
	private static Task ReplaceGraph(
		IReadOnlyList<MetricsSnapshot> metrics,
		int index,
		LineGraph graph,
		Func<ClassMetrics, double> selector)
	{
		// Lists of data for each line in the graph
		var data = new List<List<double>>()
		{
			// List for all training data
			new List<double>(),
			// List for all validation data
			new List<double>()
		};

		for (var i = 0; i < metrics.Count; i++)
		{
			var metric = metrics[i];
			data[0].Add(selector(metric.TrainingMetrics.ClassMetrics[index]));
			data[1].Add(selector(metric.ValidationMetrics.ClassMetrics[index]));
		}

		// Update the graph
		return graph.ReplaceDataAsync(data);
	}

	/// <summary>
	/// Updates a graph with the new metrics.
	/// </summary>
	/// <param name="metrics">
	/// List of metrics to use to update the graph.
	/// </param>
	/// <param name="index">
	/// Index of the class metrics to display.
	/// </param>
	/// <param name="graph">
	/// Graph to update.
	/// </param>
	/// <param name="selector">
	/// Functor that selects the data to add to the graph from the metrics.
	/// </param>
	/// <returns>
	/// A task set once the graph has been updated.
	/// </returns>
	private static async Task UpdateGraph(
		IReadOnlyList<MetricsSnapshot> metrics,
		int index,
		LineGraph graph,
		Func<ClassMetrics, double> selector)
	{
		// Figure out how many new data points need to be added
		var graphDataPointsCount = graph.DataPointsCount;
		var newMetrics = metrics.Skip(graphDataPointsCount);

		foreach (var metric in newMetrics)
		{
			// Generate the list of data to add to the graph
			var data = new List<double>()
			{
				selector(metric.TrainingMetrics.ClassMetrics[index]),
				selector(metric.ValidationMetrics.ClassMetrics[index])
			};

			// Update the graph
			await graph.AddDataAsync(
				$"Epoch {metric.CurrentEpoch}",
				data
			).ConfigureAwait(true);
		}
	}

	/// <summary>
	/// Gets the labels to use for the graph's lines.
	/// </summary>
	/// <param name="metric">
	/// Metric that the graph is displaying.
	/// </param>
	/// <returns>
	/// A list of labels that the graph should use.
	/// </returns>
	private static IReadOnlyList<string> GetLabels(string metric)
	{
		return new List<string>()
		{
			$"Training {metric}",
			$"Validation {metric}"
		};
	}
}
