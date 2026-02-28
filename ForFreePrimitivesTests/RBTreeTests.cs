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
				}
				tree.Add(num);
				var treeCount = tree.GetCount(num);
				Assert.IsTrue(treeCount == count, $"[{Tools.Dump(testcase)}] at {i}: {count} != {treeCount}");
				Assert.IsTrue(tree.IsValid(), $"[{Tools.Dump(testcase)}] at {i} validation failed");
				Assert.IsTrue(tree.Min == minValue, $"[{Tools.Dump(testcase)}] at {i}: {minValue} != {tree.Min}");
				Assert.IsTrue(tree.Max == maxValue, $"[{Tools.Dump(testcase)}] at {i}: {maxValue} != {tree.Max}");
				var sortedPart = new int[i + 1];
				Array.Copy(testcase, 0, sortedPart, 0, sortedPart.Length);
				Array.Sort(sortedPart);
				Assert.IsTrue(Enumerable.SequenceEqual(sortedPart, tree), $"[{Tools.Dump(sortedPart)}] != [{Tools.Dump(tree.ToArray())}]");
			}
			var sorted = new int[testcase.Length];
			Array.Copy(testcase, 0, sorted, 0, testcase.Length);
			Array.Sort(sorted);
			Assert.IsTrue(Enumerable.SequenceEqual(sorted, tree), $"[{Tools.Dump(sorted)}] != [{Tools.Dump(tree.ToArray())}]");
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
				Assert.IsTrue(tree.IsValid(), $"[{Tools.Dump(testcase)}] at {i}");
			}
			for (var i = 0; i < testcase.Length; i += 1)
			{
				var num = testcase[i];
				tree.Add(num);
				Assert.IsTrue(tree.IsValid(), $"[{Tools.Dump(testcase)}] at {i}");
			}
			var list = new LinkedList<int>();
			foreach (var num in sorted)
				list.AddLast(num);
			while (list.Count > 0)
			{
				var index = random.Next(0, list.Count);
				var current = list.First;
				for (var i = 0; i < index; i += 1)
					current = current.Next;
				tree.Remove(current.Value);
				Assert.IsTrue(tree.IsValid(), $"[{Tools.Dump(testcase)}] [{Tools.Dump(list.ToArray())}] remove {current.Value}");
				list.Remove(current);
				Assert.IsTrue(Enumerable.SequenceEqual(list, tree), $"[{Tools.Dump(testcase)}] ({testcase.Length - list.Count}/{testcase.Length}) [{Tools.Dump(list.ToArray())}] != [{Tools.Dump(tree.ToArray())}]");
				Assert.IsTrue(tree.Count == list.Count, $"[{Tools.Dump(testcase)}] [{Tools.Dump(list.ToArray())}] {list.Count} != {tree.Count}");
				if (list.Count > 0)
				{
					Assert.IsTrue(tree.Min == list.First.Value, $"[{Tools.Dump(testcase)}] [{Tools.Dump(list.ToArray())}]: {list.First.Value} != {tree.Min}");
					Assert.IsTrue(tree.Max == list.Last.Value, $"[{Tools.Dump(testcase)}] [{Tools.Dump(list.ToArray())}]: {list.Last.Value} != {tree.Max}");
				}
				
			}
		}
	}
}
