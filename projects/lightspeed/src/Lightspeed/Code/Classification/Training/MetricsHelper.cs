/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using static TorchSharp.torch;
namespace Lightspeed.Classification.Training;

/// <summary>
/// Helper class defining methods for calculating various metrics.
/// </summary>
public class MetricsHelper
{
	/// <summary>
	/// Calculates and returns the metrics for the model using all added data.
	/// </summary>
	public ModelMetrics Metrics
	{
		get
		{
			// Initialize variables to hold the sum of all metrics for macro-averaging
			double sumAccuracy = 0;
			double sumTruePositiveRate = 0;
			double sumFalsePositiveRate = 0;
			double sumTrueNegativeRate = 0;
			double sumFalseNegativeRate = 0;
			double sumPrecision = 0;
			double sumRecall = 0;
			double sumF1Score = 0;

			// List to hold ClassMetrics for each class
			var classMetricsList = new List<ClassMetrics>();

			// Loop over each class to calculate metrics
			foreach (var data in _classData)
			{
				var tp = data.TruePositives;
				var fp = data.FalsePositives;
				var tn = data.TrueNegatives;
				var fn = data.FalseNegatives;

				// Calculate metrics for this class
				var accuracy = SafeDivide(
					tp + tn,
					tp + fp + tn + fn
				);
				var truePositiveRate = SafeDivide(
					tp,
					tp + fn
				);
				var falsePositiveRate = SafeDivide(
					fp,
					fp + tn
				);
				var trueNegativeRate = SafeDivide(
					tn,
					tn + fp
				);
				var falseNegativeRate = SafeDivide(
					fn,
					fn + tp
				);
				var precision = SafeDivide(
					tp,
					tp + fp
				);
				var recall = SafeDivide(
					tp,
					tp + fn
				);
				var f1Score = SafeDivide(
					2 * (precision * recall),
					precision + recall
				);

				// Update the sums for macro-averaging
				sumAccuracy += accuracy;
				sumTruePositiveRate += truePositiveRate;
				sumFalsePositiveRate += falsePositiveRate;
				sumTrueNegativeRate += trueNegativeRate;
				sumFalseNegativeRate += falseNegativeRate;
				sumPrecision += precision;
				sumRecall += recall;
				sumF1Score += f1Score;

				// Create the metrics object and add it to the list
				var cm = new ClassMetrics
				{
					TruePositiveRate = truePositiveRate,
					FalsePositiveRate = falsePositiveRate,
					TrueNegativeRate = trueNegativeRate,
					FalseNegativeRate = falseNegativeRate,
					Precision = precision,
					Recall = recall,
					F1Score = f1Score
				};
				classMetricsList.Add(cm);
			}

			// Calculate macro-averaged metrics
			var numClasses = _classData.Length;
			var macroTruePositiveRate = SafeDivide(
				sumTruePositiveRate,
				numClasses
			);
			var macroFalsePositiveRate = SafeDivide(
				sumFalsePositiveRate,
				numClasses
			);
			var macroTrueNegativeRate = SafeDivide(
				sumTrueNegativeRate,
				numClasses
			);
			var macroFalseNegativeRate = SafeDivide(
				sumFalseNegativeRate,
				numClasses
			);
			var macroPrecision = SafeDivide(
				sumPrecision,
				numClasses
			);
			var macroRecall = SafeDivide(
				sumRecall,
				numClasses
			);
			var macroF1Score = SafeDivide(
				sumF1Score,
				numClasses
			);

			return new ModelMetrics
			{
				ClassMetrics = classMetricsList,
				Accuracy = (double)_totalCorrect / _totalSamples,
				Count = _totalSamples,
				TruePositiveRate = macroTruePositiveRate,
				FalsePositiveRate = macroFalsePositiveRate,
				TrueNegativeRate = macroTrueNegativeRate,
				FalseNegativeRate = macroFalseNegativeRate,
				Precision = macroPrecision,
				Recall = macroRecall,
				F1Score = macroF1Score,
				Loss = _loss
			};
		}
	}

	/// <summary>
	/// Tracks per-class data required to calculate per-class model metrics.
	/// </summary>
	private struct ClassData
	{
		/// <summary>
		/// The number of true positives for this class.
		/// </summary>
		public int TruePositives { get; set; }

		/// <summary>
		/// The number of false positives for this class.
		/// </summary>
		public int FalsePositives { get; set; }

		/// <summary>
		/// The number of true negatives for this class.
		/// </summary>
		public int TrueNegatives { get; set; }

		/// <summary>
		/// The number of false negatives for this class.
		/// </summary>
		public int FalseNegatives { get; set; }

