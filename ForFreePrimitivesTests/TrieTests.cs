using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ForFreePrimitives;

namespace ForFreePrimitivesTests
{
	[TestClass]
	public class TrieTests
	{

		[TestMethod]
		public void Tests()
		{
			ProcessDefinedTestCases();
		}

		private readonly int[][] trieTestCases = new int[][]
		{
			new int[] {1,2,3,4,5,1,2,3,4,5},
		};

		private readonly (int[] nums, int maxXor)[] maxXorTestCases = new (int[], int)[]
		{
			(new int[] {3,10,5,25,2,8}, 28),
			(new int[] {14,70,53,83,49,91,36,80,92,51,66,70}, 127),
		};

		private readonly (int[] nums, int low, int high, int xorPairsInRangeCount)[] xorInRangeCountTestCases = new (int[], int, int, int)[]
		{
			(new int[] {1,4,2,7}, 2, 6, 6),
			(new int[] {9,8,4,2,1}, 5, 14, 8),
		};

		private void ProcessDefinedTestCases()
		{
			foreach (var nums in trieTestCases)
				ValidateTrie(nums);
			foreach (var (nums, maxXor) in maxXorTestCases)
				ValidateMaxXor(nums, maxXor);
			foreach (var (nums, low, high, xorPairsInRangeCount) in xorInRangeCountTestCases)
				ValidateXorInRangeCount(nums, low, high, xorPairsInRangeCount);
		}

		private void ValidateTrie(int[] nums)
		{
			var trie = new BitTrie(8 * sizeof(int));
			var counter = new Dictionary<int, int>();
			for (var i = 0; i < nums.Length; i += 1)
			{
				var num = nums[i];
				trie.Add(num);
				if (counter.TryGetValue(num, out var count))
					counter[num] = count + 1;
				else
					counter.Add(num, 1);
				Assert.IsTrue(trie.Count == (i + 1));
				for (var j = 0; j < nums.Length; j += 1)
					Assert.IsTrue(trie.Contains(nums[j]) == counter.ContainsKey(nums[j]));
			}
			for (var i = nums.Length - 1; i >= 0; i -= 1)
			{
				var num = nums[i];
				Assert.IsTrue(trie.Remove(num));
				Assert.IsTrue(trie.Count == i);
				if (counter.TryGetValue(num, out var count))
				{
					if (count > 1)
						counter[num] = count - 1;
					else
						counter.Remove(num);
				}
				var stillContains = counter.ContainsKey(num);
				Assert.IsTrue(trie.Contains(num) == stillContains);
				if (!stillContains)
					Assert.IsFalse(trie.Remove(num));
				for (var j = 0; j < nums.Length; j += 1)
					Assert.IsTrue(trie.Contains(nums[j]) == counter.ContainsKey(nums[j]));
			}				
		}

		private void ValidateMaxXor(int[] nums, int maxXor)
		{
			var trie = new BitTrie(8 * sizeof(int));
			var max = 0;
			foreach (var num in nums)
			{
				trie.Add(num);
				max = Math.Max(max, (int)trie.GetMaxXor(num));
			}
			Assert.IsTrue(max == maxXor);
		}

		private void ValidateXorInRangeCount(int[] nums, int low, int high, int xorPairsInRangeCount)
		{
			var trie = new BitTrie(8 * sizeof(int));
			var count = 0;
			foreach (var num in nums)
			{
				count += trie.GetGreaterXorCount(low - 1, num);
				count -= trie.GetGreaterXorCount(high, num);
				trie.Add(num);
			}
			Assert.IsTrue(count == xorPairsInRangeCount);
		}
	}
}
