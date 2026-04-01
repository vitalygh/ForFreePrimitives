using System;
using System.Collections.Generic;

namespace ForFreePrimitives
{
	public class MinSegmentTree<T> : SelectSegmentTree<T> where T : IComparable
	{
		public MinSegmentTree(T[] nums) : base(nums) { }

		protected override int SelectIndex(int a, int b) => nums[a].CompareTo(nums[b]) < 0 ? a : b;

		public int GetFirstLesser(T value)
		{
			return GetLesser(1, value, 0, nums.Length - 1, 0, nums.Length - 1, true);
		}

		public int GetFirstLesser(T value, int leftBound, int rightBound)
		{
			if (leftBound > rightBound)
				return -1;
			return GetLesser(1, value, 0, nums.Length - 1, leftBound, rightBound, true);
		}

		public int GetLastLesser(T value)
		{
			return GetLesser(1, value, 0, nums.Length - 1, 0, nums.Length - 1, false);
		}

		public int GetLastLesser(T value, int leftBound, int rightBound)
		{
			if (leftBound > rightBound)
				return -1;
			return GetLesser(1, value, 0, nums.Length - 1, Math.Max(0, leftBound), Math.Min(nums.Length - 1, rightBound), false);
		}

		private int GetLesser(int index, T value, int left, int right, int leftBound, int rightBound, bool leftFirst)
		{
			var min = GetSelectedIndex(index, left, right);
			if (nums[min].CompareTo(value) >= 0)
				return -1;
			if (left == right)
				return min;
			var mid = left + (right - left) / 2;
			Func<int> leftQuery = () => GetLesser(2 * index, value, left, mid, leftBound, rightBound, leftFirst);
			Func<int> rightQuery = () => GetLesser(2 * index + 1, value, mid + 1, right, leftBound, rightBound, leftFirst);
			if (rightBound <= mid)
				return leftQuery();
			if (leftBound > mid)
				return rightQuery();
			var first = leftFirst ? leftQuery() : rightQuery();
			if (first >= 0)
				return first;
			return leftFirst ? rightQuery() : leftQuery();
		}
	}

	public class MaxSegmentTree<T> : SelectSegmentTree<T> where T : IComparable
	{
		public MaxSegmentTree(T[] nums) : base(nums) { }

		protected override int SelectIndex(int a, int b) => nums[a].CompareTo(nums[b]) > 0 ? a : b;

		public int GetFirstGreater(T value)
		{
			return GetGreater(1, value, 0, nums.Length - 1, 0, nums.Length - 1, true);
		}

		public int GetFirstGreater(T value, int leftBound, int rightBound)
		{
			if (leftBound > rightBound)
				return -1;
			return GetGreater(1, value, 0, nums.Length - 1, leftBound, rightBound, true);
		}

		public int GetLastGreater(T value)
		{
			return GetGreater(1, value, 0, nums.Length - 1, 0, nums.Length - 1, false);
		}

		public int GetLastGreater(T value, int leftBound, int rightBound)
		{
			if (leftBound > rightBound)
				return -1;
			return GetGreater(1, value, 0, nums.Length - 1, leftBound, rightBound, false);
		}

		private int GetGreater(int index, T value, int left, int right, int leftBound, int rightBound, bool leftFirst)
		{
			var max = GetSelectedIndex(index, left, right);
			if (nums[max].CompareTo(value) <= 0)
				return -1;
			if (left == right)
				return max;
			var mid = left + (right - left) / 2;
			Func<int> leftQuery = () => GetGreater(2 * index, value, left, mid, leftBound, rightBound, leftFirst);
			Func<int> rightQuery = () => GetGreater(2 * index + 1, value, mid + 1, right, leftBound, rightBound, leftFirst);
			if (rightBound <= mid)
				return leftQuery();
			if (leftBound > mid)
				return rightQuery();
			var leftGreater = leftFirst ? leftQuery() : rightQuery();
			if (leftGreater >= 0)
				return leftGreater;
			return leftFirst ? rightQuery() : leftQuery();
		}
	}

	public abstract class SelectSegmentTree<T> where T : IComparable
	{
		protected T[] nums = null;
		private int[] nodes = null;

		public SelectSegmentTree(T[] nums)
		{
			this.nums = nums;
			nodes = new int[nums.Length * 4];
			for (var i = 0; i < nodes.Length; i += 1)
				nodes[i] = -1;
		}

