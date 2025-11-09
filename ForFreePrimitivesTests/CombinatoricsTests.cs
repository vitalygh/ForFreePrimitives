using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ForFreePrimitives;

namespace ForFreePrimitivesTests
{
	[TestClass]
	public class CombinatoricsTests
	{
		[TestMethod]
		public void Tests()
		{
			ProcessTestCases();
		}

		private readonly int[][] testcases = new[]
		{
			new int[] { 20, 10, 1000000007, 184756 },
			new int[] { 100, 50, 1000000007, 538992043 },
			new int[] { 80, 44, 1000000007, 587982998 },
			new int[] { 77, 12, 1000000007, 278791162 },
		};

		private void ProcessTestCases()
		{
			ProcessDefinedTestCases();
		}

		private void ProcessDefinedTestCases()
		{
			foreach (var testcase in testcases)
				Validate(testcase);
		}

		private void Validate(int[] testcase)
		{
			var n = testcase[0];
			var k = testcase[1];
			var modulo = testcase[2];
			var result = testcase[3];
			var combinatorics = new Combinatorics();
			combinatorics.InitFactorials(Math.Max(n, k), modulo);
			Assert.IsTrue(combinatorics.Cnk(n, k) == result, $"combinatorics.Cnk({n}, {k}) == {result}");
		}
	}
}
