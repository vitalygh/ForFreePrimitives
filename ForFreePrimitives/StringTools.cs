using System;
using System.Collections.Generic;

namespace ForFreePrimitives
{
	public static class StringTools
	{
        public static List<int> KMP(string text, string pattern)
		{
            return KMP((i) => text[i], text.Length, (i) => pattern[i], pattern.Length);
		}

        public static List<int> KMP<T>(Func<int, T> text, int textLength, Func<int, T> pattern, int patternLength) where T : IComparable
        {
            var indexes = new List<int>();

            if (textLength <= 0)
                return indexes;

            if (patternLength <= 0)
			{
                for (var idx = 0; idx < textLength; idx += 1)
                    indexes.Add(idx);
                return indexes;
			}

            var lps = new int[patternLength];
            LPS(pattern, patternLength, lps);

            var i = 0;
            var j = 0;
            while (i < textLength)
            {
                if (pattern(j).CompareTo(text(i)) == 0)
                {
                    j += 1;
                    i += 1;
                }
                if (j == patternLength)
                {
                    indexes.Add(i - j);
                    j = lps[j - 1];
                }
                else if ((i < textLength) && (pattern(j).CompareTo(text(i))) != 0)
                {
                    if (j != 0)
                        j = lps[j - 1];
                    else
                        i += 1;
                }
            }
            return indexes;
        }

        public static void LPS<T>(Func<int, T> pattern, int patternLength, int[] lps) where T : IComparable
        {
            var length = 0;
            var i = 1;
            lps[0] = 0;

            while (i < patternLength)
            {
                if (pattern(i).CompareTo(pattern(length)) == 0)
                {
                    length += 1;
                    lps[i] = length;
                    i += 1;
                }
                else
                {
                    if (length != 0)
                        length = lps[length - 1];
                    else
                    {
                        lps[i] = length;
                        i += 1;
                    }
                }
            }
        }

        public static void ZFunction<T>(Func<int, T> s, int n, int[] zf) where T : IComparable
        {
            var l = 0;
            var r = 0;
            for (var i = 1; i < n; i += 1)
            {
                if (i <= r)
                    zf[i] = Math.Min(r - i + 1, zf[i - l]);
                while ((i + zf[i] < n) && (s(zf[i]).CompareTo(s(i + zf[i])) == 0))
                    zf[i] += 1;
                if ((i + zf[i] - 1) > r)
                {
                    l = i;
                    r = i + zf[i] - 1;
                }
            }
        }

        public static void BuildSuffixArray(Func<int, int> data, int[] arr)
        {
            var n = arr.Length;
            var labels = new int[n];
            for (var i = 0; i < n; i += 1)
                labels[i] = data(i) + 1;
            var hl = 1;
            var grade = new (int a, int b, int i)[n];
            var rounds = (int)Math.Ceiling(Math.Log(n, 2));
            for (var r = 0; r < rounds; r += 1)
            {
                for (var i = 0; i < n; i += 1)
                {
                    var a = labels[i] + 1;
                    var b = (i + hl) < n ? labels[i + hl] + 1 : 0;
                    grade[i] = (a, b, i);
                }
                Array.Sort(grade, (x, y) =>
                {
                    if (x.a == y.a)
                        return x.b.CompareTo(y.b);
                    return x.a.CompareTo(y.a);
                });
                var label = 0;
                labels[grade[0].i] = label;
                for (var i = 1; i < n; i += 1)
                {
                    if ((grade[i - 1].a != grade[i].a) || (grade[i - 1].b != grade[i].b))
                        label += 1;
                    labels[grade[i].i] = label;
                }
                hl *= 2;
            }
            for (var i = 0; i < n; i += 1)
                arr[labels[i]] = i;
        }

        public static void BuildLCP<T>(Func<int, T> data, int[] suf, int[] lcp) where T : IComparable
        {
            var n = suf.Length;
            var pos = new int[n];
            for (var i = 0; i < n; i += 1)
                pos[suf[i]] = i;
            var k = 0;
            for (var i = 0; i < n; i += 1)
            {
                if (k > 0)
                    k -= 1;
                if (pos[i] == (n - 1))
                {
                    lcp[n - 1] = -1;
                    k = 0;
                    continue;
                }
                var j = suf[pos[i] + 1];
                while ((Math.Max(i + k, j + k) < n) && (data(i + k).CompareTo(data(j + k)) == 0))
                    k += 1;
                lcp[pos[i]] = k;
            }
        }

        public static void ManacherOdd<T>(Func<int, T> s, int n, int[] d) where T : IComparable
        {
            var l = 0;
            var r = 0;
            for (var i = 0; i < n; i += 1)
            {
                if (i < r)
                    d[i] = Math.Min(r - i + 1, d[l + r - i]);
                while ((i - d[i] >= 0) && (i + d[i] < n) && (s(i - d[i]).CompareTo(s(i + d[i])) == 0))
                    d[i] += 1;
                if (i + d[i] - 1 > r)
                {
                    l = i - d[i] + 1;
                    r = i + d[i] - 1;
                }
            }
        }

        public static void ManacherEven<T>(Func<int, T> s, int n, int[] d) where T : IComparable
        {
            var l = 0;
            var r = 0;
            for (var i = 0; i < n - 1; i += 1)
            {
                if (i < r)
                    d[i] = Math.Min(r - i, d[l + r - i - 1]);
                while ((i - d[i] >= 0) && (i + d[i] + 1 < n) && (s(i - d[i]).CompareTo(s(i + d[i] + 1)) == 0))
                    d[i] += 1;
                if (i + d[i] > r)
                {
                    l = i - d[i] + 1;
                    r = i + d[i];
                }
            }
        }
    }
}
