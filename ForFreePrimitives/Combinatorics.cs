namespace ForFreePrimitives
{
	public class Combinatorics
	{
        private int[] fact = null;
        private int[] ifact = null;
        private int modulo = 0;

        public void InitFactorials(int maxValue, int modulo)
        {
            this.modulo = modulo;
            fact = new int[maxValue + 1];
            fact[0] = 1;
            for (var i = 1; i < fact.Length; i += 1)
            {
                long prev = fact[i - 1];
                fact[i] = (int)((prev * i) % modulo);
            }
            ifact = new int[maxValue + 1];
            ifact[ifact.Length - 1] = BinPow.Calc(fact[fact.Length - 1], modulo - 2, modulo);
            for (var i = fact.Length - 2; i >= 0; i -= 1)
            {
                long prev = ifact[i + 1];
                ifact[i] = (int)((prev * (i + 1)) % modulo);
            }
        }

        public int Factorial(int value)
		{
            return fact[value];
		}

        public int InverseFactorial(int value)
        {
            return ifact[value];
        }

        public int Cnk(int n, int k)
        {
            var result = (long)fact[n];
            result *= ifact[n - k];
            result %= modulo;
            result *= ifact[k];
            result %= modulo;
            return (int)result;
        }
    }
}
