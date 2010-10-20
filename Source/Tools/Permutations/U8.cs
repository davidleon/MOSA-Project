﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Mosa.Tools.Permutations
{
	public static class U8
	{
		public static string Type = "ulong";
		public static byte Bits = 64;

		private static IList<string> allPermutations = null;
		public static IList<string> AllPermutations { get { if (allPermutations == null) allPermutations = GetAllPermutations(); return allPermutations; } }

		public static IList<string> GetAllPermutations()
		{
			IList<string> list = new List<string>();

			list.AddIfNew(0.ToString());
			list.AddIfNew(1.ToString());
			//list.AddIfNew(2.ToString());
			list.AddIfNew(ulong.MinValue.ToString());
			list.AddIfNew(ulong.MaxValue.ToString());
			list.AddIfNew((ulong.MinValue + 1).ToString());
			list.AddIfNew((ulong.MaxValue - 1).ToString());
			list.AddIfNew(uint.MinValue.ToString());
			list.AddIfNew(uint.MaxValue.ToString());
			list.AddIfNew((uint.MinValue + 1).ToString());
			list.AddIfNew((uint.MaxValue - 1).ToString());

			//list.AddIfNew(17.ToString());
			//list.AddIfNew(123.ToString());

			// power of two numbers
			//list.AddIfNew(Power2.GetPowerTwos(ulong.MinValue, ulong.MaxValue));

			//// a few prime numbers
			//list.AddIfNew(Prime.GetSomePrimes(ulong.MinValue, ulong.MaxValue));

			return list;
		}

		public static IList<string> GetPermutationResults()
		{
			List<string> results = new List<string>();

			results.Add(NumericTemplates.CreateAdd(Type, AllPermutations, AllPermutations, false));
			results.Add(NumericTemplates.CreateSub(Type, AllPermutations, AllPermutations, false));
			results.Add(NumericTemplates.CreateMul(Type, AllPermutations, AllPermutations, false));
			results.Add(NumericTemplates.CreateDiv(Type, AllPermutations, AllPermutations, false, true));

			results.Add(
				Adjustments.CommentOut(
					NumericTemplates.CreateRem(Type, AllPermutations, AllPermutations, false),
					"[Row(UInt64.MaxValue - 1, UInt64.MaxValue)]", "Crashes test runner"
				)
				);
			results.Add(NumericTemplates.CreateRet(Type, AllPermutations, false));

			results.Add(NumericTemplates.CreateAnd(Type, AllPermutations, AllPermutations, false));
			results.Add(NumericTemplates.CreateOr(Type, AllPermutations, AllPermutations, false));
			results.Add(NumericTemplates.CreateXor(Type, AllPermutations, AllPermutations, false));

			results.Add(NumericTemplates.CreateComp(Type, AllPermutations, false));

			//results.Add(NumericTemplates.CreateShl(Type, AllPermutations, Upto.GetUpto(Bits), false));
			//results.Add(NumericTemplates.CreateShr(Type, AllPermutations, Upto.GetUpto(Bits), false));

			results.Add(NumericTemplates.CreateCeq(Type, AllPermutations, AllPermutations, false));
			results.Add(NumericTemplates.CreateCgt(Type, AllPermutations, AllPermutations, false));
			results.Add(NumericTemplates.CreateClt(Type, AllPermutations, AllPermutations, false));
			results.Add(NumericTemplates.CreateCge(Type, AllPermutations, AllPermutations, false));
			results.Add(NumericTemplates.CreateCle(Type, AllPermutations, AllPermutations, false));

			results.Add(ArrayTemplates.CreateNewarr(Type, false));
			results.Add(ArrayTemplates.CreateLdlen(Type, U1.SmallArrayPermutations, false));
			results.Add(ArrayTemplates.CreateStelem(Type, U1.SmallArrayPermutations, AllPermutations, false));
			results.Add(ArrayTemplates.CreateLdelem(Type, U1.SmallArrayPermutations, AllPermutations, false));
			results.Add(ArrayTemplates.CreateLdelema(Type, U1.SmallArrayPermutations, AllPermutations, false));

			return results;
		}
	}
}
