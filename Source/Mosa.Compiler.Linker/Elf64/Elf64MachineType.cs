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

namespace Mosa.Compiler.Linker.Elf64
{
	/// <summary>
	/// 
	/// </summary>
	public enum Elf64MachineType : ushort
	{
		/// <summary>
		/// No machine 
		/// </summary>
		NoMachine = 0x0,
		/// <summary>
		/// AT&amp;T WE 32100
		/// </summary>
		M32 = 0x1,
		/// <summary>
		/// SPARC
		/// </summary>
		Sparc = 0x2,
		/// <summary>
		/// Intel Architecture
		/// </summary>
		Intel386 = 0x3,
		/// <summary>
		/// Motorola 68000
		/// </summary>
		Motorola68000 = 0x4,
		/// <summary>
		/// Motorola 88000
		/// </summary>
		Motorola88000 = 0x5,
		/// <summary>
		/// Intel 80860
		/// </summary>
		Intel80860 = 0x7,
		/// <summary>
		/// MIPS RS3000 Big-Endian
		/// </summary>
		MipsRS3000 = 0x8,
		/// <summary>
		/// MIPS RS4000 Big-Endian
		/// </summary>
		MipsRS4000 = 0xA,
	}
}
