﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.Kernel.Memory.X86
{
	/// <summary>
	/// Static class of helpful memory functions
	/// </summary>
	public static class Memory
	{
		/// <summary>
		/// Clears the specified memory area.
		/// </summary>
		/// <param name="start">The start.</param>
		/// <param name="bytes">The bytes.</param>
		public unsafe static void Clear(uint start, uint bytes)
		{
			for (uint z = 0; z < bytes; z++)
				(*(byte*)(start + z)) = 0;	// Slow! 
		}

		/// <summary>
		/// Sets the specified value at location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="value">The value.</param>
		public unsafe static void Set32(uint location, uint value)
		{
			(*(uint*)(location)) = value;
		}

		/// <summary>
		/// Sets the specified value at location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="value">The value.</param>
		public unsafe static void Set8(uint location, byte value)
		{
			(*(byte*)(location)) = value;
		}

		/// <summary>
		/// Gets the value at specified location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <returns></returns>
		public unsafe static uint Get32(uint location)
		{
			return (uint)(*((uint*)(location)));
		}

		/// <summary>
		/// Gets the value at specified location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <returns></returns>
		public unsafe static byte Get8(uint location)
		{
			return (byte)(*((uint*)(location)));
		}

		/// <summary>
		/// Gets the value at specified location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <returns></returns>
		public unsafe static ulong Get64(uint location)
		{
			return (ulong)(*((ulong*)(location)));
		}

		/// <summary>
		/// Flushes the Translation Lookaside Buffer (TLB).
		/// </summary>
		public static void FlushTLB()
		{
			// TODO
			// Native.Invlpg(x)?
		}

		/// <summary>
		/// Flushes the Translation Lookaside Buffer (TLB).
		/// </summary>
		/// <param name="address">The address.</param>
		public static void FlushTLB(uint address)
		{
			// TODO
			// Native.Invlpg(Address)?
		}
	}
}