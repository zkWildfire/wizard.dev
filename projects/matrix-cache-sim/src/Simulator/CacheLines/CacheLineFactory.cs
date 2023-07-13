/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.Memory;
namespace Mcs.Simulator.CacheLines;

/// Interface for factory classes that construct cache lines.
public interface ICacheLineFactory
{
	/// Constructs a cache line for the given memory block.
	/// @param memory Memory block that the cache line is for.
	/// @param startingAddress Starting address of the cache line in memory.
	/// @throws ArgumentException Thrown if the starting address is not aligned
	///   to the size of the cache line.
	/// @returns The constructed cache line.
	public ICacheLine Construct(IMemory memory, int startingAddress);
}
