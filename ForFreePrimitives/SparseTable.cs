using System;

namespace ForFreePrimitives
{
	public class SparseTable<T>
	{
		private T[][] table = null;
		private Func<T, T, T> func = null;

		public SparseTable(T[] arr, Func<T, T, T> func)
		{
			this.func = func;
			var n = arr.Length;
			var logn = (int)Math.Log(n, 2) + 1;
			table = new T[n + 1][];
			for (var i = 0; i < table.Length; i += 1)
				table[i] = new T[logn];
			for (var i = 0; i < arr.Length; i += 1)
				table[i][0] = arr[i];
			for (var j = 1; j < logn; j += 1)
				for (var i = 0; i + (1<<j) <= n; i += 1)				
					table[i][j] = func(table[i][j - 1], table[i + (1<<(j - 1))][j - 1]);
		}

		public T Query(int l, int r)
		{
			var j = (int)Math.Log(r - l + 1, 2);
			return func(table[l][j], table[r - (1 << j) + 1][j]);
		}
	}

	public class MinSparseTable<T>: SparseTable<T> where T: IComparable<T>
	{
		public MinSparseTable(T[] arr): base(arr, (x, y) => x.CompareTo(y) < 0 ? x : y)
		{

		}
	}

	public class MaxSparseTable<T> : SparseTable<T> where T : IComparable<T>
	{
		public MaxSparseTable(T[] arr) : base(arr, (x, y) => x.CompareTo(y) > 0 ? x : y)
		{

		}
	}

	public class GCDSparseTable : SparseTable<int>
	{
		public GCDSparseTable(int[] arr) : base(arr, Numbers.GCD)
		{

		}
	}
}
