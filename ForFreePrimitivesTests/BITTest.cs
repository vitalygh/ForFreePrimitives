using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ForFreePrimitives;

namespace ForFreePrimitivesTests
{
	[TestClass]
	public class BITTests
	{
		[TestMethod]
		public void Tests()
		{
			ProcessDefinedSumTestCases();
			ProcessDefinedMinTestCases();
			ProcessDefinedMaxTestCases();
		}

		private readonly (int[] nums, int[] interval, int result)[] sumTestcases = new[]
		{
			(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new int[] { 2, 4 }, 12),
			(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new int[] { 0, 8 }, 45),
			(new int[] { 1, 2, 5, 4, 3, 6, 7, 8, 9 }, new int[] { 2, 4 }, 12),
			(new int[] { 1, 2, 5, 5, 5, 6, 7, 8, 9 }, new int[] { 1, 5 }, 23),
		};

		private readonly (int[] nums, int position, int result)[] minTestcases = new[]
		{
			(new int[] { 4, 3, 2, 6, 1, 8, 9, 7, 5 }, 0, 4),
			(new int[] { 4, 3, 2, 6, 1, 8, 9, 7, 5 }, 8, 1),
			(new int[] { 4, 3, 2, 6, 1, 8, 9, 7, 5 }, 3, 2),
			(new int[] { 3, 3, 2, 6, 1, 8, 9, 7, 5 }, 1, 3),
			(new int[] { 2, 3, 2, 6, 1, 8, 9, 7, 5 }, 1, 2),
		};

		private readonly (int[] nums, int position, int result)[] maxTestcases = new[]
		{
			(new int[] { 4, 1, 3, 2, 6, 8, 9, 7, 5 }, 0, 4),
			(new int[] { 4, 1, 3, 2, 6, 8, 9, 7, 5 }, 8, 9),
			(new int[] { 4, 1, 3, 2, 6, 8, 9, 7, 5 }, 3, 4),
			(new int[] { 4, 5, 3, 2, 6, 8, 9, 7, 5 }, 3, 5),
			(new int[] { 6, 5, 3, 2, 6, 8, 9, 7, 5 }, 3, 6),
		};


		private void ProcessDefinedSumTestCases()
		{
			int[] data = null;
			SumBIT tree = null;
			for (var i = 0; i < sumTestcases.Length; i += 1)
			{
				var (nums, interval, answer) = sumTestcases[i];
				if ((data == null) || (data.Length != nums.Length))
				{
					data = new int[nums.Length];
					tree = new SumBIT(data.Length);
				}
				for (var j = 0; j < nums.Length; j += 1)
					if (data[j] != nums[j])
					{
						tree.Update(j + 1, nums[j] - data[j]);
						data[j] = nums[j];
					}
				var a = interval[0];
				var b = interval[1];
				var result = tree.Query(b + 1);
				if (a > 0)
					result -= tree.Query(a);
				Assert.IsTrue(result == answer, $"Testcase {i}: sum [{a}..{b}] != {answer}");
			}
		}

		private void ProcessDefinedMinTestCases()
		{
			int[] data = null;
			MinBIT tree = null;
			for (var i = 0; i < minTestcases.Length; i += 1)
			{
				var (nums, position, answer) = minTestcases[i];
				if ((data == null) || (data.Length != nums.Length))
				{
					data = new int[nums.Length];
					for (var j = 0; j < data.Length; j += 1)
						data[j] = int.MaxValue;
					tree = new MinBIT(data.Length);
				}
				for (var j = 0; j < nums.Length; j += 1)
					if (data[j] > nums[j])
					{
						tree.Update(j + 1, nums[j]);
						data[j] = nums[j];
					}
				Assert.IsTrue(tree.Query(position + 1) == answer, $"Testcase {i}: min [0..{position}] != {answer}");
			}
		}

		private void ProcessDefinedMaxTestCases()
		{
			int[] data = null;
			MaxBIT tree = null;
			for (var i = 0; i < maxTestcases.Length; i += 1)
			{
				var (nums, position, answer) = maxTestcases[i];
				if ((data == null) || (data.Length != nums.Length))
				{
					data = new int[nums.Length];
					for (var j = 0; j < data.Length; j += 1)
						data[j] = int.MinValue;
					tree = new MaxBIT(data.Length);
				}
				for (var j = 0; j < nums.Length; j += 1)
					if (data[j] < nums[j])
					{
						tree.Update(j + 1, nums[j]);
						data[j] = nums[j];
					}
				Assert.IsTrue(tree.Query(position + 1) == answer, $"Testcase {i}: max [0..{position}] != {answer}");
			}
		}
	}
}

