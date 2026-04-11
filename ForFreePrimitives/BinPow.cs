using System;
using System.Collections.Generic;
using System.Numerics;


namespace ForFreePrimitives
{
    public static class BinExp
    {
        public static BigInteger BinPow(BigInteger val, BigInteger exp)
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

        public static BigInteger BinPow(BigInteger val, BigInteger exp, BigInteger mod)
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

        public static long BinPow(long val, long exp)
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

        public static long BinPow(long val, long exp, long mod)
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

        public static int BinPow(int val, int exp)
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

        public static int BinPow(int val, int exp, int mod)
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

        public static int[][] MulMatrix(int[][] a, int[][] b, int modulo)
        {
            var am = a.Length;
            var an = a[0].Length;
            var bm = b.Length;
            var bn = b[0].Length;
            if (an != bm)
                return null;
            var c = new int[am][];
            for (var i = 0; i < c.Length; i += 1)
            {
                c[i] = new int[bn];
                for (var j = 0; j < c[i].Length; j += 1)
                {
                    var sum = 0L;
                    for (var k = 0; k < an; k += 1)
                    {
                        sum += ((long)a[i][k] * b[k][j]) % modulo;
                        sum %= modulo;
                    }
                    c[i][j] = (int)sum;
                }
            }
            return c;
        }

        public static int[][] BinPow(int[][] val, int exp, int modulo)
        {
            var n = val.Length;
            var r = new int[n][];
            for (var i = 0; i < n; i += 1)
            {
                r[i] = new int[n];
                r[i][i] = 1;
            }
            while (exp > 0)
            {
                if ((exp & 1) != 0)
                    r = MulMatrix(r, val, modulo);
                val = MulMatrix(val, val, modulo);
                exp >>= 1;
            }
            return r;
        }
    }
}
