/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using BlazorBootstrap;
namespace Lightspeed.Components.Utils;

/// <summary>
/// Displays a line graph for a set of metrics.
/// </summary>
public partial class LineGraph : ComponentBase
{
	/// <summary>
	/// Maximum number of datasets that may be displayed simultaneously.
	/// </summary>
	public const int MAX_DATASETS = 12;

	/// <summary>
	/// Title to display over the chart.
	/// </summary>
	[Parameter]
	public string ChartTitle { get; set; } = string.Empty;

	/// <summary>
	/// Label to display for each line.
	/// This must be a list of at most `MAX_DATASETS` elements.
	/// </summary>
	[Parameter]
	public IReadOnlyList<string> LineLabels { get; set; } = new List<string>();

	/// <summary>
	/// Width of the lines displayed on the chart.
	/// </summary>
	/// <remarks>
	/// This value defaults to the value from the Blazor Charts examples.
	/// </remarks>
	[Parameter]
	public double LineBorderWidth { get; set; } = 2;

	/// <summary>
	/// Line width to use when a line is hovered over.
	/// </summary>
	/// <remarks>
	/// This value defaults to the value from the Blazor Charts examples.
	/// </remarks>
	[Parameter]
	public double LineHoverBorderWidth { get; set; } = 4;

	/// <summary>
	/// Size to use when rendering points on the chart's lines.
	/// </summary>
	/// <remarks>
	/// This value defaults to the value from the Blazor Charts examples. A
	///   value of 0 means that no points will be rendered.
	/// </remarks>
	[Parameter]
	public int PointRadius { get; set; }

	/// <summary>
	/// Size to use for the points when they are hovered over.
	/// </summary>
	/// <remarks>
	/// This value defaults to the value from the Blazor Charts examples.
	/// </remarks>
	[Parameter]
	public int PointHoverRadius { get; set; } = 4;

	/// <summary>
	/// Number of data points that are currently being displayed.
	/// This is the number of data points per line, not the number of lines.
	/// </summary>
	public int DataPointsCount
	{
		get
		{
			Debug.Assert(_chartData.Datasets != null);
			var dataset = (LineChartDataset)_chartData.Datasets.First();
			return dataset.Data?.Count ?? 0;
		}
	}

	/// <summary>
	/// Graph component used to display the data.
	/// </summary>
	private LineChart Chart { get; set; } = null!;

	/// <summary>
	/// Number of datasets that are currently being displayed.
	/// </summary>
	private int DatasetsCount => LineLabels.Count;

	/// <summary>
	/// Options to use for the chart.
	/// </summary>
	private readonly LineChartOptions _chartOptions = new()
	{
		Responsive = true,
		Interaction = new()
		{
			Mode = InteractionMode.Index
		}
	};

	/// <summary>
	/// Data to display on the chart.
	/// </summary>
	private ChartData _chartData = null!;

	/// <summary>
	/// Initializes the component.
	/// </summary>
	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync()
			.ConfigureAwait(true);
		Debug.Assert(LineLabels.Count <= MAX_DATASETS);

		// Colors defined by the charts theme (by index):
		//   0. Teal
		//   1. Indigo
		//   2. Orange
		//   3. Pink
		//   4. Lavender
		//   5. Lime
		//   6. Blue
		//   7. Purple
		//   8. Yellow
		//   9. Dark Orange
		//   10. Sea Green
		//   11. Chartreuse Green
		// Use `ColorBuilder.CategoricalTwelveColors` to access these colors.
		var datasets = new List<IChartDataset>();
		for (var i = 0; i < LineLabels.Count; i++)
		{
			var color = ColorBuilder.CategoricalTwelveColors[i]
				.ToColor()
				.ToRgbString();

			datasets.Add(new LineChartDataset()
			{
				Label = LineLabels[i],
				Data = new(),
				BackgroundColor = new List<string>
				{
					color
				},
				BorderColor = new List<string>
				{
					color
				},
				BorderWidth = new List<double>
				{
					LineBorderWidth
				},
				HoverBorderWidth = new List<double>
				{
					LineHoverBorderWidth
				},
				PointBackgroundColor = new List<string>
				{
					color
				},
				PointRadius = new List<int>
				{
					PointRadius
				},
				PointHoverRadius = new List<int>
				{
					PointHoverRadius
				}
			});
		}

		_chartData = new()
		{
			// This list contains the X axis labels to display on the chart.
			//   Since no data has been added yet, leave it as an empty list
			//   for now.
			Labels = new List<string>(),
			Datasets = datasets
		};
	}

	/// <summary>
	/// Handles first time initialization for the component.
	/// </summary>
	/// <param name="firstRender">
	/// Whether this is the first time the component has been rendered.
	/// </param>
	/// <returns>
	/// A task set once initialization completes.
	/// </returns>
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender)
			.ConfigureAwait(true);
		if (!firstRender)
		{
			return;
		}

		await Chart.InitializeAsync(_chartData, _chartOptions)
			.ConfigureAwait(true);
	}

	/// <summary>
	/// Adds a new data point to each line in the graph.
	/// </summary>
	/// <param name="label">
	/// X-axis label for the new set of data points.
	/// </param>
	/// <param name="data">
	/// Data to add to the graph. Must be a list of length `LineLabels.Count`.
	///   Each data point will be assigned to the label in `LineLabels` at the
	///   same index.
	/// </param>
	/// <returns>
	/// A task set once the chart has been updated.
	/// </returns>
	public async Task AddDataAsync(
		string label,
		IReadOnlyList<double> data)
	{
		Debug.Assert(data.Count == LineLabels.Count);
		Debug.Assert(_chartData.Datasets != null);
		Debug.Assert(_chartData.Datasets.Count == LineLabels.Count);

		// Generate `LineChartDatasetData` instances for each new data point
		var newData = new List<IChartDatasetData>();
		for (var i = 0; i < DatasetsCount; i++)
		{
			var dataset = (LineChartDataset)_chartData.Datasets[i];
			var value = data[i];

			newData.Add(new LineChartDatasetData(
				dataset.Label,
				value
			));
		}

		// Update the chart
		_chartData = await Chart.AddDataAsync(
			_chartData,
			label,
			newData
		).ConfigureAwait(true);
	}

	/// <summary>
	/// Replaces the existing data with a new set of data.
	/// </summary>
	/// <param name="data">
	/// New data to display on the graph. Each internal list must be the same
	///   length. The number of internal lists must be equal to the number of
	///   lines on the graph.
	/// </param>
	/// <returns>
	/// A task set once the chart has been cleared.
	/// </returns>
	public Task ReplaceDataAsync(
		IReadOnlyList<List<double>> data)
	{
		Debug.Assert(_chartData.Datasets != null);
		var newDatasets = new List<IChartDataset>();
		for (var i = 0; i < _chartData.Datasets.Count; i++)
		{
			var dataset = (LineChartDataset)_chartData.Datasets[i];
			dataset.Data = data[i];
			newDatasets.Add(dataset);
		}

		_chartData.Datasets = newDatasets;
		return Chart.UpdateAsync(_chartData, _chartOptions);
	}
}
