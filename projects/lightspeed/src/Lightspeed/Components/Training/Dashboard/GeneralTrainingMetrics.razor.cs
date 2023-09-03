/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Lightspeed.Classification.Training;
using Lightspeed.Components.Utils;
namespace Lightspeed.Components.Training.Dashboard;

/// <summary>
/// Component that displays the overall training metrics for a model.
/// </summary>
public partial class GeneralTrainingMetrics : ComponentBase
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
	/// Labels displayed for each line in the accuracy graph.
	/// </summary>
	private readonly IReadOnlyList<string> _accuracyGraphLabels =
		GetLabels("Accuracy");

	/// <summary>
	/// Labels displayed for each line in the loss graph.
	/// </summary>
	private readonly IReadOnlyList<string> _lossGraphLabels =
		GetLabels("Loss");

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
		var tasks = new List<Task>()
		{
			UpdateGraph(
				AccuracyGraph,
				m => m.Accuracy * 100.0
			),
			UpdateGraph(
				LossGraph,
				m => m.Loss * 1000 / m.Count
			),
			UpdateGraph(
				TruePositivesGraph,
				m => m.TruePositiveRate * 100.0
			),
			UpdateGraph(
				FalsePositivesGraph,
				m => m.FalsePositiveRate * 100.0
			),
			UpdateGraph(
				TrueNegativesGraph,
				m => m.TrueNegativeRate * 100.0
			),
			UpdateGraph(
				FalseNegativesGraph,
				m => m.FalseNegativeRate * 100.0
			),
			UpdateGraph(
				PrecisionGraph,
				m => m.Precision * 100.0
			),
			UpdateGraph(
				RecallGraph,
				m => m.Recall * 100.0
			),
			UpdateGraph(
				F1Graph,
				m => m.F1Score * 100.0
			)
		};
		foreach (var task in tasks)
		{
			await task.ConfigureAwait(true);
		}

		// This could be called from the training thread, so it has to be
		//   invoked using `InvokeAsync()`
		_ = InvokeAsync(StateHasChanged);
	}

	/// <summary>
	/// Updates a graph with the new metrics.
	/// </summary>
	/// <param name="graph">
	/// Graph to update.
	/// </param>
	/// <param name="selector">
	/// Functor that selects the data to add to the graph from the metrics.
	/// </param>
	/// <returns>
	/// A task set once the graph has been updated.
	/// </returns>
	private async Task UpdateGraph(
		LineGraph graph,
		Func<ModelMetrics, double> selector)
	{
		// Figure out how many new data points need to be added
		var graphDataPointsCount = graph.DataPointsCount;
		var metrics = _currentMetrics.Skip(graphDataPointsCount);

		foreach (var metric in metrics)
		{
			// Generate the list of data to add to the graph
			var data = new List<double>()
			{
				selector(metric.TrainingMetrics),
				selector(metric.ValidationMetrics)
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
