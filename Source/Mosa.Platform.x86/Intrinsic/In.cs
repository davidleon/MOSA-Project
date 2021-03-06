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
using System.Diagnostics;

using Mosa.Runtime;
using Mosa.Runtime.CompilerFramework;
using Mosa.Runtime.CompilerFramework.Operands;
using Mosa.Runtime.TypeSystem;
using CIL = Mosa.Runtime.CompilerFramework.CIL;
using IR = Mosa.Runtime.CompilerFramework.IR;


namespace Mosa.Platform.x86.Intrinsic
{
	/// <summary>
	/// Representations the x86 in instruction.
	/// </summary>
	public sealed class In : IIntrinsicMethod
	{

		#region Methods

		/// <summary>
		/// Replaces the instrinsic call site
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="typeSystem">The type system.</param>
		public void ReplaceIntrinsicCall(Context context, ITypeSystem typeSystem)
		{
			Operand result = context.Result;
			Operand operand1 = context.Operand1;

			RegisterOperand edx = new RegisterOperand(operand1.Type, GeneralPurposeRegister.EDX);
			RegisterOperand eax = new RegisterOperand(result.Type, GeneralPurposeRegister.EAX);

			context.SetInstruction(CPUx86.Instruction.MovInstruction, edx, operand1);
			context.AppendInstruction(CPUx86.Instruction.InInstruction, eax, edx);
			context.AppendInstruction(CPUx86.Instruction.MovInstruction, result, eax);
		}

		#endregion // Methods

	}
}
