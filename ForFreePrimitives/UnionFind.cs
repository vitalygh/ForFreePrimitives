namespace ForFreePrimitives
{
	public class UnionFind
	{
		private int[] parents = null;
		private int[] size = null;
		private int count = 0;

		public UnionFind(int count)
		{
			this.count = count;
			parents = new int[count];
			size = new int[count];
			for (var i = 0; i < count; i += 1)
			{
				parents[i] = i;
				size[i] = 1;
			}
		}
		public int Find(int a)
		{
			if (parents[a] != a)
				parents[a] = Find(parents[a]);
			return parents[a];
		}
		public bool Union(int a, int b)
		{
			var pa = Find(a);
			var pb = Find(b);
			if (pa == pb)
				return false;
			if (size[pa] < size[pb])
			{
				parents[pa] = pb;
				size[pb] += size[pa];
			}
			else
			{
				parents[pb] = pa;
				size[pa] += size[pb];
			}
			count -= 1;
			return true;
		}
		public int GetSize(int a)
		{
			return size[Find(a)];
		}
		public int Count { get => count; }
	}
}
