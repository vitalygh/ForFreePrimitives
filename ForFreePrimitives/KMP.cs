using System;
using System.Collections.Generic;

namespace ForFreePrimitives
{
	public static class KMP
	{
        public static List<int> Find(string text, string pattern)
		{
            return Find((i) => text[i], text.Length, (i) => pattern[i], pattern.Length);
		}

        public static List<int> Find<T>(Func<int, T> text, int textLength, Func<int, T> pattern, int patternLength) where T : IComparable
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
            ComputeLPS(pattern, patternLength, lps);

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

        private static void ComputeLPS<T>(Func<int, T> pattern, int patternLength, int[] lps) where T : IComparable
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
    }
}
