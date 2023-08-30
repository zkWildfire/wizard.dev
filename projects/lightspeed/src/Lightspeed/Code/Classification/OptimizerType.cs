/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Lightspeed.Classification;

/// <summary>
/// Enum identifying optimizers that may be selected for networks.
/// </summary>
public enum EOptimizerType
{
	/// <summary>
	/// Optimizer that implements the Adam stochastic optimization algorithm.
	/// </summary>
	/// <seealso href="https://arxiv.org/abs/1412.6980"/>
	Adam
}