		/// <summary>
		/// Adds metrics for a single prediction.
		/// </summary>
		/// <param name="isTruePositive">
		/// Whether the prediction is a true positive.
		/// </param>
		/// <param name="isFalsePositive">
		/// Whether the prediction is a false positive.
		/// </param>
		/// <param name="isTrueNegative">
		/// Whether the prediction is a true negative.
		/// </param>
		/// <param name="isFalseNegative">
		/// Whether the prediction is a false negative.
		/// </param>
		public void UpdateMetrics(
			bool isTruePositive,
			bool isFalsePositive,
			bool isTrueNegative,
			bool isFalseNegative)
		{
			if (isTruePositive)
			{
				TruePositives++;
			}
			if (isFalsePositive)
			{
				FalsePositives++;
			}
			if (isTrueNegative)
			{
				TrueNegatives++;
			}
			if (isFalseNegative)
			{
				FalseNegatives++;
			}
		}
	}

	/// <summary>
	/// The number of classes that the model is classifying.
	/// </summary>
	private int NumClasses => _classData.Length;

	/// <summary>
	/// Per-class data required to calculate per-class model metrics.
	/// </summary>
	private readonly ClassData[] _classData;

	/// <summary>
	/// Total number of samples that have been added to the helper.
	/// </summary>
	private long _totalSamples;

	/// <summary>
	/// Total number of correct predictions that have been added to the helper.
	/// </summary>
	private long _totalCorrect;

	/// <summary>
	/// Total loss for the model over the epoch.
	/// </summary>
	private double _loss;

	/// <summary>
	/// Initializes the helper.
	/// </summary>
	/// <param name="classes">
	/// Number of classes that the model is classifying.
	/// </param>
	public MetricsHelper(int classes)
	{
		_classData = Enumerable.Range(0, classes)
			.Select(_ => new ClassData())
			.ToArray();
	}

	/// <summary>
	/// Adds a set of data to the helper.
	/// </summary>
	/// <param name="predictions">
	/// Predictions from the model. This must be a tensor of shape NxM, where
	///   N is the number of samples in the batch and M is the number of classes.
	///   The tensor's data type must be `float` (`float32` in TorchSharp).
	/// </param>
	/// <param name="targets">
	/// Labels for each sample in the batch. This must be a tensor of shape N,
	///   where N is the number of samples in the batch. The tensor's data type
	///   must be `long` (`int64` in TorchSharp).
	/// </param>
	/// <param name="loss">
	/// Loss for the batch.
	/// </param>
	public void AddData(Tensor predictions, Tensor targets, double loss)
	{
		_loss += loss;

		// Validate the tensor shapes and data types
		Debug.Assert(predictions.Dimensions == 2);
		Debug.Assert(predictions.dtype == ScalarType.Float32);
		Debug.Assert(targets.Dimensions == 1);
		Debug.Assert(targets.dtype == ScalarType.Int64);

		var batchSize = predictions.shape[0];
		Debug.Assert(targets.shape[0] == batchSize);
		Debug.Assert(predictions.shape[1] == NumClasses);

		// Check which class was predicted for each sample
		var predictedClasses = predictions.argmax(1);

		// Initialize temporary counters for each class
		var tp = new int[NumClasses];
		var fp = new int[NumClasses];
		var tn = new int[NumClasses];
		var fn = new int[NumClasses];

		// Convert TorchSharp tensors to arrays for easier manipulation
		var predictedArray = predictedClasses.data<long>().ToArray();
		var targetArray = targets.data<long>().ToArray();

		// Loop over each prediction and update counts
		for (var i = 0; i < predictedArray.Length; i++)
		{
			var predicted = predictedArray[i];
			var actual = targetArray[i];

			// Update total samples and total correct
			_totalSamples++;
			_totalCorrect += predicted == actual ? 1 : 0;

			// Update True Positives and False Negatives
			for (var j = 0; j < NumClasses; j++)
			{
				if (actual != j)
				{
					continue;
				}

				if (predicted == j)
				{
					tp[j]++;
				}
				else
				{
					fn[j]++;
				}
			}

			// Update True Negatives and False Positives
			for (var j = 0; j < NumClasses; j++)
			{
				if (actual == j)
				{
					continue;
				}

				if (predicted != j)
				{
					tn[j]++;
				}
				else
				{
					fp[j]++;
				}
			}
		}

		// Update the _classData array
		for (var i = 0; i < NumClasses; i++)
		{
			_classData[i].UpdateMetrics(
				isTruePositive: tp[i] > 0,
				isFalsePositive: fp[i] > 0,
				isTrueNegative: tn[i] > 0,
				isFalseNegative: fn[i] > 0
			);
		}

	}

	/// <summary>
	/// Helper method that safely handles division by zero.
	/// </summary>
	/// <param name="numerator">
	/// Numerator for the division operation.
	/// </param>
	/// <param name="denominator">
	/// Denominator for the division operation. Can be zero.
	/// </param>
	/// <returns>
	/// If the denominator is zero, returns 0. Otherwise, returns the result
	///   of the division operation.
	/// </returns>
	private static double SafeDivide(double numerator, double denominator)
	{
		return (denominator == 0)
			? 0
			: numerator / denominator;
	}

}
