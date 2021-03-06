﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 */

using System;
using System.Collections.Generic;
using System.Text;
//using System.Diagnostics;

using Mosa.Runtime;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.Metadata.Signatures;
using Mosa.Runtime.TypeSystem;
using CIL = Mosa.Runtime.CompilerFramework.CIL;
using IR = Mosa.Runtime.CompilerFramework.IR;

namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// 
	/// </summary>
	public sealed class Set : IIntrinsicMethod
	{

		#region Methods

		/// <summary>
		/// Replaces the instrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		public void ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem)
		{
			Operand dest = context.Operand1;
			Operand value = context.Operand2;

			RegisterOperand edx = new RegisterOperand(dest.Type, GeneralPurposeRegister.EDX);
			RegisterOperand eax = new RegisterOperand(value.Type, GeneralPurposeRegister.EAX);
			MemoryOperand memory = new MemoryOperand(new SigType(context.InvokeTarget.Signature.Parameters[1].Type), GeneralPurposeRegister.EDX, new IntPtr(0));

			context.SetInstruction(CPUx86.Instruction.MovInstruction, edx, dest);
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, eax, value);
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, memory, eax);
		}

		#endregion // Methods

	}
}
