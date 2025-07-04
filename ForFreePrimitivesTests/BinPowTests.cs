using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Numerics;
using ForFreePrimitives;

namespace ForFreePrimitivesTests
{
	[TestClass]
	public class BinPowTests
	{
		private static Random random = new Random();

		[TestMethod]
		public void Tests()
		{
			ProcessTestCases();
		}

		private readonly int[][] testcases = new[]
		{
			new int[] { 0,  1,	1 },
			new int[] { 2, 10, 33 },
			new int[] { 8, 17, 1 },
			new int[] { 99, 99, 99 },
			new int[] { 11, 12, 13 },
		};

		private void ProcessTestCases()
		{
			ProcessDefinedTestCases();
			ProcessRandomTestCases();
		}

		private void ProcessDefinedTestCases()
		{
			foreach (var testcase in testcases)
				Validate(testcase);
		}

		private void ProcessRandomTestCases()
		{
			var count = 100;
			var minValue = 0;
			var maxValue = 100;
			var minExp = 0;
			var maxExp = 100;
			var minMod = 1;
			var maxMod = int.MaxValue;
			for (var i = 0; i < count; i += 1)
			{
				var val = random.Next(minValue, maxValue);
				var exp = random.Next(minExp, maxExp);
				var mod = random.Next(minMod, maxMod);
				var testcase = new int[] { val, exp, mod };
				Validate(testcase);
			}
		}

		private void Validate(int[] testcase)
		{
			var val = new BigInteger(testcase[0]);
			var exp = testcase[1];
			var mod = new BigInteger(testcase[2]);
			Assert.IsTrue(BinPow.Calc(val, exp) == BigInteger.Pow(val, exp), $"BinPow.Calc({val}, {exp})");
			Assert.IsTrue(BinPow.Calc(val, exp, mod) == BigInteger.ModPow(val, exp, mod), $"BinPow.Calc({val}, {exp}, {mod})");
			if (BigInteger.Pow(val, exp) <= long.MaxValue)
				Assert.IsTrue(BinPow.Calc((long)val, exp) == BigInteger.Pow(val, exp), $"BinPow.Calc({val}, {exp})");
			Assert.IsTrue(BinPow.Calc((long)val, exp, (long)mod) == BigInteger.ModPow(val, exp, mod), $"BinPow.Calc({val}, {exp}, {mod})");
			if (BigInteger.Pow(val, exp) <= int.MaxValue)
				Assert.IsTrue(BinPow.Calc((int)val, exp) == BigInteger.Pow(val, exp), $"BinPow.Calc({val}, {exp})");
			Assert.IsTrue(BinPow.Calc((int)val, exp, (int)mod) == BigInteger.ModPow(val, exp, mod), $"BinPow.Calc({val}, {exp}, {mod})");
		}
	}
}

