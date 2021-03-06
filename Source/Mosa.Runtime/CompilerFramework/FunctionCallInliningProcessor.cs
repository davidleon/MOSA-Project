/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Runtime.CompilerFramework
{

	/// <summary>
	/// Determines if the target of a function call can be inlined.
	/// </summary>
	public sealed class FunctionCallInliningProcessor : BaseMethodCompilerStage, IMethodCompilerStage, IPipelineStage
	{

		#region Construction

		/// <summary>
		/// Initializes a new instance of <see cref="Mosa.Runtime.CompilerFramework.FunctionCallInliningProcessor"/>.
		/// </summary>
		public FunctionCallInliningProcessor()
		{
		}

		#endregion // Construction

		#region IPipelineStage Members

		/// <summary>
		/// Retrieves the name of the compilation stage.
		/// </summary>
		/// <value>The name of the compilation stage.</value>
		string IPipelineStage.Name
		{
			get { return @"Inlining optimization"; }
		}

		/// <summary>
		/// Performs stage specific processing on the compiler context.
		/// </summary>
		public void Run()
		{
		}

		#endregion
	}
}
