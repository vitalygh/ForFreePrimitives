using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ForFreePrimitives;

namespace ForFreePrimitivesTests
{
	/// <summary>
	/// Summary description for CHTTests
	/// </summary>
	[TestClass]
	public class CHTTests
	{
		// Leetcode 3826
		private static readonly (int[] nums, int k, long answer)[] minimalPartitionScoreTestcases = new[]
		{
			(new int[] { 45,71,34,89,63,83,91,91,73,95,89,96,66,28,90,79,1,77,75,61 }, 16, 63750L),
            (new int[] { 35,73,100,16,24,39,70,46,62,54,77,71,83,78,91,99,89,54,60,55 }, 7, 117998L),
            (new int[] { 13,30,25,65,74,48,90,34,13,80,7,62,28,89,50,81,31,59,52,40 }, 5, 95449L),
            (new int[] { 24,52,39,54,46,51,58,54,39,98,74,48,23,78,98,73,87,55,83,30 }, 20, 39356L),
        };


		[TestMethod]
		public void Tests()
		{
			ProcessDefinedTestCases();
		}

		private void ProcessDefinedTestCases()
		{
			foreach (var (nums, k, answer) in minimalPartitionScoreTestcases)
				ValidateMPS(nums, k, answer);
		}

        public long MinPartitionScore(int[] nums, int k, Action<long, long> init, Action<long, long> update, Func<long, long> query)
        {
            var p = new long[nums.Length + 1];
            for (var i = 0; i < nums.Length; i += 1)
                p[i + 1] = p[i] + nums[i];
            var dp = new long[2][];
            for (var i = 0; i < dp.Length; i += 1)
                dp[i] = new long[nums.Length + 1];
            for (var i = 0; i < nums.Length; i += 1)
                dp[0][i + 1] = p[i + 1] * (p[i + 1] + 1) / 2;
            for (var d = 1; d < k; d += 1)
            {
                for (var i = 0; i < dp[1].Length; i += 1)
                    dp[1][i] = long.MaxValue;
                var minx = p[Math.Min(p.Length - 1, d + 1)];
                var maxx = p[p.Length - 1];
                init(minx, maxx);
                for (var i = d; i < nums.Length; i += 1)
                {
                    var z = (p[i + 1] * p[i + 1] + p[i + 1]) / 2;
                    var a = -p[i];
                    var b = (p[i] * p[i] - p[i]) / 2 + dp[0][i];
                    update(a, b);
                    var min = query(p[i + 1]);
                    dp[1][i + 1] = Math.Min(dp[1][i + 1], min + z);
                }
                (dp[0], dp[1]) = (dp[1], dp[0]);
            }
            return dp[0][nums.Length];
        }

        private void ValidateMPS(int[] nums, int k, long answer)
		{
            ConvexHullTrick cht = null;
            var chtResult = MinPartitionScore(nums, k, (minx, maxx) =>
            {
                cht = new ConvexHullTrick();
            }, 
            (a, b) => cht.Update(a, b),
            (x) => cht.Query(x));
            LiChaoTree lct = null;
            var lctResult = MinPartitionScore(nums, k, (minx, maxx) =>
            {
                lct = new LiChaoTree((int)minx, (int)maxx);
            },
            (a, b) => lct.Update(a, b),
            (x) => lct.Query(x));
            LiChaoDynamicTree lcdt = null;
            var lcdtResult = MinPartitionScore(nums, k, (minx, maxx) =>
            {
                lcdt = new LiChaoDynamicTree(minx, maxx);
            },
            (a, b) => lcdt.Update(a, b),
            (x) => lcdt.Query(x));
            if ((chtResult != answer) || (lctResult != answer) || (lcdtResult != answer))
                Assert.Fail($"new int[] {{{Tools.Dump(nums)}}}, {k}, {answer}L");
        }
	}
}
