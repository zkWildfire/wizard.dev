/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.Memory;
namespace Mcs.Simulator.CacheLines;

/// Cache line factory class for the write-through cache line class.
public class WriteThroughCacheLineFactory : ICacheLineFactory
{
	/// Size of each cache line in number of elements.
	private readonly int _size;

	/// Initializes the factory.
	/// @param cacheLineSize Size of each cache line in number of elements.
	public WriteThroughCacheLineFactory(int cacheLineSize)
	{
		_size = cacheLineSize;
	}

	/// Constructs a cache line for the given memory block.
	/// @param memory Memory block that the cache line is for.
	/// @param startingAddress Starting address of the cache line in memory.
	/// @throws ArgumentException Thrown if the starting address is not aligned
	///   to the size of the cache line.
	/// @returns The constructed cache line.
	public ICacheLine Construct(IMemory memory, int startingAddress)
	{
		return startingAddress % _size == 0
			? new WriteThroughCacheLine(memory, startingAddress, _size)
			: throw new ArgumentException(
				"Starting address must be aligned to the size of the cache line."
			);
	}
}
