﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace System
{
	/// <summary>
	/// A platform-specific type that is used to represent a pointer or a handle.
	/// </summary>
	[Serializable]
	public struct IntPtr
	{
		/// <summary>
		/// This is 32-bit specific :(
		/// </summary>
		internal int _value;

		public static int Size { get { return 4; } }
	}
}
