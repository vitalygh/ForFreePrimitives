using System;
using System.Collections;
using System.Collections.Generic;

namespace ForFreePrimitives
{
	public class RBTree<T>: IEnumerable<T> where T: IComparable<T>
	{
		private class Node
		{
			public Node parent = null;
			public Node left = null;
			public Node right = null;
			public bool black = false;
			public T value = default(T);
			public int count = 1;
			public int subtreeSize = 1;

			public IEnumerator<T> GetEnumerator()
			{
				if (left != null)
					foreach (var item in left)
						yield return item;
				for (var i = 0; i < count; i +=	1)
					yield return value;
				if (right != null)
					foreach (var item in right)
						yield return item;
			}
		}

		private Node root = null;

		private static bool IsBlack(Node node) => node?.black ?? true;
		private static bool IsRed(Node node) => !IsBlack(node);

		public void Add(T value)
		{
			if (root == null)
			{
				root = new Node()
				{
					value = value,
					black = true
				};
				return;
			}
			Add(root, value);
		}

		private void Add(Node node, T value)
		{
			if (value.CompareTo(node.value) == 0)
			{
				node.count += 1;
				var parent = node;
				while (parent != null)
				{
					parent.subtreeSize += 1;
					parent = parent.parent;
				}
				return;
			}
			if (value.CompareTo(node.value) < 0)
			{
				if (node.left != null)
					Add(node.left, value);
				else
				{
					node.left = new Node()
					{
						parent = node,
						value = value,
						black = false,
						count = 1,
					};
					var parent = node;
					while (parent != null)
					{
						parent.subtreeSize += 1;
						parent = parent.parent;
					}
					BalanceAfterAdd(node.left);
				}
				return;
			}
			if (node.right != null)
				Add(node.right, value);
			else
			{
				node.right = new Node()
				{
					parent = node,
					value = value,
					black = false,
					count = 1,
					subtreeSize = 1,
				};
				var parent = node;
				while (parent != null)
				{
					parent.subtreeSize += 1;
					parent = parent.parent;
				}
				BalanceAfterAdd(node.right);
			}
		}

		public bool Remove(T value)
		{
			return Remove(root, value);
		}

		private bool Remove(Node node, T value)
		{
			if (node == null)
				return false;
			if (value.CompareTo(node.value) < 0)
				return Remove(node.left, value);
			if (value.CompareTo(node.value) > 0)
				return Remove(node.right, value);
			if (node.count > 1)
			{
				node.count -= 1;
				var current = node;
				while (current != null)
				{
					current.subtreeSize -= 1;
					current = current.parent;
				}
				return true;
			}
			if ((node.left == null) || (node.right == null))
			{
				var current = node;
				while (current != null)
				{
					current.subtreeSize -= 1;
					current = current.parent;
				}
				var wasLeft = true;
				var child = node.left ?? node.right;
				if (node.parent == null)
					root = child;
				else if (node.parent.left == node)
					node.parent.left = child;
				else
				{
					node.parent.right = child;
					wasLeft = false;
				}
				if (child != null)
					child.parent = node.parent;
				if (IsBlack(node))
					BalanceAfterRemove(node.parent, wasLeft);
				return true;
			}
			var nextNode = Next(node);
			if (nextNode.count != node.count)
			{
				var current = nextNode;
				while (current != null)
				{
					current.subtreeSize += node.count - nextNode.count; 
					current = current.parent;
				}
				current = node;
				while (current != null)
				{
					current.subtreeSize += nextNode.count - node.count;
					current = current.parent;
				}
			}
			(node.value, nextNode.value) = (nextNode.value, node.value);
			(node.count, nextNode.count) = (nextNode.count, node.count);
			return Remove(nextNode, value);
		}

		private static Node Next(Node node)
		{
			if (node == null)
				return null;
			if (node.right != null)
				node = node.right;
			else
			{
				if (node.parent == null)
					return null;
				if (node.parent.right == null)
					return node.parent;
				if (node.parent.right == node)
					return null;
				node = node.parent.right;
			}
			while (node.left != null)
				node = node.left;
			return node;
		}

