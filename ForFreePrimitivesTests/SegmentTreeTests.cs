using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Collections.Generic;
using ForFreePrimitives;

namespace ForFreePrimitivesTests
{
	[TestClass]
	public class SegmentTreeTests
	{
		[TestMethod]
		public void Tests()
		{
			ProcessDefinedTestCases();
			ProcessRandomTestCases();
		}

		private static Random random = new Random();

		private readonly (int[] target, int[] available, int missingCount)[] maxSegmentTreeTestcases = new[]
		{
			(new int[] { 4,2,5 },
			new int[] { 3,5,4 },
			1),
			(new int[] { 3,6,1 },
			new int[] { 6,4,7 },
			0),
			(new int[] { 215, 99, 907, 682, 222, 301, 253, 834, 452, 163, 712, 272, 568, 338, 392, 296, 671, 373, 531, 822, 111, 848, 756, 589, 795, 851, 164, 360, 860, 779, 735, 162, 219, 801, 246, 112, 611 },
			new int[] { 287, 33, 183, 641, 669, 841, 813, 580, 165, 46, 574, 823, 171, 662, 831, 333, 142, 351, 313, 334, 575, 412, 353, 264, 307, 152, 763, 425, 469, 544, 980, 564, 911, 926, 231, 246, 247 },
			10),
			(new int[] { 560, 84, 525, 549, 129, 391, 458, 358, 73, 207, 473, 598, 678, 435, 19, 138, 965, 701, 368, 606, 287, 860, 80, 320, 15, 905, 967, 826, 508, 456, 465, 970, 992, 488, 57, 989, 57, 337, 574, 570, 400, 652, 521, 262, 161, 463, 283, 938, 982, 363, 91, 37, 950, 778, 1000, 526, 98 },
			new int[] { 843, 538, 593, 857, 639, 348, 223, 934, 272, 736, 48, 392, 653, 82, 741, 995, 611, 626, 89, 946, 961, 244, 191, 585, 874, 414, 466, 855, 458, 366, 449, 340, 280, 308, 408, 713, 446, 411, 52, 617, 156, 312, 85, 748, 461, 325, 256, 269, 389, 717, 746, 560, 261, 651, 604, 716, 469 },
			12),
			(new int[] { 667, 107, 57, 806, 744, 824, 10, 103, 445, 70, 643, 883, 583, 188, 745, 222, 206, 507, 758, 700, 563, 229, 555, 854, 499, 670, 187, 230, 908, 493, 359, 455, 221, 586, 683, 445, 48 },
			new int[] { 813, 204, 990, 765, 51, 892, 774, 217, 391, 283, 311, 919, 420, 47, 719, 857, 899, 640, 140, 656, 853, 755, 8, 67, 400, 709, 502, 51, 165, 708, 893, 510, 622, 963, 742, 518, 96 },
			6),
		};

		private readonly (int[] nums, int[][] segments)[] lengthSegmentTreeTestcases = new[]
		{
			(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }, new int[][]
			{ 
				new int[] { 1, 1, 1, 1 },
				new int[] { 2, 3, 1, 3 },
				new int[] { 1, 3, 1, 3 },
				new int[] { 1, 3, -1, 3 },
				new int[] { 1, 2, -1, 1 },
				new int[] { 3, 3, -1, 0 },
			}),			
		};

		private readonly int[][][] segmentTreeTestcases = new[]
		{
			new[]
			{
				new int[] {	10,	20,	1 },
				new int[] {	50,	60,	1 },
				new int[] {	10,	40,	2 },
				new int[] {	5,	15,	3 },
				new int[] {	5,	10,	3 },
				new int[] {	25,	55,	3 },
			},
			new[]
			{
				new int[] { 24,	40, 1 },
				new int[] { 43,	50, 1 },
				new int[] { 27,	43, 2 },
				new int[] { 5,	21, 2 },
				new int[] { 30,	40, 3 },
				new int[] { 14,	29, 3 },
				new int[] { 3,	19, 3 },
				new int[] { 3,	14, 3 },
				new int[] { 25,	39, 4 },
				new int[] { 6,	19, 4 },
			},
		};

