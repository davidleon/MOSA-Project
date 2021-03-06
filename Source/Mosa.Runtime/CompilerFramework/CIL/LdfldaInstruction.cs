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
using System.Diagnostics;
using System.Text;

using Mosa.Runtime.Metadata;
using Mosa.Runtime.TypeSystem;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.CompilerFramework.Operands;

namespace Mosa.Runtime.CompilerFramework.CIL
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class LdfldaInstruction : UnaryInstruction
	{
		#region Construction

		/// <summary>
		/// Initializes a new instance of the <see cref="LdfldaInstruction"/> class.
		/// </summary>
		/// <param name="opcode">The opcode.</param>
		public LdfldaInstruction(OpCode opcode)
			: base(opcode, 1)
		{
		}

		#endregion // Construction

		#region Methods

		/// <summary>
		/// Decodes the specified instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="decoder">The instruction decoder, which holds the code stream.</param>
		public override void Decode(Context ctx, IInstructionDecoder decoder)
		{
			base.Decode(ctx, decoder);

			Token token = decoder.DecodeTokenType();

			ITypeModule module = null;
			Mosa.Runtime.TypeSystem.Generic.CilGenericType genericType = decoder.Method.DeclaringType as Mosa.Runtime.TypeSystem.Generic.CilGenericType;
			if (genericType != null)
				module = (decoder.Method.DeclaringType as Mosa.Runtime.TypeSystem.Generic.CilGenericType).BaseGenericType.Module;
			else
				module = decoder.Method.Module;
			ctx.RuntimeField = module.GetField(token);

			if (ctx.RuntimeField.ContainsGenericParameter)
			{
				;
			}

			SigType sigType = new RefSigType(ctx.RuntimeField.SignatureType);
			ctx.Result = LoadInstruction.CreateResultOperand(decoder, Operand.StackTypeFromSigType(sigType), sigType);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor.</param>
		/// <param name="context">The context.</param>
		public override void Visit(ICILVisitor visitor, Context context)
		{
			visitor.Ldflda(context);
		}

		#endregion Methods
	}
}
