using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ForFreePrimitives;

namespace ForFreePrimitivesTests
{
	[TestClass]
	public class RBTreeTests
	{
		private static Random random = new Random();

		[TestMethod]
		public void Tests()
		{
			ProcessTestCases();
		}

		private readonly int[][] testcases = new[]
		{
			new int[] { },
			new int[] { 1, },
			new int[] { 1,  2,  3 },
			new int[] { 9,  8,  7 },
			new int[] { 5,  5,  5 },
			new int[] { -1, 0,  1 },
			new int[] { 0,  0,  1 },
			new int[] { 0,  1,  0 },
			new int[] { 1,  0,  0 },
			new int[] { -8, -5, 8, 3, 9, 1, -9, -1, 0, -6 },
			new int[] { 1, 1, 1, 0, -1, -1, -2, 1, -1, 1, 0, -1 },
			new int[] { -1, -2, 1, -2, 0, -2, 0, 1, 0 },
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
			var minCount = 0;
			var maxCount = 20;
			var minValue = int.MinValue;
			var maxValue = int.MaxValue;
			for (var count = minCount; count <= maxCount; count += 1)
			{
				var testcase = new int[count];
				for (var i = 0; i < testcase.Length; i += 1)
					testcase[i] = random.Next(minValue, maxValue);
				Validate(testcase);
			}
			minValue = -2;
			maxValue = 2;
			for (var count = minCount; count <= maxCount; count += 1)
			{
				var testcase = new int[count];
				for (var i = 0; i < testcase.Length; i += 1)
					testcase[i] = random.Next(minValue, maxValue);
				Validate(testcase);
			}
		}

		private void Validate(int[] testcase)
		{
			var tree = new RBTree<int>();
			var set = new SortedSet<int>();
			var counter = new Dictionary<int, int>();
			var minValue = 0;
			var maxValue = 0;
			for (var i = 0; i < testcase.Length; i += 1)
			{
				var num = testcase[i];
				if (i == 0)
				{
					minValue = num;
					maxValue = num;
				}
				else
				{
					minValue = Math.Min(minValue, num);
					maxValue = Math.Max(maxValue, num);
				}
				if (counter.TryGetValue(num, out var count))
				{
					count += 1;
					counter[num] = count;
				}
				else
				{
					count = 1;
					counter.Add(num, count);
					set.Add(num);
				}
				tree.Add(num);
				var treeCount = tree.GetCount(num);
				if (treeCount != count)
				Assert.Fail($"[{Tools.Dump(testcase)}] at {i}: {count} != {treeCount}");
				if (!tree.IsValid())
					Assert.Fail($"[{Tools.Dump(testcase)}] at {i} validation failed");
				if (tree.Min != minValue)
				Assert.Fail($"[{Tools.Dump(testcase)}] at {i}: {minValue} != {tree.Min}");
				if (tree.Max != maxValue)
					Assert.Fail($"[{Tools.Dump(testcase)}] at {i}: {maxValue} != {tree.Max}");
			}
			var sorted = new int[testcase.Length];
			Array.Copy(testcase, 0, sorted, 0, testcase.Length);
			Array.Sort(sorted);
			if (!Enumerable.SequenceEqual(sorted, tree))
				Assert.Fail($"[{Tools.Dump(sorted)}] != [{Tools.Dump(tree.ToArray())}]");
			var lesserCount = 0;
			for (var i = 0; i < sorted.Length; i += 1)
			{
				if ((i > 0) && (sorted[i - 1] != sorted[i]))
				{
					lesserCount = i;
					Assert.IsTrue(tree.TryGetLesser(sorted[i], out var lesser) && (lesser == sorted[i - 1]));
				}
				Assert.IsTrue(tree.GetLesserCount(sorted[i]) == lesserCount);
			}
			var greaterCount = 0;
			for (var i = sorted.Length - 1; i >= 0; i -= 1)
			{
				if ((i < sorted.Length - 1) && (sorted[i + 1] != sorted[i]))
				{
					greaterCount = sorted.Length - i - 1;
					Assert.IsTrue(tree.TryGetGreater(sorted[i], out var greater) && (greater == sorted[i + 1]));
				}
				Assert.IsTrue(tree.GetGreaterCount(sorted[i]) == greaterCount);
			}
			for (var i = 0; i < testcase.Length; i += 1)
			{
				var num = testcase[i];
				tree.Remove(num);
				if (!tree.IsValid())
					Assert.Fail($"[{Tools.Dump(testcase)}] at {i}");
			}
			for (var i = 0; i < testcase.Length; i += 1)
			{
				var num = testcase[i];
				tree.Add(num);
				if (!tree.IsValid())
					Assert.Fail($"[{Tools.Dump(testcase)}] at {i}");
			}
			var shuffle = new int[testcase.Length];
			Array.Copy(testcase, 0, shuffle, 0, testcase.Length);
			for (var i = 0; i < shuffle.Length; i += 1)
			{
				var index = random.Next(i, shuffle.Length);
				(shuffle[i], shuffle[index]) = (shuffle[index], shuffle[i]);
			}
			for (var i = 0; i < shuffle.Length; i += 1)
			{
				var num = shuffle[i];
				counter[num] -= 1;
				if (counter[num] < 1)
					set.Remove(num);
				Assert.IsTrue(tree.Count == shuffle.Length - i);
				tree.Remove(num);
				Assert.IsTrue(tree.Count == shuffle.Length - i - 1);
				if (!tree.IsValid())
					Assert.Fail($"[{Tools.Dump(testcase)}] [{Tools.Dump(shuffle.ToArray(), i + 1)}] remove at {i}");
				if (tree.Count > 0)
				{
					if (tree.Min != set.Min)
						Assert.Fail($"[{Tools.Dump(testcase)}] [{Tools.Dump(shuffle.ToArray(), i + 1)}]: {set.Min} != {tree.Min}");
					if (tree.Max != set.Max)
						Assert.Fail($"[{Tools.Dump(testcase)}] [{Tools.Dump(shuffle.ToArray(), i + 1)}]: {set.Max} != {tree.Max}");
				}				
			}
		}
	}
}
