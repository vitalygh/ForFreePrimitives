using System;

namespace ForFreePrimitives
{
	public class MaxSegmentTree<T> where T : IComparable
	{
		private T[] nums = null;
		private int[] nodes = null;

		public MaxSegmentTree(T[] nums)
		{
			this.nums = nums;
			nodes = new int[nums.Length * 4];
			for (var i = 0; i < nodes.Length; i += 1)
				nodes[i] = -1;
		}

		public void Update(int index, T value)
		{
			nums[index] = value;
			Update(1, index, 0, nums.Length - 1);
		}

		private void Update(int nodeIndex, int valueIndex, int left, int right)
		{
			nodes[nodeIndex] = -1;
			if (left == right)
				return;
			var mid = left + (right - left) / 2;
			if (valueIndex <= mid)
				Update(2 * nodeIndex, valueIndex, left, mid);
			else
				Update(2 * nodeIndex + 1, valueIndex, mid + 1, right);
		}

		private int GetMax(int index, int left, int right)
		{
			var max = nodes[index];
			if (max >= 0)
				return max;
			if (left == right)
			{
				nodes[index] = left;
				return nodes[index];
			}
			var mid = left + (right - left) / 2;
			var leftMax = GetMax(2 * index, left, mid);
			var rightMax = GetMax(2 * index + 1, mid + 1, right);
			if (nums[leftMax].CompareTo(nums[rightMax]) < 0)
				nodes[index] = rightMax;
			else
				nodes[index] = leftMax;
			return nodes[index];
		}

		public int GetFirstGreater(T value)
		{
			return GetFirstGreater(1, value, 0, nums.Length - 1);
		}

		private int GetFirstGreater(int index, T value, int left, int right)
		{
			var max = GetMax(index, left, right);
			if (nums[max].CompareTo(value) <= 0)
				return -1;
			if (left == right)
				return max;
			var mid = left + (right - left) / 2;
			var leftGreater = GetFirstGreater(index * 2, value, left, mid);
			if (leftGreater >= 0)
				return leftGreater;
			return GetFirstGreater(index * 2 + 1, value, mid + 1, right);
		}
	}

	public class LengthSegmentTree
	{
		private class Node
		{
			public int left = 0;
			public int right = 0;
			public int count = 0;
			public int length = 0;
		}

		private Node[] nodes = null;
		private int[] nums = null;

		public LengthSegmentTree(int[] nums)
		{
			this.nums = nums;
			nodes = new Node[(nums.Length - 1) * 4];
			for (var i = 0; i < nodes.Length; i += 1)
				nodes[i] = new Node();
			Build(1, 0, nums.Length - 2);
		}

		private void Build(int node, int left, int right)
		{
			nodes[node].left = left;
			nodes[node].right = right;
			if (left != right)
			{
				var mid = left + (right - left) / 2;
				Build(node * 2, left, mid);
				Build(node * 2 + 1, mid + 1, right);
			}
		}

		public void Update(int left, int right, int count)
		{
			Update(1, left, right, count);
		}

		private void Update(int node, int left, int right, int count)
		{
			var n = nodes[node];
			if ((n.left >= left) && (n.right <= right))
				n.count += count;
			else
			{
				var mid = n.left + (n.right - n.left) / 2;
				if (left <= mid)
					Update(node * 2, left, right, count);
				if (right > mid)
					Update(node * 2 + 1, left, right, count);
			}
			if (n.count > 0)
				n.length = nums[n.right + 1] - nums[n.left];
			else if (n.left == n.right)
				n.length = 0;
			else
				n.length = nodes[node * 2].length + nodes[node * 2 + 1].length;
		}

		public int Length { get => nodes[1].length; }
	}

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
