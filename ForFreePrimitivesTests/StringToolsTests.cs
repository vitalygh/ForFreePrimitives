using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ForFreePrimitives;
using System.Linq;

namespace ForFreePrimitivesTests
{
	[TestClass]
	public class StringToolsTests
	{
		[TestMethod]
		public void Tests()
		{
			ProcessDefinedTestCases();
		}

		private readonly (string text, string pattern, int firstIndex)[] kmpTestcases = new []
		{
			("", "", -1),
			("", "", 0),
			("", "a", -1),
			("", "foo", -1),
			("fo", "foo", -1),
			("foo", "foo", 0),
			("oofofoofooo", "f", 2),
			("oofofoofooo", "foo", 4),
			("barfoobarfoo", "foo", 3),
			("foo", "", 0),
			("foo", "o", 1),
			("abcABCabc", "A", 3),
			("jrzm6jjhorimglljrea4w3rlgosts0w2gia17hno2td4qd1jz", "jz", 47),
			("ekkuk5oft4eq0ocpacknhwouic1uua46unx12l37nioq9wbpnocqks6", "ks6", 52),
			("999f2xmimunbuyew5vrkla9cpwhmxan8o98ec", "98ec", 33),
			("9lpt9r98i04k8bz6c6dsrthb96bhi", "96bhi", 24),
			("55u558eqfaod2r2gu42xxsu631xf0zobs5840vl", "5840vl", 33),
			("", "a", -1),
			("x", "a", -1),
			("x", "x", 0),
			("abc", "a", 0),
			("abc", "b", 1),
			("abc", "c", 2),
			("abc", "x", -1),
			("", "ab", -1),
			("bc", "ab", -1),
			("ab", "ab", 0),
			("xab", "ab", 1),
			("", "abc", -1),
			("xbc", "abc", -1),
			("abc", "abc", 0),
			("xabc", "abc", 1),
			("xabxc", "abc", -1),
			("", "abcd", -1),
			("xbcd", "abcd", -1),
			("abcd", "abcd", 0),
			("xabcd", "abcd", 1),
			("xyabcd", "abcd", -1),
			("xbcqq", "abcqq", -1),
			("abcqq", "abcqq", 0),
			("xabcqq", "abcqq", 1),
			("xyabcqq", "abcqq", -1),
			("xabxcqq", "abcqq", -1),
			("xabcqxq", "abcqq", -1),
			("", "01234567", -1),
			("32145678", "01234567", -1),
			("01234567", "01234567", 0),
			("x01234567", "01234567", 1),
			("x0123456x01234567", "01234567", 9),
			("", "0123456789", -1),
			("3214567844", "0123456789", -1),
			("0123456789", "0123456789", 0),
			("x0123456789", "0123456789", 1),
			("x012345678x0123456789", "0123456789", 11),
			("x01234567x89", "0123456789", -1),
			("", "0123456789012345", -1),
			("3214567889012345", "0123456789012345", -1),
			("0123456789012345", "0123456789012345", 0),
			("x0123456789012345", "0123456789012345", 1),
			("x012345678901234x0123456789012345", "0123456789012345", 17),
			("", "01234567890123456789", -1),
			("32145678890123456789", "01234567890123456789", -1),
			("01234567890123456789", "01234567890123456789", 0),
			("x01234567890123456789", "01234567890123456789", 1),
			("x0123456789012345678x01234567890123456789", "01234567890123456789", 21),
			("", "0123456789012345678901234567890", -1),
			("321456788901234567890123456789012345678911", "0123456789012345678901234567890", -1),
			("0123456789012345678901234567890", "0123456789012345678901234567890", 0),
			("x0123456789012345678901234567890", "0123456789012345678901234567890", 1),
			("x012345678901234567890123456789x0123456789012345678901234567890", "0123456789012345678901234567890", 32),
			("", "01234567890123456789012345678901", -1),
			("32145678890123456789012345678901234567890211", "01234567890123456789012345678901", -1),
			("01234567890123456789012345678901", "01234567890123456789012345678901", 0),
			("x01234567890123456789012345678901", "01234567890123456789012345678901", 1),
			("x0123456789012345678901234567890x01234567890123456789012345678901", "01234567890123456789012345678901", 33),
			("xxxxxx012345678901234567890123456789012345678901234567890123456789012", "012345678901234567890123456789012345678901234567890123456789012", 6),
			("", "0123456789012345678901234567890123456789", -1),
			("xx012345678901234567890123456789012345678901234567890123456789012", "0123456789012345678901234567890123456789", 2),
			("xx012345678901234567890123456789012345678901234567890123456789012", "0123456789012345678901234567890123456xxx", -1),
			("xx0123456789012345678901234567890123456789012345678901234567890120123456789012345678901234567890123456xxx", "0123456789012345678901234567890123456xxx", 65),
			("oxoxoxoxoxoxoxoxoxoxoxoy", "oy", 22),
			("oxoxoxoxoxoxoxoxoxoxoxox", "oy", -1),
			("oxoxoxoxoxoxoxoxoxoxox☺", "☺", 22),
			("xx0123456789012345678901234567890123456789012345678901234567890120123456789012345678901234567890123456xxx\xed\x9f\xc0", "\xed\x9f\xc0", 105),
			("ababababbabababbababababbababababbabababa", "abab", 0),
		};

