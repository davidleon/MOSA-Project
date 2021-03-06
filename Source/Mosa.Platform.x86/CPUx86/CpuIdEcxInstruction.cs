﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Simon Wollwage (rootnode) <rootnode@mosa-project.org>
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Mosa.Runtime.CompilerFramework.Operands;
using IR = Mosa.Runtime.CompilerFramework.IR;

using Mosa.Runtime.CompilerFramework;


namespace Mosa.Platform.x86.CPUx86
{
	/// <summary>
	/// Representations the x86 CPUID instruction.
	/// </summary>
	public sealed class CpuIdEcxInstruction : TwoOperandInstruction
	{

		#region Data Members

		private static readonly OpCode opcode = new OpCode(new byte[] { 0x0F, 0xA2 }); // Move imm32 to r/m32

		#endregion // Data Members

		#region Methods

		/// <summary>
		/// Emits the specified platform instruction.
		/// </summary>
		/// <param name="ctx">The context.</param>
		/// <param name="emitter">The emitter.</param>
		protected override void Emit(Context ctx, MachineCodeEmitter emitter)
		{
			emitter.Emit(opcode, null, null);
		}

		/// <summary>
		/// Allows visitor based dispatch for this instruction object.
		/// </summary>
		/// <param name="visitor">The visitor object.</param>
		/// <param name="context">The context.</param>
		public override void Visit(IX86Visitor visitor, Context context)
		{
			visitor.CpuIdEax(context);
		}

		#endregion // Methods
	}
}