		protected abstract int SelectIndex(int a, int b);

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

		protected int GetSelectedIndex(int index, int left, int right)
		{
			var val = nodes[index];
			if (val >= 0)
				return val;
			if (left == right)
			{
				nodes[index] = left;
				return nodes[index];
			}
			var mid = left + (right - left) / 2;
			var leftVal = GetSelectedIndex(2 * index, left, mid);
			var rightVal = GetSelectedIndex(2 * index + 1, mid + 1, right);
			nodes[index] = SelectIndex(leftVal, rightVal);
			return nodes[index];
		}

		public T GetValueInBounds(int leftBound, int rightBound)
		{
			var index = GetIndexInBounds(leftBound, rightBound);
			return nums[index];
		}

		public int GetIndexInBounds(int leftBound, int rightBound)
		{
			return GetIndexInBounds(1, 0, nums.Length - 1, leftBound, rightBound);
		}

		private int GetIndexInBounds(int index, int left, int right, int leftBound, int rightBound)
		{
			if ((leftBound <= left) && (right <= rightBound))
				return GetSelectedIndex(index, left, right);
			if (left >= right)
				return -1;
			var mid = left + (right - left) / 2;
			if (rightBound <= mid)
				return GetIndexInBounds(index * 2, left, mid, leftBound, rightBound);
			if (leftBound > mid)
				return GetIndexInBounds(index * 2 + 1, mid + 1, right, leftBound, rightBound);
			var leftMax = GetIndexInBounds(index * 2, left, mid, leftBound, mid);
			var rightMax = GetIndexInBounds(index * 2 + 1, mid + 1, right, mid + 1, rightBound);
			return SelectIndex(leftMax, rightMax);
		}
	}

	public class MinMaxLazySegmentTree
	{
		private class Node
		{
			public int min = 0;
			public int max = 0;
			public int update = 0;
		}

		private Node[] nodes = null;

		public int Size => nodes.Length / 4;

		public MinMaxLazySegmentTree(int size)
		{
			nodes = new Node[size * 4];
			for (var i = 0; i < nodes.Length; i += 1)
				nodes[i] = new Node();
		}

		public void Update(int leftBound, int rightBound, int value)
		{
			Update(1, 0, Size - 1, leftBound, rightBound, value);
		}

		public int GetMin(int leftBound, int rightBound)
		{
			return GetMin(1, 0, Size - 1, leftBound, rightBound);
		}

		public int GetMax(int leftBound, int rightBound)
		{
			return GetMax(1, 0, Size - 1, leftBound, rightBound);
		}

		public int GetLastZeroIndex(int leftBound, int rightBound)
		{
			return GetLastZeroIndex(1, 0, Size - 1, leftBound, rightBound);
		}

		private int GetLastZeroIndex(int nodeIndex, int left, int right, int leftBound, int rightBound)
		{
			var mid = left + (right - left) / 2;
			var node = nodes[nodeIndex];
			if (left < right)
			{
				PushUpdate(nodeIndex, left, right);
				if (rightBound <= mid)
					return GetLastZeroIndex(2 * nodeIndex, left, mid, leftBound, rightBound);
				if (leftBound > mid)
					return GetLastZeroIndex(2 * nodeIndex + 1, mid + 1, right, leftBound, rightBound);
			}
			if (left == right)
				return node.update == 0 ? left : -1;
			var min = GetMin(2 * nodeIndex + 1, mid + 1, right, leftBound, rightBound);
			var max = GetMax(2 * nodeIndex + 1, mid + 1, right, leftBound, rightBound);
			if ((min == 0) && (max == 0))
				return Math.Min(right, rightBound);
			if ((min <= 0) && (max >= 0))
			{
				var rightIndex = GetLastZeroIndex(2 * nodeIndex + 1, mid + 1, right, leftBound, rightBound);
				if (rightIndex != -1)
					return rightIndex;
			}
			min = GetMin(2 * nodeIndex, left, mid, leftBound, rightBound);
			max = GetMax(2 * nodeIndex, left, mid, leftBound, rightBound);
			if ((min == 0) && (max == 0))
				return Math.Min(mid, rightBound);
			if ((min <= 0) && (max >= 0))
				return GetLastZeroIndex(2 * nodeIndex, left, mid, leftBound, rightBound);
			return -1;
		}