		private static readonly string [] zTestcases = new string[]
		{
			"aaabaab",
			"abcdefgh",
			"aabbaabbcd",
			"ititititvt",
			"abcababacdabcababad",
		};

		private static readonly string[] manacherTestcases = new string[]
		{
			"abcdefgh",
			"abababaa",
			"ababbabbaba",
			"aaaaaaaaaaa",
		};

		private static readonly (string data, int[] result)[] suffixArrayTestcases = new (string data, int[] result)[]
		{
			("ABAACBAB", new int[] { 2,6,0,3,7,1,5,4 }),
			("abcabxabcd", new int[] { 0,6,3,1,7,4,2,8,9,5 }),
		};

		private void ProcessDefinedTestCases()
		{
			foreach ((var text, var pattern, _) in kmpTestcases)
				ValidateKMP(text, pattern);
			foreach (var text in zTestcases)
				ValidateZ(text);
			foreach (var text in manacherTestcases)
				ValidateManacher(text);
			foreach (var (text, arr) in suffixArrayTestcases)
				ValidateSuffixArray(text, arr);
		}

		private List<int> GetValidResult(string text, string pattern)
		{
			var result = new List<int>();
			var start = 0;
			while (start < text.Length)
			{
				var index = text.IndexOf(pattern, start);
				if (index < 0)
					break;
				result.Add(index);
				start = index + 1;
			}
			return result;
		}

		private void ValidateKMP(string text, string pattern)
		{
			var result = StringTools.KMP(text, pattern);
			var validResult = GetValidResult(text, pattern);
			Assert.IsTrue((result != null) && (result.Count == validResult.Count) && Enumerable.SequenceEqual(result, validResult), $"text: {text}, pattern: {pattern}");
		}

		private int CalcZ(string text, int index)
		{
			for (var i = index; i < text.Length; i += 1)
				if (text[i] != text[i - index])
					return i - index;
			return text.Length - index;
		}

		private void ValidateZ(string text)
		{
			var zf = new int[text.Length];
			StringTools.ZFunction(i => text[i], text.Length, zf);
			for (var i = 1; i < text.Length; i += 1)
			{
				var val = CalcZ(text, i);
				if (zf[i] != val)
					Assert.Fail($"\"{text}\" at {i}: {zf[i]} != {val}");
			}
		}

		private int PalindromeOddSize(string text, int index)
		{
			var size = 1;
			for (var i = 1; i < text.Length; i += 1)
			{
				var l = index - i;
				if (l < 0)
					break;
				var r = index + i;
				if (r >= text.Length)
					break;
				if (text[l] != text[r])
					break;
				size += 1;
			}
			return size;
		}

		private int PalindromeEvenSize(string text, int index)
		{
			var size = 0;
			for (var i = 0; i < text.Length; i += 1)
			{
				var l = index - i;
				if (l < 0)
					break;
				var r = index + i + 1;
				if (r >= text.Length)
					break;
				if (text[l] != text[r])
					break;
				size += 1;
			}
			return size;
		}

		private void ValidateManacher(string text)
		{
			var odd = new int[text.Length];
			var even = new int[text.Length];
			StringTools.ManacherOdd(i => text[i], text.Length, odd);
			StringTools.ManacherEven(i => text[i], text.Length, even);
			for (var i = 0; i < text.Length; i += 1)
			{
				var rOdd = PalindromeOddSize(text, i);
				var rEven = PalindromeEvenSize(text, i);
				if (odd[i] != rOdd)
					Assert.Fail($"\"{text}\" odd at {i}: {odd[i]} != {rOdd}");
				if (even[i] != rEven)
					Assert.Fail($"\"{text}\" odd at {i}: {odd[i]} != {rOdd}");
			}
		}

		private void ValidateSuffixArray(string text, int[] arr)
		{
			var suf = new int[text.Length];
			StringTools.BuildSuffixArray(i => text[i], suf);
			if (!Enumerable.SequenceEqual(suf, arr))
				Assert.Fail($"Testcase failed: \"{text}\", new int[] {{ {Tools.Dump(arr)} }}");
		}
	}
}
