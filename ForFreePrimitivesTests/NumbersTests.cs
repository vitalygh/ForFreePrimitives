using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ForFreePrimitives;

namespace ForFreePrimitivesTests
{
	[TestClass]
	public class NumbersTests
	{
		[TestMethod]
		public void Tests()
		{
			ProcessTestCases();
		}

		private readonly int[][] testcasesGCD = new[]
		{
			new int[] { 12, 18, 6 },
			new int[] { 28, 36, 4 },
			new int[] { 35, 133, 7 },
			new int[] { 12, 15, 3 },
			new int[] { 2145, 214568, 1 },
			new int[] { 429136, 214568, 214568 },
		};

		private void ProcessTestCases()
		{
			ProcessDefinedGCDTestCases();
		}

		private void ProcessDefinedGCDTestCases()
		{
			foreach (var testcase in testcasesGCD)
				ValidateGCD(testcase);
		}

		private void ValidateGCD(int[] testcase)
		{
			var a = testcase[0];
			var b = testcase[1];
			var result = testcase[2];
			Assert.IsTrue(Numbers.GCD(a, b) == result, $"Numbers.GCD({a}, {b}) == {result}");
		}
	}

}
