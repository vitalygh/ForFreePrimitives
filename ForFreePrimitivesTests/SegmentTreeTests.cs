using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
		}

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

