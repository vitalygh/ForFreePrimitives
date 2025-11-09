using System;

namespace ForFreePrimitives
{
	public static class Numbers
	{
		public static int LCM(int a, int b)
		{
			return Math.Abs(a * b) / GCD(a, b);
		}

		public static int GCD(int a, int b)
		{
			while (a != 0 && b != 0)
			{
				if (a > b)
					a %= b;
				else
					b %= a;
			}
			return a | b;
		}
	}
}