		private static void SetLeft(Node parent, Node node)
		{
			if (node != null)
				node.parent = parent;
			parent.left = node;
		}

		private static void SetRight(Node parent, Node node)
		{
			if (node != null)
				node.parent = parent;
			if (parent != null)
				parent.right = node;
		}

		private void Replace(Node parent, Node from, Node to)
		{
			to.parent = parent;
			if (parent != null)
			{
				if (parent.left == from)
					parent.left = to;
				else
					parent.right = to;
				return;
			}
			root = to;
		}

		private void LeftRotate(Node node)
		{
			var parent = node.parent;
			var right = node.right;
			var rightLeft = right.left;
			node.subtreeSize -= right.subtreeSize;
			right.subtreeSize += node.subtreeSize;
			node.subtreeSize += rightLeft?.subtreeSize ?? 0;
			SetRight(node, rightLeft);
			SetLeft(right, node);
			Replace(parent, node, right);
		}

		private void RightRotate(Node node)
		{
			var parent = node.parent;
			var left = node.left;
			var leftRight = left.right;
			node.subtreeSize -= left.subtreeSize;
			left.subtreeSize += node.subtreeSize;
			node.subtreeSize += leftRight?.subtreeSize ?? 0;
			SetLeft(node, leftRight);
			SetRight(left, node);
			Replace(parent, node, left);
		}

		private void BalanceAfterAdd(Node node)
		{
			var parent = node.parent;
			if (parent == null)
			{
				node.black = true;
				return;
			}
			if (IsBlack(parent))
				return;
			var grandparent = parent.parent;
			var uncle = grandparent.left == parent ? grandparent.right : grandparent.left;
			if (IsRed(uncle))
			{
				node.parent.black = true;
				uncle.black = true;
				grandparent.black = false;
				BalanceAfterAdd(grandparent);
				return;
			}
			if (grandparent.left == parent)
			{
				if (parent.right == node)
				{
					LeftRotate(parent);
					node = parent;
					parent = node.parent;
				}
				grandparent.black = false;
				parent.black = true;
				RightRotate(grandparent);
			}
			else
			{
				if (parent.left == node)
				{
					RightRotate(parent);
					node = parent;
					parent = node.parent;
				}
				grandparent.black = false;
				parent.black = true;
				LeftRotate(grandparent);
			}
		}

		private void BalanceAfterRemove(Node parent, bool left)
		{
			if (parent == null)
				return;
			var sibling = left ? parent.right : parent.left;
			if (left)
			{
				if (IsRed(sibling))
				{
					sibling.black = true;
					parent.black = false;
					LeftRotate(parent);
					sibling = parent.right;
				}
				if (IsBlack(sibling.left) && IsBlack(sibling.right))
				{
					sibling.black = false;
					if (IsRed(parent))
						parent.black = true;
					else
					{
						var grandparent = parent.parent;
						var parentIsLeft = (grandparent != null) && (grandparent.left == parent) ? true : false;
						BalanceAfterRemove(grandparent, parentIsLeft);
					}
					return;
				}
				if (IsBlack(sibling.right))
				{
					sibling.left.black = false;
					sibling.black = false;
					RightRotate(sibling);
					sibling = sibling.parent;
				}
				sibling.black = parent.black;
				parent.black = true;
				sibling.right.black = true;
				LeftRotate(parent);
			}
			else
			{
				if (IsRed(sibling))
				{
					sibling.black = true;
					parent.black = false;
					RightRotate(parent);
					sibling = parent.left;
				}
				if (IsBlack(sibling.left) && IsBlack(sibling.right))
				{
					sibling.black = false;
					if (IsRed(parent))
						parent.black = true;
					else
					{
						var grandparent = parent.parent;
						var parentIsLeft = (grandparent != null) && (grandparent.left == parent) ? true : false;
						BalanceAfterRemove(grandparent, parentIsLeft);
					}
					return;
				}
				if (IsBlack(sibling.left))
				{
					sibling.right.black = false;
					sibling.black = false;
					LeftRotate(sibling);
					sibling = sibling.parent;
				}
				sibling.black = parent.black;
				parent.black = true;
				sibling.left.black = true;
				RightRotate(parent);
			}
		}

