using System;
using System.Collections.Generic;
using System.Numerics;


namespace ForFreePrimitives
{
    public static class BinPow
    {
        public static BigInteger Calc(BigInteger val, BigInteger exp)
        {
            var result = new BigInteger(1);
            while (exp > 0)
            {
                if ((exp & 1) == 1)
                    result = result * val;
                val = val * val;
                exp >>= 1;
            }
            return result;
        }

        public static BigInteger Calc(BigInteger val, BigInteger exp, BigInteger mod)
        {
            var result = new BigInteger(1);
            val %= mod;
            while (exp > 0)
            {
                if ((exp & 1) == 1)
                    result = result * val % mod;
                val = val * val % mod;
                exp >>= 1;
            }
            return result;
        }

        public static long Calc(long val, long exp)
        {
            var result = 1L;
            while (exp > 0)
            {
                if ((exp & 1) == 1)
                    result = result * val;
                val = val * val;
                exp >>= 1;
            }
            return result;
        }

        public static long Calc(long val, long exp, long mod)
        {
            var result = 1L;
            val %= mod;
            while (exp > 0)
            {
                if ((exp & 1) == 1)
                    result = result * val % mod;
                val = val * val % mod;
                exp >>= 1;
            }
            return result;
        }

        public static int Calc(int val, int exp)
        {
            var result = 1;
            while (exp > 0)
            {
                if ((exp & 1) == 1)
                    result = result * val;
                val = val * val;
                exp >>= 1;
            }
            return result;
        }

        public static int Calc(int val, int exp, int mod)
        {
            var result = 1L;
            var v = (long)val;
            v %= mod;
            while (exp > 0)
            {
                if ((exp & 1) == 1)
                    result = result * v % mod;
                v = v * v % mod;
                exp >>= 1;
            }
            return (int)result;
        }

    }
}