		private void PushUpdate(int nodeIndex, int left, int right)
		{
			var node = nodes[nodeIndex];
			if (node.update == 0)
				return;
			var mid = left + (right - left) / 2;
			Update(2 * nodeIndex, left, mid, left, mid, node.update);
			Update(2 * nodeIndex + 1, mid + 1, right, mid + 1, right, node.update);
			node.update = 0;
			var leftMin = GetMin(2 * nodeIndex, left, mid, left, mid);
			var rightMin = GetMin(2 * nodeIndex + 1, mid + 1, right, mid + 1, right);
			node.min = Math.Min(leftMin, rightMin);
			var leftMax = GetMax(2 * nodeIndex, left, mid, left, mid);
			var rightMax = GetMax(2 * nodeIndex + 1, mid + 1, right, mid + 1, right);
			node.max = Math.Max(leftMax, rightMax);
		}

		private int GetMin(int nodeIndex, int left, int right, int leftBound, int rightBound)
		{
			var node = nodes[nodeIndex];
			if ((leftBound <= left) && (right <= rightBound))
				return node.min + node.update;
			PushUpdate(nodeIndex, left, right);
			var mid = left + (right - left) / 2;
			if (rightBound <= mid)
				return GetMin(2 * nodeIndex, left, mid, leftBound, rightBound);
			if (leftBound > mid)
				return GetMin(2 * nodeIndex + 1, mid + 1, right, leftBound, rightBound);
			var leftVal = GetMin(2 * nodeIndex, left, mid, leftBound, rightBound);
			var rightVal = GetMin(2 * nodeIndex + 1, mid + 1, right, leftBound, rightBound);
			return Math.Min(leftVal, rightVal);
		}

		private int GetMax(int nodeIndex, int left, int right, int leftBound, int rightBound)
		{
			var node = nodes[nodeIndex];
			if ((leftBound <= left) && (right <= rightBound))
				return node.max + node.update;
			PushUpdate(nodeIndex, left, right);
			var mid = left + (right - left) / 2;			
			if (rightBound <= mid)
				return GetMax(2 * nodeIndex, left, mid, leftBound, rightBound);
			if (leftBound > mid)
				return GetMax(2 * nodeIndex + 1, mid + 1, right, leftBound, rightBound);
			var leftVal = GetMax(2 * nodeIndex, left, mid, leftBound, rightBound);
			var rightVal = GetMax(2 * nodeIndex + 1, mid + 1, right, leftBound, rightBound);
			return Math.Max(leftVal, rightVal);
		}

		private void Update(int nodeIndex, int left, int right, int leftBound, int rightBound, int value)
		{
			var node = nodes[nodeIndex];
			if ((left >= leftBound) && (right <= rightBound))
			{
				node.update += value;
				return;
			}
			var mid = left + (right - left) / 2;
			if (node.update != 0)
			{
				Update(2 * nodeIndex, left, mid, left, mid, node.update);
				Update(2 * nodeIndex + 1, mid + 1, right, mid + 1, right, node.update);
				node.update = 0;
			}
			if (leftBound <= mid)
				Update(2 * nodeIndex, left, mid, leftBound, rightBound, value);
			if (rightBound > mid)
				Update(2 * nodeIndex + 1, mid + 1, right, leftBound, rightBound, value);
			var	leftMin = GetMin(2 * nodeIndex, left, mid, left, mid);
			var rightMin = GetMin(2 * nodeIndex + 1, mid + 1, right, mid + 1, right);
			node.min = Math.Min(leftMin, rightMin);
			var	leftMax = GetMax(2 * nodeIndex, left, mid, left, mid);
			var rightMax = GetMax(2 * nodeIndex + 1, mid + 1, right, mid + 1, right);
			node.max = Math.Max(leftMax, rightMax);
		}
	}

	public class MajoritySegmentTree
	{
		private (int value, int count)[] nodes = null;
		private Dictionary<int, List<int>> indexes = new Dictionary<int, List<int>>();

		public int Size => nodes.Length / 4;

