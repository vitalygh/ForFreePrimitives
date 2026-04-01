using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ForFreePrimitives;

namespace ForFreePrimitivesTests
{
	[TestClass]
	public class UnionFindTests
	{
		[TestMethod]
		public void Tests()
		{
			ProcessDefinedTestCases();
		}

		private readonly (int[][] connected, int count)[] unionFindTestcases = new[]
		{
			(new[] 
			{ 
				new int[] { 1, 1, 0 },
				new int[] { 1, 1, 0 },
				new int[] { 0, 0, 1 },
			}, 2),
			(new[]
			{
				new int[] { 1, 0, 0 },
				new int[] { 0, 1, 0 },
				new int[] { 0, 0, 1 },
			}, 3),
			(new[]
			{
				new int[] { 1, 0, 0, 1 },
				new int[] { 0, 1, 1, 0 },
				new int[] { 0, 1, 1, 1 },
				new int[] { 1, 0, 1, 1 },
			}, 1),
			(new[]
			{
				new int[] { 1, 1, 1 },
				new int[] { 1, 1, 1 },
				new int[] { 1, 1, 1 },
			}, 1),
			(new[]
			{
				new int[] { 1,0,0,0,0,0,0,0,0,1,0,0,0,0,0 },
				new int[] { 0,1,0,1,0,0,0,0,0,0,0,0,0,1,0 },
				new int[] { 0,0,1,0,0,0,0,0,0,0,0,0,0,0,0 },
				new int[] { 0,1,0,1,0,0,0,1,0,0,0,1,0,0,0 },
				new int[] { 0,0,0,0,1,0,0,0,0,0,0,0,1,0,0 },
				new int[] { 0,0,0,0,0,1,0,0,0,0,0,0,0,0,0 },
				new int[] { 0,0,0,0,0,0,1,0,0,0,0,0,0,0,0 },
				new int[] { 0,0,0,1,0,0,0,1,1,0,0,0,0,0,0 },
				new int[] { 0,0,0,0,0,0,0,1,1,0,0,0,0,0,0 },
				new int[] { 1,0,0,0,0,0,0,0,0,1,0,0,0,0,0 },
				new int[] { 0,0,0,0,0,0,0,0,0,0,1,0,0,0,0 },
				new int[] { 0,0,0,1,0,0,0,0,0,0,0,1,0,0,0 },
				new int[] { 0,0,0,0,1,0,0,0,0,0,0,0,1,0,0 },
				new int[] { 0,1,0,0,0,0,0,0,0,0,0,0,0,1,0 },
				new int[] { 0,0,0,0,0,0,0,0,0,0,0,0,0,0,1 },
			}, 8),
			(new[]
			{
				new int[] { 1,0,0,0,0,0,0,0,1,0,0,0,0,0,0 },
				new int[] { 0,1,1,0,0,0,0,0,0,0,0,0,0,1,0 },
				new int[] { 0,1,1,0,0,0,0,0,0,0,0,1,0,0,1 },
				new int[] { 0,0,0,1,0,1,0,0,1,0,0,0,0,1,0 },
				new int[] { 0,0,0,0,1,0,0,0,0,0,0,1,0,0,0 },
				new int[] { 0,0,0,1,0,1,0,0,0,0,0,1,0,0,0 },
				new int[] { 0,0,0,0,0,0,1,0,0,0,0,0,0,0,0 },
				new int[] { 0,0,0,0,0,0,0,1,0,0,0,0,0,0,0 },
				new int[] { 1,0,0,1,0,0,0,0,1,1,1,0,0,1,0 },
				new int[] { 0,0,0,0,0,0,0,0,1,1,0,1,1,0,0 },
				new int[] { 0,0,0,0,0,0,0,0,1,0,1,1,0,0,0 },
				new int[] { 0,0,1,0,1,1,0,0,0,1,1,1,0,0,0 },
				new int[] { 0,0,0,0,0,0,0,0,0,1,0,0,1,0,1 },
				new int[] { 0,1,0,1,0,0,0,0,1,0,0,0,0,1,0 },
				new int[] { 0,0,1,0,0,0,0,0,0,0,0,0,1,0,1 },
			}, 3),
		};

		private void ProcessDefinedTestCases()
		{
			foreach ((var map, var result) in unionFindTestcases)
				Validate(map, result);
		}

		private void Validate(int[][] map, int result)
		{
			var uf = new UnionFind(map.Length);
			for (var i = 0; i < map.Length; i += 1)
				for (var j = 0; j < map[i].Length; j += 1)
					if (map[i][j] > 0)
						uf.Union(i, j);
			Assert.IsTrue(uf.Count == result);
			var ufu = new UnionFindUndo(map.Length);
			for (var i = 0; i < map.Length; i += 1)
				for (var j = 0; j < map[i].Length; j += 1)
					if (map[i][j] > 0)
						ufu.Union(i, j);
			Assert.IsTrue(ufu.Count == result);
		}
	}
}
