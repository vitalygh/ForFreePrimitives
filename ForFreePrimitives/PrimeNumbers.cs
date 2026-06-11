using System;
using System.Collections;
using System.Collections.Generic;


namespace ForFreePrimitives
{
	public static class PrimeNumbers
	{
        private static BitArray composites = null;
        public static bool IsPrimesInitialized => composites != null;
        public static void InitPrimes(int max)
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

        private static int[] mobiusFunctionValues = null;

        public static bool IsMobiusInitialized => mobiusFunctionValues != null;

        public static void InitMobius(int max)
		{
            mobiusFunctionValues = CalcMobius(max);
        }

        public static int[] CalcMobius(int max)
		{
            var mu = new int[max + 1];
            var primes = new int[max + 1];
            var composite = new BitArray(max + 1);
            var count = 0;
            mu[1] = 1;
            for (var i = 2; i <= max; i += 1)
            {
                if (!composite[i])
                {
                    primes[count] = i;
                    count += 1;
                    mu[i] = -1;
                }
                for (var j = 0; j < count; j += 1)
                {
                    var p = primes[j];
                    var val = i * p;
                    if (val > max)
                        break;
                    composite[val] = true;
                    if ((i % p) == 0)
                    {
                        mu[val] = 0;
                        break;
                    }
                    mu[val] = -mu[i];
                }
            }
            return mu;
        }

        public static int GetMobius(long num)
        {
            if (mobiusFunctionValues != null)
                return mobiusFunctionValues[num];
            var count = 0;
            if ((num % 2) == 0)
            {
                num /= 2;
                count += 1;
                if ((num % 2) == 0)
                    return 0;
            }
            for (var i = 3L; i * i <= num; i += 2)
            {
                if ((num % i) == 0)
                {
                    num /= i;
                    count += 1;
                    if ((num % i) == 0)
                        return 0;
                }
            }
            if (num > 1)
                count += 1;
            return ((count % 2) == 0) ? 1 : -1;
        }

        private static int[] smallestPrimeFactors = null;

        public static bool IsSPFInitialized => smallestPrimeFactors != null;

        public static void InitSPF(int max)
		{
            smallestPrimeFactors = CalcSPF(max);
		}

        public static int[] CalcSPF(int max)
		{
            var spf = new int[max + 1];
            for (var i = 1; i <= max; i += 1)
                spf[i] = i;
            for (var i = 2L; i * i <= max; i += 1)
			{
                if (spf[i] != i)
                    continue;
                for (var j = i * i; j <= max; j += i)
                    if (spf[j] == j)
                        spf[j] = (int)i;
			}
            return spf;
        }

        public static int GetSPF(int num)
		{
            if (smallestPrimeFactors != null)
                return smallestPrimeFactors[num];
            if (num < 2)
                return num;
            if ((num % 2) == 0)
                return 2;
            for (var i = 3L; i * i <= num; i += 2)
                if ((num % i) == 0)
                    return (int)i;
            return num;
		}
    }
}
