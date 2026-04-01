using System;
using System.Collections.Generic;

namespace ForFreePrimitives
{
	public class UnionFind
	{
		protected int[] parents = null;
		protected int[] size = null;
		protected int count = 0;
		protected int maxSize = 0;
		
		public UnionFind(int count)
		{
			this.count = count;
			if (count > 0)
				maxSize = 1;
			parents = new int[count];
			size = new int[count];
			for (var i = 0; i < count; i += 1)
			{
				parents[i] = i;
				size[i] = 1;
			}
		}
		public virtual int Find(int a)
		{
			if (parents[a] != a)
				parents[a] = Find(parents[a]);
			return parents[a];
		}
		public virtual bool Union(int a, int b)
		{
			var pa = Find(a);
			var pb = Find(b);
			if (pa == pb)
				return false;
			if (size[pa] < size[pb])
				SetParent(pa, pb);
			else
				SetParent(pb, pa);
			count -= 1;
			return true;
		}

		protected virtual void SetParent(int lesser, int greater)
		{
			parents[lesser] = greater;
			size[greater] += size[lesser];
			maxSize = Math.Max(maxSize, size[greater]);
		}

		public int GetSize(int a)
		{
			return size[Find(a)];
		}
		public int Count { get => count; }
		public int MaxSize { get => maxSize; }
	}

	public class UnionFindUndo: UnionFind
	{
		private class UndoData
		{
			public int parentIndex = 0;
			public int parentValue = 0;
			public int sizeIndex = 0;
			public int sizeValue = 0;
			public int maxSize = 0;
			public int count = 0;
		}
		private Stack<UndoData> undo = new Stack<UndoData>();

		public UnionFindUndo(int count) : base(count)
		{
		}

		public override int Find(int a)
		{
			if (parents[a] == a)
				return a;
			return Find(parents[a]);
		}

		public bool Union(int a, int b, bool addEmptyUndo)
		{
			if (!base.Union(a, b))
			{
				if (addEmptyUndo)
					undo.Push(null);
				return false;
			}
			return true;
		}

		protected override void SetParent(int lesser, int greater)
		{
			undo.Push(new UndoData()
			{
				parentIndex = lesser,
				parentValue = parents[lesser],
				sizeIndex = greater,
				sizeValue = size[greater],
				maxSize = maxSize,
				count = count,
			});
			base.SetParent(lesser, greater);
		}

		public void Undo()
		{
			if (undo.Count < 1)
				return;
			var data = undo.Pop();
			if (data == null)
				return;
			parents[data.parentIndex] = data.parentValue;
			size[data.sizeIndex] = data.sizeValue;
			maxSize = data.maxSize;
			count = data.count;
		}
	}
}
