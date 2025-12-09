using System;
using System.Collections;
using System.Collections.Generic;

namespace ForFreePrimitives
{
	public class AVLTree<T>: IEnumerable<T> where T : IComparable<T>
	{
		private class Node
		{
			private AVLTree<T> tree = null;
			private Node parent = null;
			private Node left = null;
			private Node right = null;
			private int level = 1;
			private int count = 1;
			private T value = default;

			public Node(T value, AVLTree<T> tree)
			{
				this.value = value;
				this.tree = tree;
			}

			public void Add(T item)
			{
				var result = item.CompareTo(value);
				if (result < 0)
					if (left != null)
						left.Add(item);
					else
					{
						left = new Node(item, tree)
						{
							parent = this, 
						};
						left.Reconstruct();
					}
				else
					if (right != null)
						right.Add(item);
					else
					{
						right = new Node(item, tree)
						{ 
							parent = this, 
						};
						right.Reconstruct();
					}
			}

			public bool Remove(T item)
			{
				var result = item.CompareTo(value);
				if (result < 0)
					return left == null ? false : left.Remove(item);
				if (result > 0)
					return right == null ? false : right.Remove(item);
				if ((left == null) && (right == null))
					if (parent != null)
					{
						if (parent.left == this)
							parent.left = null;
						else
							parent.right = null;
						parent.Reconstruct();
					}
					else
						tree.root = null;
				else if ((left == null) || (right == null))
				{
					var child = left ?? right;
					if (parent != null)
					{
						if (parent.left == this)
							parent.left = child;
						else
							parent.right = child;
						child.parent = parent;
						parent.Reconstruct();
					}
					else
					{
						tree.root = child;
						child.parent = null;
					}
				}
				else
				{
					var remove = left;
					while (remove.right != null)
						remove = remove.right;
					(value, remove.value) = (remove.value, value);
					return remove.Remove(remove.value);
				}
				return true;
			}

			public IEnumerator<T> GetEnumerator()
			{
				if (left != null)
					foreach (var item in left)
						yield return item;
				yield return value;
				if (right != null)
					foreach (var item in right)
						yield return item;
			}

			private void Reconstruct(bool recursive = true)
			{
				count = 1;
				count += left?.count ?? 0;
				count += right?.count ?? 0;
				var leftLevel = left?.level ?? 0;
				var rightLevel = right?.level ?? 0;
				if (leftLevel - rightLevel > 1)
				{
					leftLevel = left.left?.level ?? 0;
					rightLevel = left.right?.level ?? 0;
					if (leftLevel >= rightLevel)
					{
						left.Elevate();
						Reconstruct();
					}
					else
					{
						var pivot = left.right;
						pivot.Elevate();
						pivot.Elevate();
						pivot.left?.Reconstruct(false);
						pivot.right?.Reconstruct();
					}
				}
				else if(rightLevel - leftLevel > 1)
				{
					leftLevel = right.left?.level ?? 0;
					rightLevel = right.right?.level ?? 0;
					if (rightLevel >= leftLevel)
					{
						right.Elevate();
						Reconstruct();
					}
					else
					{
						var pivot = right.left;
						pivot.Elevate();
						pivot.Elevate();
						pivot.left?.Reconstruct(false);
						pivot.right?.Reconstruct();
					}
				}
				else
				{
					level = Math.Max(leftLevel, rightLevel) + 1;
					if ((parent != null) && recursive)
						parent.Reconstruct();
				}
			}

			private void Elevate()
			{
				var root = parent;
				var up = root.parent;
				parent = up;
				if (up == null)
					tree.root = this;
				else if (up.left == root)
					up.left = this;
				else
					up.right = this;
				if (root.left == this)
				{
					root.left = right;
					if (right != null)
						right.parent = root;
					right = root;
					root.parent = this;
				}
				else
				{
					root.right = left;
					if (left != null)
						left.parent = root;
					left = root;
					root.parent = this;
				}
			}

			public bool IsValid()
			{
				var leftLevel = left?.level ?? 0;
				var rightLevel = right?.level ?? 0;
				if (Math.Abs(leftLevel - rightLevel) > 1)
					return false;
				var max = Math.Max(leftLevel, rightLevel);
				if (level != max + 1)
					return false;
				if ((left != null) && !left.IsValid())
					return false;
				if ((right != null) && !right.IsValid())
					return false;
				return true;
			}


			public int GetGreaterCount(T item)
			{
				var result = value.CompareTo(item);
				if (result <= 0)
					return right?.GetGreaterCount(item) ?? 0;
				var count = 1;
				count += right?.count ?? 0;
				count += left?.GetGreaterCount(item) ?? 0;
				return count;
			}

			public int GetLesserCount(T item)
			{
				var result = value.CompareTo(item);
				if (result >= 0)
					return left?.GetLesserCount(item) ?? 0;
				var count = 1;
				count += left?.count ?? 0;
				count += right?.GetLesserCount(item) ?? 0;
				return count;
			}

			public bool GetGreater(T item, out T result)
			{
				result = default;
				var cmp = value.CompareTo(item);
				if (cmp <= 0)
					return right?.GetGreater(item, out result) ?? false;
				if (left?.GetGreater(item, out result) ?? false)
					return true;
				result = value;
				return true;
			}

			public bool GetLesser(T item, out T result)
			{
				result = default;
				var cmp = value.CompareTo(item);
				if (cmp >= 0)
					return left?.GetLesser(item, out result) ?? false;
				if (right?.GetLesser(item, out result) ?? false)
					return true;
				result = value;
				return true;
			}
		}

		private Node root = null;

		public void Add(T item)
		{
			if (root != null)
				root.Add(item);
			else
				root = new Node(item, this);
		}

		public bool Remove(T item)
		{
			if (root != null)
				return root.Remove(item);
			return false;
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
			return root?.IsValid() ?? true;
		}

		public int GetGreaterCount(T item)
		{
			return root?.GetGreaterCount(item) ?? 0;
		}

		public int GetLesserCount(T item)
		{
			return root?.GetLesserCount(item) ?? 0;
		}

		public bool GetGreater(T item, out T result)
		{
			result = default;
			return root?.GetGreater(item, out result) ?? false;
		}

		public bool GetLesser(T item, out T result)
		{
			result = default;
			return root?.GetLesser(item, out result) ?? false;
		}
	}
}
