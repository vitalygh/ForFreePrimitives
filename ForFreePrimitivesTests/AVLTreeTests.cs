using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ForFreePrimitives;

namespace ForFreePrimitivesTests
{
	[TestClass]
	public class AVLTreeTests
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
			new int[] {	9,	8,	7 },
			new int[] {	5,	5,	5 },
			new int[] {	-1,	0,	1 },
			new int[] { 0,  0,  1 },
			new int[] { 0,  1,  0 },
			new int[] { 1,  0,  0 },
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
			var tree = new AVLTree<int>();
			var min = testcase.FirstOrDefault();
			var max = testcase.FirstOrDefault();
			foreach (var num in testcase)
			{
				tree.Add(num);
				min = Math.Min(min, num);
				max = Math.Max(max, num);
				Assert.IsTrue(tree.Min == min);
				Assert.IsTrue(tree.Max == max);
				Assert.IsTrue(tree.IsValid());
			}
			var sorted = new int[testcase.Length];
			Array.Copy(testcase, 0, sorted, 0, testcase.Length);
			Array.Sort(sorted);
			var treeAsArray = tree.ToArray();
			Assert.IsTrue(Enumerable.SequenceEqual(treeAsArray, sorted));
			var count = 0;
			for (var i = 0; i < sorted.Length; i += 1)
			{
				if ((i > 0) && (sorted[i - 1] != sorted[i]))
					count = i;
				Assert.IsTrue(tree.GetLesserCount(sorted[i]) == count);
			}
			count = 0;
			for (var i = sorted.Length - 1; i >= 0; i -= 1)
			{
				if ((i < sorted.Length - 1) && (sorted[i + 1] != sorted[i]))
					count = sorted.Length - i - 1;
				Assert.IsTrue(tree.GetGreaterCount(sorted[i]) == count);
			}
			var distinct = new List<int>();
			var lesser = 0;
			for (var i = 0; i < sorted.Length; i += 1)
			{
				if ((distinct.Count < 1) || (distinct[distinct.Count - 1] != sorted[i]))
					distinct.Add(sorted[i]);
				var exist = distinct.Count > 1;
				if (exist)
					lesser = distinct[distinct.Count - 2];
				Assert.IsTrue(tree.TryGetLesser(sorted[i], out var treeVal) == exist);
				if (exist)
					Assert.IsTrue(treeVal == lesser);
			}
			for (var i = 0; i < sorted.Length; i += 1)
			{
				var target = random.Next(int.MinValue, int.MaxValue);
				var start = 0;
				var end = sorted.Length - 1;
				while (start <= end)
				{
					var mid = start + (end - start) / 2;
					if (sorted[mid] < target)
						start = mid + 1;
					else
						end = mid - 1;
				}
				var lesserIndex = start - 1;
				var exist = lesserIndex >= 0;
				Assert.IsTrue(tree.TryGetLesser(target, out var treeVal) == exist);
				if (exist)
					Assert.IsTrue(treeVal == sorted[lesserIndex]);
			}
			distinct.Clear();
			var greater = 0;
			for (var i = sorted.Length - 1; i >= 0; i -= 1)
			{
				if ((distinct.Count < 1) || (distinct[distinct.Count - 1] != sorted[i]))
					distinct.Add(sorted[i]);
				var exist = distinct.Count > 1;
				if (exist)
					greater = distinct[distinct.Count - 2];
				Assert.IsTrue(tree.TryGetGreater(sorted[i], out var treeVal) == exist);
				if (exist)
					Assert.IsTrue(treeVal == greater);
			}
			for (var i = 0; i < sorted.Length; i += 1)
			{
				var target = random.Next(int.MinValue, int.MaxValue);
				var start = 0;
				var end = sorted.Length - 1;
				while (start <= end)
				{
					var mid = start + (end - start) / 2;
					if (sorted[mid] > target)
						end = mid - 1;
					else
						start = mid + 1;
				}
				var lesserIndex = end + 1;
				var exist = lesserIndex < sorted.Length;
				Assert.IsTrue(tree.TryGetGreater(target, out var treeVal) == exist);
				if (exist)
					Assert.IsTrue(treeVal == sorted[lesserIndex]);
			}
			var set = new SortedSet<int>();
			var counter = new Dictionary<int, int>();
			foreach (var num in sorted)
			{
				if (!counter.ContainsKey(num))
				{
					counter.Add(num, 0);
					set.Add(num);
				}
				counter[num] += 1;
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
				Assert.IsTrue(tree.IsValid());
				Assert.IsTrue(tree.Count == shuffle.Length - i - 1);
				if (tree.Count > 0)
				{
					Assert.IsTrue(tree.Min == set.Min);
					Assert.IsTrue(tree.Max == set.Max);
				}
			}
		}
	}
}

