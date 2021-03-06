﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Alex Lyman <mail.alex.lyman@gmail.com>
 *  Simon Wollwage (rootnode) <kintaro@think-in-co.de>
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 *  
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using MbUnit.Framework;

using Mosa.Test.Runtime.CompilerFramework;

namespace Mosa.Test.Cases.OLD.IL
{
	[TestFixture]
	public class Div : TestCompilerAdapter
	{
		private static string CreateTestCode(string name, string typeIn, string typeOut)
		{
			return @"
				static class Test
				{
					static bool " + name + "(" + typeOut + " expect, " + typeIn + " a, " + typeIn + @" b)
					{
						return expect == (a / b);
					}
				}";
		}

		private static string CreateTestCodeWithReturn(string name, string typeIn, string typeOut)
		{
			return @"
				static class Test
				{
					static " + typeOut + " " + name + "(" + typeOut + " expect, " + typeIn + " a, " + typeIn + @" b)
					{
						return (a / b);
					}
				}";
		}

		private static string CreateConstantTestCode(string name, string typeIn, string typeOut, string constLeft, string constRight)
		{
			if (String.IsNullOrEmpty(constRight))
			{
				return @"
					static class Test
					{
						static bool " + name + "(" + typeOut + " expect, " + typeIn + @" x)
						{
							return expect == (" + constLeft + @" / x);
						}
					}";
			}
			else if (String.IsNullOrEmpty(constLeft))
			{
				return @"
					static class Test
					{
						static bool " + name + "(" + typeOut + " expect, " + typeIn + @" x)
						{
							return expect == (x / " + constRight + @");
						}
					}";
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		private static string CreateConstantTestCodeWithReturn(string name, string typeIn, string typeOut, string constLeft, string constRight)
		{
			if (String.IsNullOrEmpty(constRight))
			{
				return @"
					static class Test
					{
						static " + typeOut + " " + name + "(" + typeOut + " expect, " + typeIn + @" x)
						{
							return (" + constLeft + @" / x);
						}
					}";
			}
			else if (String.IsNullOrEmpty(constLeft))
			{
				return @"
					static class Test
					{
						static " + typeOut + " " + name + "(" + typeOut + " expect, " + typeIn + @" x)
						{
							return (x / " + constRight + @");
						}
					}";
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		#region C
		
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(17, 128)]
		[Row('a', 'Z')]
		[Row(char.MinValue, char.MaxValue)]
		[Test]
		public void DivC(char a, char b)
		{
			settings.CodeSource = CreateTestCode("DivC", "char", "char");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivC", (char)(a / b), a, b));
		}

		//[Row(0, 'a')]
		//[Row('-', '.')]
		[Row('a', 'Z')]
		[Test]
		public void DivConstantCRight(char a, char b)
		{
			settings.CodeSource = CreateConstantTestCodeWithReturn("DivConstantCRight", "char", "int", null, "'" + b.ToString() + "'");
			Assert.AreEqual(a / b, Run<int>(string.Empty, "Test", "DivConstantCRight", (a / b), a));
		}

		[Row('a', 0, ExpectedException = typeof(DivideByZeroException))]
		[Row('-', '.')]
		[Row((char)97, (char)90)]
		[Test]
		public void DivConstantCLeft(char a, char b)
		{
			settings.CodeSource = CreateConstantTestCodeWithReturn("DivConstantCLeft", "char", "int", "'" + a.ToString() + "'", null);
			Assert.AreEqual(a / b, Run<int>(string.Empty, "Test", "DivConstantCLeft", (a / b), (char)b));
		}
		#endregion

		#region I1
		
		[Row(1, 2)]
		[Row(23, 21)]
		[Row(1, -2)]
		[Row(-1, 2)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(-17, -2)]
		// And reverse
		[Row(2, 1)]
		[Row(21, 23)]
		[Row(-2, 1)]
		[Row(2, -1)]
		[Row(-2, -17)]
		// (MinValue, X) Cases
		[Row(sbyte.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(sbyte.MinValue, 1)]
		[Row(sbyte.MinValue, 17)]
		[Row(sbyte.MinValue, 123)]
		[Row(sbyte.MinValue, -0, ExpectedException = typeof(DivideByZeroException))]
		[Row(sbyte.MinValue, -1)]
		[Row(sbyte.MinValue, -17)]
		[Row(sbyte.MinValue, -123)]
		// (MaxValue, X) Cases
		[Row(sbyte.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(sbyte.MaxValue, 1)]
		[Row(sbyte.MaxValue, 17)]
		[Row(sbyte.MaxValue, 123)]
		[Row(sbyte.MaxValue, -0, ExpectedException = typeof(DivideByZeroException))]
		[Row(sbyte.MaxValue, -1)]
		[Row(sbyte.MaxValue, -17)]
		[Row(sbyte.MaxValue, -123)]
		// (X, MinValue) Cases
		[Row(0, sbyte.MinValue)]
		[Row(1, sbyte.MinValue)]
		[Row(17, sbyte.MinValue)]
		[Row(123, sbyte.MinValue)]
		[Row(-0, sbyte.MinValue)]
		[Row(-1, sbyte.MinValue)]
		[Row(-17, sbyte.MinValue)]
		[Row(-123, sbyte.MinValue)]
		// (X, MaxValue) Cases
		[Row(0, sbyte.MaxValue)]
		[Row(1, sbyte.MaxValue)]
		[Row(17, sbyte.MaxValue)]
		[Row(123, sbyte.MaxValue)]
		[Row(-0, sbyte.MaxValue)]
		[Row(-1, sbyte.MaxValue)]
		[Row(-17, sbyte.MaxValue)]
		[Row(-123, sbyte.MaxValue)]
		// Extremvaluecases
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Row(sbyte.MaxValue, sbyte.MinValue)]
		[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Test]
		public void DivI1(sbyte a, sbyte b)
		{
			settings.CodeSource = CreateTestCode("DivI1", "sbyte", "int");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivI1", a / b, a, b));
		}

		[Row(23, 21)]
		[Row(2, -17)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void DivConstantI1Right(sbyte a, sbyte b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantI1Right", "sbyte", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantI1Right", (a / b), a));
		}

		[Row(23, 21)]
		[Row(2, -17)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(sbyte.MinValue, sbyte.MaxValue)]
		[Test]
		public void DivConstantI1Left(sbyte a, sbyte b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantI1Left", "sbyte", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantI1Left", (a / b), b));
		}
		#endregion

		#region U1
		
		[Row(1, 2)]
		[Row(23, 21)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		// And reverse
		[Row(2, 1)]
		[Row(21, 23)]
		// (MinValue, X) Cases
		[Row(byte.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(byte.MinValue, 1)]
		[Row(byte.MinValue, 17)]
		[Row(byte.MinValue, 123)]
		// (MaxValue, X) Cases
		[Row(byte.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(byte.MaxValue, 1)]
		[Row(byte.MaxValue, 17)]
		[Row(byte.MaxValue, 123)]
		// (X, MinValue) Cases
		[Row(0, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(1, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(17, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(123, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
		// (X, MaxValue) Cases
		[Row(0, byte.MaxValue)]
		[Row(1, byte.MaxValue)]
		[Row(17, byte.MaxValue)]
		[Row(123, byte.MaxValue)]
		// Extremvaluecases
		[Row(byte.MinValue, byte.MaxValue)]
		[Row(byte.MaxValue, byte.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Test]
		public void DivU1(byte a, byte b)
		{
			settings.CodeSource = CreateTestCode("DivU1", "byte", "uint");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivU1", (uint)(a / b), a, b));
		}

		[Row(23, 21)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(byte.MinValue, byte.MaxValue)]
		[Test]
		public void DivConstantU1Right(byte a, byte b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantU1Right", "byte", "uint", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantU1Right", (uint)(a / b), a));
		}

		[Row(23, 21)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(byte.MinValue, byte.MaxValue)]
		[Test]
		public void DivConstantU1Left(byte a, byte b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantU1Left", "byte", "uint", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantU1Left", (uint)(a / b), b));
		}
		#endregion

		#region I2
		
		[Row(1, 2)]
		[Row(23, 21)]
		[Row(1, -2)]
		[Row(-1, 2)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(-17, -2)]
		// And reverse
		[Row(2, 1)]
		[Row(21, 23)]
		[Row(-2, 1)]
		[Row(2, -1)]
		[Row(-2, -17)]
		// (MinValue, X) Cases
		[Row(short.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(short.MinValue, 1)]
		[Row(short.MinValue, 17)]
		[Row(short.MinValue, 123)]
		[Row(short.MinValue, -0, ExpectedException = typeof(DivideByZeroException))]
		[Row(short.MinValue, -1)]
		[Row(short.MinValue, -17)]
		[Row(short.MinValue, -123)]
		// (MaxValue, X) Cases
		[Row(short.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(short.MaxValue, 1)]
		[Row(short.MaxValue, 17)]
		[Row(short.MaxValue, 123)]
		[Row(short.MaxValue, -0, ExpectedException = typeof(DivideByZeroException))]
		[Row(short.MaxValue, -1)]
		[Row(short.MaxValue, -17)]
		[Row(short.MaxValue, -123)]
		// (X, MinValue) Cases
		[Row(0, short.MinValue)]
		[Row(1, short.MinValue)]
		[Row(17, short.MinValue)]
		[Row(123, short.MinValue)]
		[Row(-0, short.MinValue)]
		[Row(-1, short.MinValue)]
		[Row(-17, short.MinValue)]
		[Row(-123, short.MinValue)]
		// (X, MaxValue) Cases
		[Row(0, short.MaxValue)]
		[Row(1, short.MaxValue)]
		[Row(17, short.MaxValue)]
		[Row(123, short.MaxValue)]
		[Row(-0, short.MaxValue)]
		[Row(-1, short.MaxValue)]
		[Row(-17, short.MaxValue)]
		[Row(-123, short.MaxValue)]
		// Extremvaluecases
		[Row(short.MinValue, short.MaxValue)]
		[Row(short.MaxValue, short.MinValue)]
		[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Test]
		public void DivI2(short a, short b)
		{
			settings.CodeSource = CreateTestCode("DivI2", "short", "int");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivI2", a / b, a, b));
		}

		[Row(-23, 21)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(short.MinValue, short.MaxValue)]
		[Test]
		public void DivConstantI2Right(short a, short b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantI2Right", "short", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantI2Right", (a / b), a));
		}

		[Row(-23, 21)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(short.MinValue, short.MaxValue)]
		[Test]
		public void DivConstantI2Left(short a, short b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantI2Left", "short", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantI2Left", (a / b), b));
		}
		#endregion

		#region U2
	
		[Row(1, 2)]
		[Row(23, 21)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		// And reverse
		[Row(2, 1)]
		[Row(21, 23)]
		// (MinValue, X) Cases
		[Row(ushort.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(ushort.MinValue, 1)]
		[Row(ushort.MinValue, 17)]
		[Row(ushort.MinValue, 123)]
		// (MaxValue, X) Cases
		[Row(ushort.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(ushort.MaxValue, 1)]
		[Row(ushort.MaxValue, 17)]
		[Row(ushort.MaxValue, 123)]
		// (X, MinValue) Cases
		[Row(0, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(1, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(17, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(123, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
		// (X, MaxValue) Cases
		[Row(0, ushort.MaxValue)]
		[Row(1, ushort.MaxValue)]
		[Row(17, ushort.MaxValue)]
		[Row(123, ushort.MaxValue)]
		// Extremvaluecases
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Row(ushort.MaxValue, ushort.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Test]
		public void DivU2(ushort a, ushort b)
		{
			settings.CodeSource = CreateTestCode("DivU2", "ushort", "uint");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivU2", (uint)(a / b), a, b));
		}

		[Row(23, 21)]
		[Row(148, 23)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Test]
		public void DivConstantU2Right(ushort a, ushort b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantU2Right", "ushort", "uint", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantU2Right", (uint)(a / b), a));
		}

		[Row(23, 21)]
		[Row(148, 23)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(ushort.MinValue, ushort.MaxValue)]
		[Test]
		public void DivConstantU2Left(ushort a, ushort b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantU2Left", "ushort", "uint", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantU2Left", (uint)(a / b), b));
		}
		#endregion

		#region I4
	
		[Row(1, 2)]
		/*[Row(23, 21)]
		[Row(1, -2)]
		[Row(-1, 2)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(-17, -2)]
		// And reverse
		[Row(2, 1)]
		[Row(21, 23)]
		[Row(-2, 1)]
		[Row(2, -1)]
		[Row(-2, -17)]
		// (MinValue, X) Cases
		[Row(int.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(int.MinValue, 1)]
		[Row(int.MinValue, 17)]
		[Row(int.MinValue, 123)]
		[Row(int.MinValue, -0, ExpectedException = typeof(DivideByZeroException))]
		[Row(int.MinValue, -1, ExpectedException = typeof(OverflowException))]
		[Row(int.MinValue, -17)]
		[Row(int.MinValue, -123)]
		// (MaxValue, X) Cases
		[Row(int.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(int.MaxValue, 1)]
		[Row(int.MaxValue, 17)]
		[Row(int.MaxValue, 123)]
		[Row(int.MaxValue, -0, ExpectedException = typeof(DivideByZeroException))]
		[Row(int.MaxValue, -1)]
		[Row(int.MaxValue, -17)]
		[Row(int.MaxValue, -123)]
		// (X, MinValue) Cases
		[Row(0, int.MinValue)]
		[Row(1, int.MinValue)]
		[Row(17, int.MinValue)]
		[Row(123, int.MinValue)]
		[Row(-0, int.MinValue)]
		[Row(-1, int.MinValue)]
		[Row(-17, int.MinValue)]
		[Row(-123, int.MinValue)]
		// (X, MaxValue) Cases
		[Row(0, int.MaxValue)]
		[Row(1, int.MaxValue)]
		[Row(17, int.MaxValue)]
		[Row(123, int.MaxValue)]
		[Row(-0, int.MaxValue)]
		[Row(-1, int.MaxValue)]
		[Row(-17, int.MaxValue)]
		[Row(-123, int.MaxValue)]
		// Extremvaluecases
		[Row(int.MinValue, int.MaxValue)]
		[Row(int.MaxValue, int.MinValue)]
		[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]*/
		[Test]
		public void DivI4(int a, int b)
		{
			settings.CodeSource = CreateTestCode("DivI4", "int", "int");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivI4", (a / b), a, b));
		}

		[Row(-23, 21)]
		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(int.MinValue, int.MaxValue)]
		[Test]
		public void DivConstantI4Right(int a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantI4Right", "int", "int", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantI4Right", (a / b), a));
		}

		[Row(-23, 21)]
		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(int.MinValue, int.MaxValue)]
		[Test]
		public void DivConstantI4Left(int a, int b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantI4Left", "int", "int", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantI4Left", (a / b), b));
		}
		#endregion

		#region U4
	
		[Row(1, 2)]
		[Row(23, 21)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		// And reverse
		[Row(2, 1)]
		[Row(21, 23)]
		// (MinValue, X) Cases
		[Row(uint.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(uint.MinValue, 1)]
		[Row(uint.MinValue, 17)]
		[Row(uint.MinValue, 123)]
		// (MaxValue, X) Cases
		[Row(uint.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(uint.MaxValue, 1)]
		[Row(uint.MaxValue, 17)]
		[Row(uint.MaxValue, 123)]
		// (X, MinValue) Cases
		[Row(0, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(1, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(17, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(123, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
		// (X, MaxValue) Cases
		[Row(0, uint.MaxValue)]
		[Row(1, uint.MaxValue)]
		[Row(17, uint.MaxValue)]
		[Row(123, uint.MaxValue)]
		// Extremvaluecases
		[Row(uint.MinValue, uint.MaxValue)]
		[Row(uint.MaxValue, uint.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Test]
		public void DivU4(uint a, uint b)
		{
			settings.CodeSource = CreateTestCode("DivU4", "uint", "uint");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivU4", (uint)(a / b), a, b));
		}

		[Row(1, 2)]
		[Row(23, 21)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(123, uint.MaxValue)]
		[Row(uint.MinValue, uint.MaxValue)]
		[Test]
		public void DivConstantU4Right(uint a, uint b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantU4Right", "uint", "uint", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantU4Right", (uint)(a / b), a));
		}

		[Row(1, 2)]
		[Row(23, 21)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(123, uint.MaxValue)]
		[Row(uint.MinValue, uint.MaxValue)]
		[Test]
		public void DivConstantU4Left(uint a, uint b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantU4Left", "uint", "uint", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantU4Left", (uint)(a / b), b));
		}
		#endregion

		#region U8
	
		[Row(1, 2)]
		[Row(23, 21)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		// And reverse
		[Row(2, 1)]
		[Row(21, 23)]
		// (MinValue, X) Cases
		[Row(ulong.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(ulong.MinValue, 1)]
		[Row(ulong.MinValue, 17)]
		[Row(ulong.MinValue, 123)]
		// (MaxValue, X) Cases
		[Row(ulong.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(ulong.MaxValue, 1)]
		[Row(ulong.MaxValue, 17)]
		[Row(ulong.MaxValue, 123)]
		// (X, MinValue) Cases
		[Row(0, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(1, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(17, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(123, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
		// (X, MaxValue) Cases
		[Row(0, ulong.MaxValue)]
		[Row(1, ulong.MaxValue)]
		[Row(17, ulong.MaxValue)]
		[Row(123, ulong.MaxValue)]
		// Extremvaluecases
		[Row(ulong.MinValue, ulong.MaxValue)]
		[Row(ulong.MaxValue, ulong.MinValue, ExpectedException = typeof(DivideByZeroException))]
		[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Test]
		public void DivU8(ulong a, ulong b)
		{
			settings.CodeSource = CreateTestCodeWithReturn("DivU8", "ulong", "ulong");
			Assert.AreEqual((ulong)(a / b), Run<ulong>(string.Empty, "Test", "DivU8", (ulong)(a / b), a, b));
		}
		#endregion

		#region I8
		
		[Row(1, 2)]
		[Row(23, 21)]
		[Row(1, -2)]
		[Row(-1, 2)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(-17, -2)]
		// And reverse
		[Row(2, 1)]
		[Row(21, 23)]
		[Row(-2, 1)]
		[Row(2, -1)]
		[Row(-2, -17)]
		// (MinValue, X) Cases
		[Row(long.MinValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(long.MinValue, 1)]
		[Row(long.MinValue, 17)]
		[Row(long.MinValue, 123)]
		[Row(long.MinValue, -0, ExpectedException = typeof(DivideByZeroException))]
		[Row(long.MinValue, -1, ExpectedException = typeof(OverflowException))]
		[Row(long.MinValue, -17)]
		[Row(long.MinValue, -123)]
		// (MaxValue, X) Cases
		[Row(long.MaxValue, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(long.MaxValue, 1)]
		[Row(long.MaxValue, 17)]
		[Row(long.MaxValue, 123)]
		[Row(long.MaxValue, -0, ExpectedException = typeof(DivideByZeroException))]
		[Row(long.MaxValue, -1)]
		[Row(long.MaxValue, -17)]
		[Row(long.MaxValue, -123)]
		// (X, MinValue) Cases
		[Row(0, long.MinValue)]
		[Row(1, long.MinValue)]
		[Row(17, long.MinValue)]
		[Row(123, long.MinValue)]
		[Row(-0, long.MinValue)]
		[Row(-1, long.MinValue)]
		[Row(-17, long.MinValue)]
		[Row(-123, long.MinValue)]
		// (X, MaxValue) Cases
		[Row(0, long.MaxValue)]
		[Row(1, long.MaxValue)]
		[Row(17, long.MaxValue)]
		[Row(123, long.MaxValue)]
		[Row(-0, long.MaxValue)]
		[Row(-1, long.MaxValue)]
		[Row(-17, long.MaxValue)]
		[Row(-123, long.MaxValue)]
		// Extremvaluecases
		[Row(long.MinValue, long.MaxValue)]
		[Row(long.MaxValue, long.MinValue)]
		[Row(1, 0, ExpectedException = typeof(DivideByZeroException))]
		[Test]
		public void DivI8(long a, long b)
		{
			settings.CodeSource = CreateTestCode("DivI8", "long", "long");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivI8", (a / b), a, b));
		}

		[Row(-23, 21)]
		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(long.MinValue, long.MaxValue)]
		[Test]
		public void DivConstantI8Right(long a, long b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantI8Right", "long", "long", null, b.ToString());
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantI8Right", (a / b), a));
		}

		[Row(-23, 21)]
		[Row(-23, 148)]
		[Row(17, 1)]
		[Row(0, 0, ExpectedException = typeof(DivideByZeroException))]
		[Row(long.MinValue, long.MaxValue)]
		[Test]
		public void DivConstantI8Left(long a, long b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantI8Left", "long", "long", a.ToString(), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantI8Left", (a / b), b));
		}

		#endregion

		#region R4
		
		[Row(1.0f, 2.0f)]
		[Row(2.0f, 1.0f)]
		[Row(1.0f, 2.5f)]
		[Row(1.7f, 2.3f)]
		[Row(2.0f, -1.0f)]
		[Row(1.0f, -2.5f)]
		[Row(1.7f, -2.3f)]
		[Row(-2.0f, 1.0f)]
		[Row(-1.0f, 2.5f)]
		[Row(-1.7f, 2.3f)]
		[Row(-2.0f, -1.0f)]
		[Row(-1.0f, -2.5f)]
		[Row(-1.7f, -2.3f)]
		[Row(1.0f, float.NaN)]
		[Row(float.NaN, 1.0f)]
		[Row(1.0f, float.PositiveInfinity)]
		[Row(float.PositiveInfinity, 1.0f)]
		[Row(1.0f, float.NegativeInfinity)]
		[Row(float.NegativeInfinity, 1.0f)]
		[Test]
		public void DivR4(float a, float b)
		{
			settings.CodeSource = CreateTestCode("DivR4", "float", "float");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivR4", (a / b), a, b));
		}

		[Row(23f, 148.0016f)]
		[Row(17.2f, 1f)]
		[Row(0f, 0f)]
		//[Row(float.MinValue, float.MaxValue)]
		[Test]
		public void DivConstantR4Right(float a, float b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantR4Right", "float", "float", null, b.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantR4Right", (a / b), a));
		}

		[Row(23f, 148.0016f)]
		[Row(17.2f, 1f)]
		[Row(0f, 0f)]
		//[Row(float.MinValue, float.MaxValue)]
		[Test]
		public void DivConstantR4Left(float a, float b)
		{

			settings.CodeSource = CreateConstantTestCode("DivConstantR4Left", "float", "float", a.ToString(System.Globalization.CultureInfo.InvariantCulture) + "f", null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantR4Left", (a / b), b));
		}
		#endregion

		#region R8
		
		[Row(1.0, 2.0)]
		[Row(2.0, 1.0)]
		[Row(1.0, 2.5)]
		[Row(1.7, 2.3)]
		[Row(2.0, -1.0)]
		[Row(1.0, -2.5)]
		[Row(1.7, -2.3)]
		[Row(-2.0, 1.0)]
		[Row(-1.0, 2.5)]
		[Row(-1.7, 2.3)]
		[Row(-2.0, -1.0)]
		[Row(-1.0, -2.5)]
		[Row(-1.7, -2.3)]
		[Row(1.0, double.NaN)]
		[Row(double.NaN, 1.0)]
		[Row(1.0, double.PositiveInfinity)]
		[Row(double.PositiveInfinity, 1.0)]
		[Row(1.0, double.NegativeInfinity)]
		[Row(double.NegativeInfinity, 1.0)]
		[Row(1.0, 0.0)]
		[Test]
		public void DivR8(double a, double b)
		{
			settings.CodeSource = CreateTestCode("DivR8", "double", "double");
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivR8", (a / b), a, b));
		}

		[Row(23, 148.0016)]
		[Row(17.2, 1.0)]
		[Row(0.0, 0.0)]
		[Row(-1.79769313486231E+308, 1.79769313486231E+308)]
		[Test]
		public void DivConstantR8Right(double a, double b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantR8Right", "double", "double", null, b.ToString(System.Globalization.CultureInfo.InvariantCulture));
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantR8Right", (a / b), a));
		}

		[Row(23, 148.0016)]
		[Row(17.2, 1.0)]
		[Row(0.0, 0.0)]
		[Row(-1.79769313486231E+308, 1.79769313486231E+308)]
		[Test]
		public void DivConstantR8Left(double a, double b)
		{
			settings.CodeSource = CreateConstantTestCode("DivConstantR8Left", "double", "double", a.ToString(System.Globalization.CultureInfo.InvariantCulture), null);
			Assert.IsTrue(Run<bool>(string.Empty, "Test", "DivConstantR8Left", (a / b), b));
		}
		#endregion
	}
}
