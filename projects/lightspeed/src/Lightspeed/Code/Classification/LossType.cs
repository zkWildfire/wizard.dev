/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Lightspeed.Classification;

/// <summary>
/// Enum identifying loss functions that may be selected for networks.
/// </summary>
public enum ELossType
{
	/// <summary>
	/// Cross entropy loss function.
	/// </summary>
	/// <seealso href="https://en.wikipedia.org/wiki/Cross_entropy"/>
	CrossEntropy
}
