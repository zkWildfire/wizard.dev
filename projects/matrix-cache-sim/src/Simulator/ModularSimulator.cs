/*
 *   Copyright (c) 2023 Zach Wilson
 *   All rights reserved.
 */
using Mcs.Simulator.CacheLines;
using Mcs.Simulator.Caches;
using Mcs.Simulator.Events;
using Mcs.Simulator.Memory;
using Mcs.Simulator.Simulation;
using Mcs.Simulator.Validators;
namespace Mcs.Simulator;

/// Simulator that uses modular components to implement the simulation.
public class ModularSimulator : ISimulator
{
	/// Event emitted when a cache line is loaded.
	public event EventHandler<OnCacheLineLoadedEventArgs>? OnCacheLineLoaded;

	/// Event emitted when a cache line is evicted.
	public event EventHandler<OnCacheLineEvictedEventArgs>? OnCacheLineEvicted;

	/// Event raised when a memory location is accessed.
	public event EventHandler<OnMemoryAccessedEventArgs>? OnMemoryAccess;

	/// Memory block used for the simulation.
	private readonly IMemory _memory;

	/// Cache used for the simulation.
	private readonly ICache _cache;

	/// Cache line factory used for the simulation.
	private readonly ICacheLineFactory _cacheLineFactory;

	/// Memory validator used for the simulation.
	private readonly IMemoryValidator _memoryValidator;

	/// Matrix used for the simulation.
	private readonly IMatrix _matrix;

	/// Initializes the simulator.
	/// @param memory Memory block used for the simulation.
	/// @param cache Cache used for the simulation.
	/// @param cacheLineFactory Cache line factory used for the simulation.
	/// @param memoryValidator Memory validator used for the simulation.
	/// @param matrix Matrix used for the simulation.
	public ModularSimulator(
		IMemory memory,
		ICache cache,
		ICacheLineFactory cacheLineFactory,
		IMemoryValidator memoryValidator,
		IMatrix matrix)
	{
		_memory = memory;
		_cache = cache;
		_cacheLineFactory = cacheLineFactory;
		_memoryValidator = memoryValidator;
		_matrix = matrix;

		// Bind to events from the cache
		_cache.OnCacheLineLoaded += (sender, args) =>
		{
			OnCacheLineLoaded?.Invoke(this, args);
		};
		_cache.OnCacheLineEvicted += (sender, args) =>
		{
			OnCacheLineEvicted?.Invoke(this, args);
		};

		// Set the initial memory state
		_memoryValidator.Initialize(_memory, _matrix);
	}

	/// Reads a value from memory.
	/// @param address Address to read from.
	/// @throws ArgumentOutOfRangeException If the address is not a valid
	///   address in the matrix.
	/// @returns The value at the address.
	public int Read(int address)
	{
		// Make sure that the memory address is in the memory block, then
		//   make sure that the memory address is in the matrix
		_memory.ValidateAddress(address);
		var (x, y) = _matrix.ToMatrixCoordinate(address);

		var (cacheLine, isCacheHit) = GetCacheLine(address);
		var value = cacheLine.Read(address);
		OnMemoryAccess?.Invoke(
			this,
			new OnMemoryAccessedEventArgs()
			{
				Address = address,
				X = x,
				Y = y,
				IsRead = true,
				IsCacheHit = isCacheHit,
				NewValue = value,
				OldValue = value
			}
		);

		return value;
	}

	/// Writes a value to memory.
	/// @param address Address to write to.
	/// @param value Value to write.
	/// @throws ArgumentOutOfRangeException If the address is not a valid
	///   address in the matrix.
	public void Write(int address, int value)
	{
		// Make sure that the memory address is in the memory block, then
		//   make sure that the memory address is in the matrix
		_memory.ValidateAddress(address);
		var (x, y) = _matrix.ToMatrixCoordinate(address);

		var (cacheLine, isCacheHit) = GetCacheLine(address);
		var oldValue = cacheLine.Read(address);
		cacheLine.Write(address, value);
		OnMemoryAccess?.Invoke(
			this,
			new OnMemoryAccessedEventArgs()
			{
				Address = address,
				X = x,
				Y = y,
				IsRead = false,
				IsCacheHit = isCacheHit,
				NewValue = value,
				OldValue = oldValue
			}
		);
	}

	/// Checks whether the simulation's matrix has been fully transposed.
	/// @returns True if the matrix has been fully transposed, false otherwise.
	public bool Validate()
	{
		return _memoryValidator.Validate(_memory, _matrix);
	}

	/// Gets the cache line for the address, loading it if necessary.
	/// @param address Address to get the cache line for.
	/// @returns The cache line for the address and whether the cache line
	///   was loaded (as opposed to being already present in the cache).
	private (ICacheLine, bool) GetCacheLine(int address)
	{
		var isCacheHit = true;

		// Check if the cache line for the address needs to be loaded
		if (!_cache.IsPresent(address))
		{
			// Determine the starting address of the cache line containing
			//   the target memory address
			var startingAddress = address - (address % _cache.CacheLineSize);

			// Load the cache line
			var newCacheLine = _cacheLineFactory.Construct(
				_memory,
				startingAddress
			);
			_cache.LoadCacheLine(newCacheLine);
			isCacheHit = false;
		}

		return (_cache.GetCacheLine(address), isCacheHit);
	}
}
