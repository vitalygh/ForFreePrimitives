using System;

namespace ForFreePrimitives
{
	public class SegmentTree
	{
		private class Node
		{
			public int start = 0;
			public int mid = 0;
			public int end = 0;
			public int count = 0;
			public Node left = null;
			public Node right = null;

			public Node(int start, int end, int count)
			{
				this.start = start;
				mid = -1;
				this.end = end;
				this.count = count;
			}
		}

		private readonly Node root = null;

		public SegmentTree(int minValue, int maxValue)
		{
			root = new Node(minValue, maxValue, 0);
		}

		public int Query(int start, int end)
		{
			return Query(start, end, root);
		}

		private int Query(int start, int end, Node root)
		{
			if (root.mid != -1)
			{
				if (start >= root.mid)
					return Query(start, end, root.right);
				else if (end <= root.mid)
					return Query(start, end, root.left);
				else
				{
					var left = Query(start, root.mid, root.left);
					var right = Query(root.mid, end, root.right);
					return Math.Max(left, right);
				}
			}
			if (start >= root.start || end <= root.end)
				return root.count;
			else
				return 0;
		}

		public void Add(int start, int end)
		{
			Add(start, end, root);
		}

		private void Add(int start, int end, Node root)
		{
			if (root.mid != -1)
			{
				if (start >= root.mid)
					Add(start, end, root.right);
				else if (end <= root.mid)
					Add(start, end, root.left);
				else
				{
					Add(start, root.mid, root.left);
					Add(root.mid, end, root.right);
				}
				return;
			}
			if (start == root.start && end == root.end)
				root.count++;
			else if (start == root.start)
			{
				root.mid = end;
				root.left = new Node(start, end, root.count + 1);
				root.right = new Node(end, root.end, root.count);
			}
			else if (end == root.end)
			{
				root.mid = start;
				root.left = new Node(root.start, start, root.count);
				root.right = new Node(start, end, root.count + 1);
			}
			else
			{
				root.mid = start;
				root.left = new Node(root.start, root.mid, root.count);
				root.right = new Node(root.mid, root.end, root.count);
				Add(start, end, root.right);
			}
		}
	}
}