		public bool GetGreater(T value, out T result)
		{
			return GetGreater(root, value, out result);
		}

		private static bool GetGreater(Node node, T value, out T result)
		{
			result = default(T);
			if (node == null)
				return false;
			if (node.value.CompareTo(value) <= 0)
				return GetGreater(node.right, value, out result);
			if (GetGreater(node.left, value, out result))
				return true;
			result = node.value;
			return true;
		}

		public bool GetLesser(T value, out T result)
		{
			return GetLesser(root, value, out result);
		}

		private static bool GetLesser(Node node, T value, out T result)
		{
			result = default(T);
			if (node == null)
				return false;
			if (node.value.CompareTo(value) >= 0)
				return GetLesser(node.left, value, out result);
			if (GetLesser(node.right, value, out result))
				return true;
			result = node.value;
			return true;
		}

		public int GetGreaterCount(T value)
		{
			return GetGreaterCount(root, value);
		}

		private static int GetGreaterCount(Node node, T value)
		{
			if (node == null)
				return 0;
			if (node.value.CompareTo(value) <= 0)
				return GetGreaterCount(node.right, value);
			var count = node.count;
			count += node.right?.subtreeSize ?? 0;
			count += GetGreaterCount(node.left, value);
			return count;
		}

		public int GetLesserCount(T value)
		{
			return GetLesserCount(root, value);
		}

		private static int GetLesserCount(Node node, T value)
		{
			if (node == null)
				return 0;
			if (node.value.CompareTo(value) >= 0)
				return GetLesserCount(node.left, value);
			var count = node.count;
			count += node.left?.subtreeSize ?? 0;
			count += GetLesserCount(node.right, value);
			return count;
		}

		public int GetCount(T value)
		{
			return GetCount(root, value);
		}

		private static int GetCount(Node node, T value)
		{
			if (node == null)
				return 0;
			if (node.value.CompareTo(value) == 0)
				return node.count;
			if (node.value.CompareTo(value) < 0)
				return GetCount(node.right, value);
			return GetCount(node.left, value);
		}

		public IEnumerator<T> GetEnumerator()
		{
			if (root != null)
				foreach (var item in root)
					yield return item;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public bool IsValid()
		{
			if (!ValidateParent(root))
				return false;
			if (ValidateBlackHeight(root) < 0)
				return false;
			if (ValidateSubtreeSize(root) < 0)
				return false;
			return true;
		}

		private bool ValidateParent(Node node, Node parent = null)
		{
			if (node == null)
				return true;
			if (node.parent != parent)
				return false;
			return ValidateParent(node.left, node) && ValidateParent(node.right, node);
		}

		private int ValidateSubtreeSize(Node node)
		{
			if (node == null)
				return 0;
			var left = ValidateSubtreeSize(node.left);
			if (left < 0)
				return left;
			var right = ValidateSubtreeSize(node.right);
			if (right < 0)
				return right;
			var subtreeSize = node.count + left + right;
			if (node.subtreeSize != subtreeSize)
				return -1;
			return subtreeSize;
		}

		private int ValidateBlackHeight(Node node)
		{
			if (node == null)
				return 0;
			var left = ValidateBlackHeight(node.left);
			if (left < 0)
				return left;
			if (IsBlack(node.left))
				left += 1;
			var right = ValidateBlackHeight(node.right);
			if (right < 0)
				return right;
			if (IsBlack(node.right))
				right += 1;
			if (left != right)
				return -1;
			return left;
		}
	}
}
