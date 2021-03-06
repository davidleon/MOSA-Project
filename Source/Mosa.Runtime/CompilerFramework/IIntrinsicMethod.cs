﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.Collections.Generic;
using System.Text;

using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.TypeSystem;

namespace Mosa.Runtime.CompilerFramework
{
	/// <summary>
	/// Interface to an intrinsic instruction
	/// </summary>
	public interface IIntrinsicMethod
	{
		/// <summary>
		/// Replaces the intrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		void ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem);
	}
}
