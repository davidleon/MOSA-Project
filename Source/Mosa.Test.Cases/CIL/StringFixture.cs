﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Michael Fröhlich (grover) <michael.ruck@michaelruck.de>
 *  
 */

using MbUnit.Framework;

using Mosa.Test.Runtime.CompilerFramework;

namespace Mosa.Test.Cases.CIL
{

	[TestFixture]
	[Importance(Importance.Critical)]
	//[Category(@"Basic types")]
	//[Description(@"Tests support for the basic type System.String")]
	public class StringFixture : TestCompilerAdapter
	{

		public StringFixture()
		{
			settings.AddReference("Mosa.Test.Collection.dll");
		}

		[Test]
		public void MustProperlyCompileLdstrAndLengthMustMatch()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StringTests", "LengthMustMatch"));
		}

		[Test]
		public void FirstCharacterMustMatchInStrings()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StringTests", "FirstCharacterMustMatch"));
		}

		[Test]
		public void LastCharacterMustMatchInStrings()
		{
			Assert.IsTrue(Run<bool>("Mosa.Test.Collection", "StringTests", "LastCharacterMustMatch"));
		}
	}
}
