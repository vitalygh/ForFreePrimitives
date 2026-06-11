using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ForFreePrimitives;

namespace ForFreePrimitivesTests
{
	[TestClass]
	public class SparseTableTests
	{
		private static readonly Random random = new Random();

		[TestMethod]
		public void Tests()
		{
			ProcessDefinedTestCases();
			ProcessRandomTestCases();
		}

		private static readonly int[][] sparseTableTestcases = new[]
		{
			new[] { 26,97,44,38,68,10,91,9,79,10 },			
		};

		private void ProcessRandomTestCases()
		{
			var testsCount = 100;
			var numsCount = 10;
			var minValue = 1;
			var maxValue = 100;
			for (var i = 0; i < testsCount; i += 1)
			{
				var nums = new int[numsCount];
				for (var j = 0; j < nums.Length; j += 1)
					nums[j] = random.Next(minValue, maxValue);
				Validate(nums);
			}
		}

		private void ProcessDefinedTestCases()
		{
			foreach (var testcase in sparseTableTestcases)
				Validate(testcase);
		}

		private void Validate(int[] nums)
		{
			var minSparseTable = new MinSparseTable<int>(nums);
			var maxSparseTable = new MaxSparseTable<int>(nums);
			var gcdSparseTable = new GCDSparseTable(nums);
			for (var l = 0; l < nums.Length; l += 1)
			{
				var min = nums[l];
				var max = nums[l];
				var gcd = nums[l];
				for (var r = l; r < nums.Length; r += 1)
				{
					min = Math.Min(min, nums[r]);
					max = Math.Max(max, nums[r]);
					gcd = Numbers.GCD(gcd, nums[r]);
					var spmin = minSparseTable.Query(l, r);
					if (spmin != min)
						Assert.Fail($"new[] {{ {Tools.Dump(nums)} }}, min [{l}..{r}] {spmin} != {min}");
					var spmax = maxSparseTable.Query(l, r);
					if (spmax != max)
						Assert.Fail($"new[] {{ {Tools.Dump(nums)} }}, max [{l}..{r}] {spmax} != {max}");
					var spgcd = gcdSparseTable.Query(l, r);
					if (spgcd != gcd)
						Assert.Fail($"new[] {{ {Tools.Dump(nums)} }}, gcd [{l}..{r}] {spgcd} != {gcd}");
				}
			}
		}
	}
}