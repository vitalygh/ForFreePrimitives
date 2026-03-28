using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using ForFreePrimitives;

namespace ForFreePrimitivesTests
{
	/// <summary>
	/// Summary description for UnitTest1
	/// </summary>
	[TestClass]
	public class SuffixTreeTests
	{
		[TestMethod]
		public void Tests()
		{
			ProcessDefinedTestCases();
			ProcessRandomTestCases();
		}

		private static Random random = new Random();

		private static readonly (string data, int[] result)[] suffixArrayTestcases = new (string data, int[] result)[]
		{
			("ABAACBAB", new int[] { 2,6,0,3,7,1,5,4 }),
			("abcabxabcd", new int[] { 0,6,3,1,7,4,2,8,9,5 }),
		};

		private void ProcessDefinedTestCases()
		{
			foreach (var (data, result) in suffixArrayTestcases)
				ValidateSuffixArray(data, result);
		}

		private string GenerateLowercaseLettersString(int length, int dictionarySize, Random random)
		{
			var arr = new char[length];
			dictionarySize = Math.Max(1, dictionarySize);
			dictionarySize = Math.Min('z' - 'a' + 1, dictionarySize);
			for (var i = 0; i < arr.Length; i += 1)
				arr[i] = (char)random.Next('a', 'a' + dictionarySize - 1);
			return new string(arr);
		}

		private int[] BuildSuffixArrayNaive(string text)
		{
			var suf = new (string text, int i)[text.Length];
			for (var j = 0; j < text.Length; j += 1)
				suf[j] = (text.Substring(j), j);
			Array.Sort(suf);
			return suf.Select(x => x.i).ToArray();
		}

		private void ProcessRandomTestCases()
		{
			var minLength = 1;
			var maxLength = 20;
			var count = 100;
			var lettersCount = 'z' - 'a' + 1;
			for (var i = 0; i < count; i += 1)
			{
				var length = random.Next(minLength, maxLength + 1);
				var dictionarySize = random.Next(1, lettersCount + 1);
				var text = GenerateLowercaseLettersString(length, dictionarySize, random);
				var suf = BuildSuffixArrayNaive(text);
				ValidateSuffixArray(text, suf);
			}
		}

		private void ValidateSuffixArray(string text, int[] result)
		{
			var tree = new UkkSuffixTree(i => text[i], text.Length);
			var arr = new int[text.Length];
			tree.BuildSuffixArray(arr);
			if (!Enumerable.SequenceEqual(result, arr))
				Assert.Fail($"Testcase failed: \"{text}\", new int[] {{ {Tools.Dump(arr)} }}");
		}
	}
}
