using System;

namespace ForFreePrimitives
{
	public class SumBIT
	{
		private int[] tree = null;

		public SumBIT(int size)
		{
			tree = new int[size + 1];
		}

		public void Update(int i, int v)
		{
			while (i < tree.Length)
			{
				tree[i] += v;
				i += i & -i;
			}
		}

		public int Query(int i)
		{
			var sum = 0;
			while (i > 0)
			{
				sum += tree[i];
				i -= i & -i;
			}
			return sum;
		}
	}

	public class MinBIT
	{
		private int[] tree = null;

		public MinBIT(int size)
		{
			tree = new int[size + 1];
			for (var i = 0; i < tree.Length; i += 1)
				tree[i] = int.MaxValue;
		}

		public void Update(int i, int v)
		{
			while (i < tree.Length)
			{
				tree[i] = Math.Min(tree[i], v);
				i += i & -i;
			}
		}

		public int Query(int i)
		{
			var min = int.MaxValue;
			while (i > 0)
			{
				min = Math.Min(tree[i], min);
				i -= i & -i;
			}
			return min;
		}
	}

	public class MaxBIT
	{
		private int[] tree = null;

		public MaxBIT(int size)
		{
			tree = new int[size + 1];
			for (var i = 0; i < tree.Length; i += 1)
				tree[i] = int.MinValue;
		}

		public void Update(int i, int v)
		{
			while (i < tree.Length)
			{
				tree[i] = Math.Max(tree[i], v);
				i += i & -i;
			}
		}

		public int Query(int i)
		{
			var max = int.MinValue;
			while (i > 0)
			{
				max = Math.Max(tree[i], max);
				i -= i & -i;
			}
			return max;
		}
	}
}
