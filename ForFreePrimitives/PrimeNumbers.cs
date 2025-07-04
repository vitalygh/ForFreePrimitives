using System;
using System.Collections;
using System.Collections.Generic;


namespace ForFreePrimitives
{
	public static class PrimeNumbers
	{
        private static BitArray composites = null;
        public static bool IsInitialized => composites != null;
        public static void Init(int max)
		{
            composites = new BitArray(max + 1);
            for (var i = 3L; i * i < composites.Count; i += 2)
            {
                if (composites[(int)i])
                    continue;
                for (var j = i * i; j < composites.Count; j += 2 * i)
                    composites[(int)j] = true;
            }
        }
        public static bool IsPrime(int n)
        {
            if (n < 2)
                return false;
            if (n == 2)
                return true;
            if (n % 2 == 0)
                return false;
            if (n == 3)
                return true;
            if (n % 3 == 0)
                return false;
            if (composites != null)
                return !composites[n];
            for (var i = 5L; i * i <= n; i += 6)
                if (n % i == 0 || n % (i + 2) == 0)
                    return false;
            return true;
        }
    }
}
