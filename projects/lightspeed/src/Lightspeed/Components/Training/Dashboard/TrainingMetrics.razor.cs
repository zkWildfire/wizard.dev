/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using BlazorBootstrap;
using Lightspeed.Classification.Training;
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
	/// Metrics that are currently being displayed by the component.
	/// </summary>
	private IReadOnlyList<MetricsSnapshot> _currentMetrics =
		new List<MetricsSnapshot>();

	/// <summary>
	/// Chart that displays the model's accuracy over time.
	/// </summary>
	private LineChart AccuracyChart { get; set; } = null!;

	/// <summary>
	/// Chart that displays the model's loss over time.
	/// </summary>
	private LineChart LossChart { get; set; } = null!;

	/// <summary>
	/// Options to use for the accuracy chart.
	/// </summary>
	private readonly LineChartOptions _accuracyChartOptions = new()
	{
		Responsive = true,
		Interaction = new()
		{
			Mode = InteractionMode.Index,
		}
	};

	/// <summary>
	/// Options to use for the accuracy chart.
	/// </summary>
	private readonly LineChartOptions _lossChartOptions = new()
	{
		Responsive = true,
		Interaction = new()
		{
			Mode = InteractionMode.Index,
		}
	};

	/// <summary>
	/// Data to display on the accuracy chart.
	/// </summary>
	private ChartData _accuracyChartData;

	/// <summary>
	/// Data to display on the accuracy chart.
	/// </summary>
	private ChartData _lossChartData;

	/// <summary>
	/// Initializes the component.
	/// </summary>
	public TrainingMetrics()
	{
		const double LINE_BORDER_WIDTH = 2;
		const double LINE_HOVER_BORDER_WIDTH = 4;
		// Hide points by default
		const int POINT_RADIUS = 0;
		const int POINT_HOVER_RADIUS = 4;

		// Colors defined by the charts theme (by index):
		// 0. Teal
		// 1. Indigo
		// 2. Orange
		// 3. Pink
		// 4. Lavender
		// 5. Lime
		// 6. Blue
		// 7. Purple
		// 8. Yellow
		// 9. Dark Orange
		// 10. Sea Green
		// 11. Chartreuse Green
		var accuracyLineColor = ColorBuilder.CategoricalTwelveColors[1]
			.ToColor()
			.ToRgbString();
		var lossLineColor = ColorBuilder.CategoricalTwelveColors[1]
			.ToColor()
			.ToRgbString();

		_accuracyChartData = new()
		{
			Labels = new List<string>(),
			Datasets = new List<IChartDataset>()
			{
				new LineChartDataset
				{
					Label = "Model Accuracy (Train)",
					Data = new(),
					BackgroundColor = new List<string>
					{
						accuracyLineColor
					},
					BorderColor = new List<string>
					{
						accuracyLineColor
					},
					BorderWidth = new List<double>
					{
						LINE_BORDER_WIDTH
					},
					HoverBorderWidth = new List<double>
					{
						LINE_HOVER_BORDER_WIDTH
					},
					PointBackgroundColor = new List<string>
					{
						accuracyLineColor
					},
					PointRadius = new List<int>
					{
						POINT_RADIUS
					},
					PointHoverRadius = new List<int>
					{
						POINT_HOVER_RADIUS
					}
				}
			}
		};
		_lossChartData = new()
		{
			Labels = new List<string>(),
			Datasets = new List<IChartDataset>()
			{
				new LineChartDataset
				{
					Label = "Loss",
					Data = new(),
					BackgroundColor = new List<string>
					{
						lossLineColor
					},
					BorderColor = new List<string>
					{
						lossLineColor
					},
					BorderWidth = new List<double>
					{
						LINE_BORDER_WIDTH
					},
					HoverBorderWidth = new List<double>
					{
						LINE_HOVER_BORDER_WIDTH
					},
					PointBackgroundColor = new List<string>
					{
						lossLineColor
					},
					PointRadius = new List<int>
					{
						POINT_RADIUS
					},
					PointHoverRadius = new List<int>
					{
						POINT_HOVER_RADIUS
					}
				}
			}
		};
	}

	/// <summary>
	/// Initializes the component.
	/// </summary>
	protected override async Task OnInitializedAsync()
	{
		base.OnInitialized();

		// Bind to the training session's `OnEpochComplete` event so that the
		//   component updates over time
		TrainingSession.OnEpochComplete +=
			async (_, args) => await UpdateState(TrainingSession.Metrics)
				.ConfigureAwait(false);

		// Force an update of the component's internal state so that it matches
		//   the training session state immediately, rather than only matching
		//   after the first epoch completion event is fired
		await UpdateState(TrainingSession.Metrics)
			.ConfigureAwait(false);
	}

	/// <summary>
	/// Initializes the component's graphs.
	/// </summary>
	/// <param name="firstRender">
	/// Whether this is the first time the component is being rendered.
	/// </param>
	/// <returns></returns>
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender)
			.ConfigureAwait(false);
		if (!firstRender)
		{
			return;
		}

		await AccuracyChart.InitializeAsync(
			_accuracyChartData,
			_accuracyChartOptions)
			.ConfigureAwait(false);
		await LossChart.InitializeAsync(
			_lossChartData,
			_lossChartOptions)
			.ConfigureAwait(false);
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
		var accuracyTask = UpdateAccuracyChart();
		var lossTask = UpdateLossChart();
		await accuracyTask.ConfigureAwait(false);
		await lossTask.ConfigureAwait(false);

		// This could be called from the training thread, so it has to be
		//   invoked using `InvokeAsync()`
		_ = InvokeAsync(StateHasChanged);
	}

	/// <summary>
	/// Updates the accuracy chart to match the current stored metrics.
	/// </summary>
	/// <returns>
	/// A task set once the chart has been updated.
	/// </returns>
	private async Task UpdateAccuracyChart()
	{
		// Get the number of data points currently displayed in the chart
		Debug.Assert(_accuracyChartData.Datasets != null);
		var dataset = (LineChartDataset)_accuracyChartData.Datasets.Single();
		var dataPointsCount = dataset.Data?.Count ?? 0;

		// Iterate over the metrics that have not yet been displayed in the
		//   chart and add them to the chart
		var metrics = _currentMetrics.Skip(dataPointsCount);
		foreach (var metric in metrics)
		{
			var data = new List<IChartDatasetData>()
			{
				new LineChartDatasetData(
					dataset.Label,
					// Scale the accuracy to the range `[0, 100]`
					metric.Accuracy * 100.0
				)
			};

			_accuracyChartData = await AccuracyChart.AddDataAsync(
				_accuracyChartData,
				$"Epoch {metric.CurrentEpoch}",
				data
			).ConfigureAwait(false);
		}
	}

	/// <summary>
	/// Updates the loss chart to match the current stored metrics.
	/// </summary>
	/// <returns>
	/// A task set once the chart has been updated.
	/// </returns>
	private async Task UpdateLossChart()
	{
		// Get the number of data points currently displayed in the chart
		Debug.Assert(_lossChartData.Datasets != null);
		var dataset = (LineChartDataset)_lossChartData.Datasets.Single();
		var dataPointsCount = dataset.Data?.Count ?? 0;

		// Iterate over the metrics that have not yet been displayed in the
		//   chart and add them to the chart
		var metrics = _currentMetrics.Skip(dataPointsCount);
		foreach (var metric in metrics)
		{
			var data = new List<IChartDatasetData>()
			{
				new LineChartDatasetData(
					dataset.Label,
					metric.Loss
				)
			};

			_lossChartData = await LossChart.AddDataAsync(
				_lossChartData,
				$"Epoch {metric.CurrentEpoch}",
				data
			).ConfigureAwait(false);
		}
	}
}
