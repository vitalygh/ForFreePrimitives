using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
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

		private readonly (int[][], int, int, int[][])[] matrixTestcases = new[]
		{
			(new [] {
				new [] { 0, 1 },
				new [] { 1, 0 },
			},
			1000000000,
			1000000007,
			new [] {
				new [] { 1, 0 },
				new [] { 0, 1 },
			}),
			(new [] {
				new [] { 0, 999999999 },
				new [] { 999999999, 0 },
			},
			2,
			1000000007,
			new [] {
				new [] { 64, 0 },
				new [] { 0, 64 },
			}),
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
			foreach (var (val, exp, mod, result) in matrixTestcases)
				Validate(val, exp, mod, result);
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
			Assert.IsTrue(BinExp.BinPow(val, exp) == BigInteger.Pow(val, exp), $"BinExp.BinPow({val}, {exp})");
			Assert.IsTrue(BinExp.BinPow(val, exp, mod) == BigInteger.ModPow(val, exp, mod), $"BinExp.BinPow({val}, {exp}, {mod})");
			if (BigInteger.Pow(val, exp) <= long.MaxValue)
				Assert.IsTrue(BinExp.BinPow((long)val, exp) == BigInteger.Pow(val, exp), $"BinExp.BinPow({val}, {exp})");
			Assert.IsTrue(BinExp.BinPow((long)val, exp, (long)mod) == BigInteger.ModPow(val, exp, mod), $"BinExp.BinPow({val}, {exp}, {mod})");
			if (BigInteger.Pow(val, exp) <= int.MaxValue)
				Assert.IsTrue(BinExp.BinPow((int)val, exp) == BigInteger.Pow(val, exp), $"BinExp.BinPow({val}, {exp})");
			Assert.IsTrue(BinExp.BinPow((int)val, exp, (int)mod) == BigInteger.ModPow(val, exp, mod), $"BinExp.BinPow({val}, {exp}, {mod})");
		}

		private void Validate(int[][] val, int exp, int mod, int[][] result)
		{
			var test = BinExp.BinPow(val, exp, mod);
			Assert.IsTrue(test.Length == result.Length, "Incorrect result matrix size");
			if (test.Length == result.Length)
				for (var i = 0; i < test.Length; i += 1)
					Assert.IsTrue(Enumerable.SequenceEqual(test[i], result[i]), $"Incorrect matrix row {i}");
		}
	}
}