		private void ProcessDefinedTestCases()
		{
			foreach (var testcase in segmentTreeTestcases)
				Validate(testcase);
			foreach (var (target, available, missingCount) in maxSegmentTreeTestcases)
				ValidateMax(target, available, missingCount);
			foreach (var (nums, segments) in lengthSegmentTreeTestcases)
				ValidateLength(nums, segments);
		}

		private void ProcessRandomTestCases()
		{
			ValidateMinMaxInBounds();
		}

		private string Dump(int[] nums, int left, int right)
		{
			var sb = new StringBuilder();
			for (var i = left; i <= Math.Min(right, nums.Length - 1); i += 1)
			{
				if (sb.Length > 0)
					sb.Append(",");
				sb.Append(nums[i]);
			}
			return sb.ToString();
		}

		private void ValidateMinMaxInBounds()
		{
			var length = 20;
			var minValue = int.MinValue;
			var maxValue = int.MaxValue;
			var testData = new int[length];
			for (var i = 0; i < testData.Length; i += 1)
				testData[i] = random.Next(minValue, maxValue);
			var maxTree = new MaxSegmentTree<int>(testData);
			var minTree = new MinSegmentTree<int>(testData);
			for (var i = 0; i < testData.Length; i += 1)
				for (var j = i; j < testData.Length; j += 1)
				{
					var max = testData[i];
					var min = testData[i];
					var firstGreaterTarget = random.Next(minValue, maxValue);
					var firstGreater = testData[i] > firstGreaterTarget ? i : -1;
					for (var k = i + 1; k <= j; k += 1)
					{
						max = Math.Max(max, testData[k]);
						min = Math.Min(min, testData[k]);
						if ((firstGreater < 0) && (testData[k] > firstGreaterTarget))
							firstGreater = k;
					}
					var treeMax = maxTree.GetValueInBounds(i, j);
					var treeMin = minTree.GetValueInBounds(i, j);
					var treeFirstGreater = maxTree.GetFirstGreater(firstGreaterTarget, i, j);
					Assert.IsTrue(max == treeMax, $"[{Dump(testData, 0, testData.Length - 1)}] [{i},{j}] max {max} != {treeMax}");
					Assert.IsTrue(min == treeMin, $"[{Dump(testData, 0, testData.Length - 1)}] [{i},{j}] min {min} != {treeMin}");
					Assert.IsTrue(firstGreater == treeFirstGreater, $"[{Dump(testData, 0, testData.Length - 1)}] [{i},{j}] first greater for {firstGreaterTarget} is {firstGreater} != {treeFirstGreater}");
				}
		}

		private void ValidateMax(int[] target, int[] available, int missingCount)
		{
			var tree = new MaxSegmentTree<int>(available);
			var missing = 0;
			for (var i = 0; i < target.Length; i += 1)
			{
				var index = tree.GetFirstGreater(target[i] - 1);
				if (index < 0)
					missing += 1;
				else
					tree.Update(index, 0);
			}
			Assert.IsTrue(missing == missingCount, $"{missing} != {missingCount}");
		}

		private void ValidateLength(int[] nums, int[][] segments)
		{
			var tree = new LengthSegmentTree(nums);
			for (var i = 0; i < segments.Length; i += 1)
			{
				var s = segments[i];
				tree.Update(s[0], s[1], s[2]);
				Assert.IsTrue(tree.Length == s[3], $"{i}. {s[3]} != {tree.Length}");
			}
		}

		private void Validate(int[][] testcase)
		{
			var min = int.MaxValue;
			var max = int.MinValue;
			foreach (var segment in testcase)
			{
				min = Math.Min(min, segment[0]);
				max = Math.Max(max, segment[1]);
			}
			var tree = new SegmentTree(min, max);
			var maxCount = 0;
			foreach (var segment in testcase)
			{
				var start = segment[0];
				var end = segment[1];
				var targetMaxCount = segment[2];
				tree.Add(start, end);
				maxCount = Math.Max(maxCount, tree.Query(start, end));
				Assert.IsTrue(maxCount == targetMaxCount);
			}
		}
	}
}

