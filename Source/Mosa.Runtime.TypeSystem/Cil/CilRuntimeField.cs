/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Ruck (grover) <sharpos@michaelruck.de>
 */

using System;
using System.Diagnostics;

using Mosa.Runtime;
using Mosa.Runtime.Metadata;
using Mosa.Runtime.Metadata.Loader;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.Metadata.Tables;
using Mosa.Runtime.TypeSystem;

namespace Mosa.Runtime.TypeSystem.Cil
{
	/// <summary>
	/// A CIL specialization of <see cref="RuntimeField"/>.
	/// </summary>
	sealed class CilRuntimeField : RuntimeField
	{
		#region Data Members

		#endregion // Data Members

		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="CilRuntimeField"/> class.
		/// </summary>
		/// <param name="module">The module.</param>
		/// <param name="name">The name.</param>
		/// <param name="signature">The signature.</param>
		/// <param name="token">The token.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="rva">The rva.</param>
		/// <param name="declaringType">Type of the declaring.</param>
		/// <param name="attributes">The attributes.</param>
		public CilRuntimeField(ITypeModule module, string name, FieldSignature signature, Token token, uint offset, uint rva, RuntimeType declaringType, FieldAttributes attributes) :
			base(module, token, declaringType)
		{
			this.Name = name;
			this.Signature = signature;
			base.Attributes = attributes;
			base.RVA = rva;
		}

		#endregion // Construction

	}
}
