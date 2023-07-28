/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
namespace Mcs.Simulator.Memory;

/// Memory implementation backed by a flat array.
public class ArrayMemory : IMemory
{
	/// Number of elements in the memory.
	public int Size { get; }

	/// Backing array for the memory.
	private readonly List<int> _memory;

	/// Initializes the memory.
	/// @param size Number of elements that may be stored in memory.
	public ArrayMemory(int size)
	{
		Size = size;
		_memory = new List<int>(new int[size]);
	}

	/// Reads a value from memory.
	/// @param address Address to read from.
	/// @throws ArgumentOutOfRangeException If the address is not a valid
	///   address in the memory block.
	/// @returns The value at the address.
	public int Read(int address)
	{
		return address >= 0 || address < Size
			? _memory[address]
			: throw new ArgumentOutOfRangeException(
				nameof(address),
				address,
				$"Expected address '{address}' to be in the range [0, {Size})."
			);
	}

	/// Writes a value to memory.
	/// @param address Address to write to.
	/// @param value Value to write.
	/// @throws ArgumentOutOfRangeException If the address is not a valid
	///   address in the memory block.
	public void Write(int address, int value)
	{
		if (address < 0 || address >= Size)
		{
			throw new ArgumentOutOfRangeException(
				nameof(address),
				address,
				$"Expected address '{address}' to be in the range [0, {Size})."
			);
		}

		_memory[address] = value;
	}

	/// Checks if the address is a valid address in the memory block.
	/// @throws ArgumentOutOfRangeException If the address is not a valid
	///   address in the memory block.
	public void ValidateAddress(int address)
	{
		if (address < 0 || address >= Size)
		{
			throw new ArgumentOutOfRangeException(
				nameof(address),
				address,
				$"Expected address '{address}' to be in the range [0, {Size})."
			);
		}
	}
}
