using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
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

		private readonly (string[] words, string text, (int pos, int word)[] matches)[] ahoCorasickTestcases = new (string[], string, (int, int)[])[]
		{
			(new string[] {"he", "she", "his", "hers"}, "a hishers", new (int, int)[]{ (4, 2), (6, 0), (6, 1), (8, 3) }),
			(new string[] {"aba", "ba" }, "ababa", new (int, int)[]{ (2, 0), (2, 1), (4, 0), (4, 1) }),
			(new string[] {"sh", "she" }, "she", new (int, int)[]{ (1, 0), (2, 1) }),
			(new string[] { "cat", "cat", "dog" }, "catdog", new (int, int)[]{ (2, 0), (2, 1), (5, 2) }),
			(new string[] { "a", "aa", "aaa", "aaaa" }, "aaaa", new (int, int)[]{ (0, 0), (1, 0), (2, 0), (3, 0), (1, 1), (2, 1), (3, 1), (2, 2), (3, 2), (3, 3) }),
			(new string[] { "xyz", "abc" }, "defghijkl", new (int, int)[]{ }),
			(new string[] { "xyz", "abc" }, "", new (int, int)[]{ }),
			(new string[] { }, "defghijkl", new (int, int)[]{ }),
			(new string[] { "", "abc" }, "abc", new (int, int)[]{ (2, 1) }),
		};

		private void ProcessDefinedTestCases()
		{
			foreach (var nums in trieTestCases)
				ValidateTrie(nums);
			foreach (var (nums, maxXor) in maxXorTestCases)
				ValidateMaxXor(nums, maxXor);
			foreach (var (nums, low, high, xorPairsInRangeCount) in xorInRangeCountTestCases)
				ValidateXorInRangeCount(nums, low, high, xorPairsInRangeCount);
			foreach (var (words, text, matches) in ahoCorasickTestcases)
				ValidateACTrie(words, text, matches);
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

		private void ValidateACTrie(string[] words, string text, (int pos, int word)[] matches)
		{
			var sortedMatches = new (int, int)[matches.Length];
			Array.Copy(matches, sortedMatches, matches.Length);
			Array.Sort(sortedMatches);
			var result = new List<(int, int)>();
			var min = int.MaxValue;
			var max = int.MinValue;
			foreach (var word in words)
				foreach (var c in word)
				{
					min = Math.Min(min, c);
					max = Math.Max(max, c);
				}
			var dictionarySize = min <= max ? max - min + 1 : 0;
			var trie = new ACTrie<int>(dictionarySize);
			for (var i = 0; i < words.Length; i += 1)
			{
				var word = words[i];
				trie.Add(x => word[x] - min, word.Length, i);
			}
			trie.Find(x => text[x] - min, text.Length, (pos, data) => result.Add((pos, data)));
			var sortedResult = result.ToArray();
			Array.Sort(sortedResult);
			if (!Enumerable.SequenceEqual(sortedMatches, sortedResult))
				Assert.Fail($"[{Tools.Dump(words)}] {text}");
		}
	}
}