		public MajoritySegmentTree(int[] nums)
		{
			nodes = new (int, int)[nums.Length * 4];
			if (nums.Length > 0)
			{
				Build(1, 0, nums.Length - 1, nums);
				for (var i = 0; i < nums.Length; i += 1)
				{
					var num = nums[i];
					if (!indexes.TryGetValue(num, out var list))
					{
						list = new List<int>();
						indexes.Add(num, list);
					}
					list.Add(i);
				}
			}
		}

		private (int value, int count) Merge((int value, int count) left, (int value, int count) right)
		{
			if (left.value == right.value)
				return (left.value, left.count + right.count);
			if (left.count > right.count)
				return (left.value, left.count - right.count);
			return (right.value, right.count - left.count);
		}

		private void Build(int nodeIndex, int left, int right, int[] nums)
		{
			if (left >= right)
			{
				nodes[nodeIndex] = (nums[left], 1);
				return;
			}
			var mid = left + (right - left) / 2;
			Build(2 * nodeIndex, left, mid, nums);
			Build(2 * nodeIndex + 1, mid + 1, right, nums);
			var ln = nodes[2 * nodeIndex];
			var rn = nodes[2 * nodeIndex + 1];
			nodes[nodeIndex] = Merge(ln, rn);
		}

		private int Greater(IList<int> nums, int target)
		{
			var start = 0;
			var end = nums.Count - 1;
			while (start <= end)
			{
				var mid = start + (end - start) / 2;
				if (nums[mid] <= target)
					start = mid + 1;
				else
					end = mid - 1;
			}
			return end + 1;
		}

		public (int value, int count) Query(int left, int right)
		{
			var value = Query(1, 0, Size - 1, left, right).value;
			var idxs = indexes[value];
			var count = Greater(idxs, right) - Greater(idxs, left - 1);
			return (value, count);
		}

		private (int value, int count) Query(int nodeIndex, int left, int right, int leftBound, int rightBound)
		{
			if ((left >= leftBound) && (right <= rightBound))
			{
				var node = nodes[nodeIndex];
				return (node.value, node.count);
			}
			var mid = left + (right - left) / 2;
			if (rightBound <= mid)
				return Query(2 * nodeIndex, left, mid, leftBound, rightBound);
			if (leftBound > mid)
				return Query(2 * nodeIndex + 1, mid + 1, right, leftBound, rightBound);
			var ln = Query(2 * nodeIndex, left, mid, leftBound, rightBound);
			var rn = Query(2 * nodeIndex + 1, mid + 1, right, leftBound, rightBound); ;
			return Merge(ln, rn);
		}
	}

	public class DynamicConnectivitySegmentTree
	{
		private List<(int u, int v)>[] nodes = null;

		public int Size { get => nodes.Length / 4; }

		public DynamicConnectivitySegmentTree(int size)
		{
			nodes = new List<(int, int)>[size * 4];
			for (var i = 0; i < nodes.Length; i += 1)
				nodes[i] = new List<(int u, int v)>();
		}

		public void Update(int u, int v, int leftBound, int rightBound)
		{
			Update(1, 0, Size - 1, u, v, leftBound, rightBound);
		}

		private void Update(int n, int left, int right, int u, int v, int leftBound, int rightBound)
		{
			if ((leftBound <= left) && (rightBound >= right))
			{
				nodes[n].Add((u, v));
				return;
			}
			var mid = left + (right - left) / 2;
			if (leftBound <= mid)
				Update(n * 2, left, mid, u, v, leftBound, rightBound);
			if (rightBound > mid)
				Update(n * 2 + 1, mid + 1, right, u, v, leftBound, rightBound);
		}

		public void Query(Action<int, int> join, Action<int, int> disjoin, Action<int, int> query)
		{
			Query(1, 0, Size - 1, join, disjoin, query);
		}

		private void Query(int n, int left, int right, Action<int, int> join, Action<int, int> disjoin, Action<int, int> query)
		{
			var node = nodes[n];
			var count = node.Count;
			for (var i = 0; i < node.Count; i += 1)
			{
				var (u, v) = node[i];
				join(u, v);
			}
			query(left, right);
			if (left < right)
			{
				var mid = left + (right - left) / 2;
				Query(n * 2, left, mid, join, disjoin, query);
				Query(n * 2 + 1, mid + 1, right, join, disjoin, query);
			}
			for (var i = node.Count - 1; i >= 0; i -= 1)
			{
				var (u, v) = node[i];
				disjoin(u, v);
			}
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
