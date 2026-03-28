using System;
using System.Collections.Generic;

namespace ForFreePrimitives
{
	public class UkkSuffixTree
	{
		private class Node
		{
			private UkkSuffixTree tree = null;
			private int endIndex = -1;

			public int Start = 0;
			public int End { get => tree?.ends[endIndex] ?? -1; }

			public Node[] Children = null;
			public Node SuffixLink = null;
			public int SuffixIndex = -1;

			public Node(int dictionarySize, UkkSuffixTree tree = null, int start = 0, int endIndex = -1)
			{
				this.tree = tree;
				this.endIndex = endIndex;
				Start = start;
				SuffixLink = tree?.root;
				Children = new Node[dictionarySize];
			}

			public int Length { get => End - Start + 1; }
		}

		private List<int> ends = new List<int>() { 0 };
		private Node root = null;
		private Node lastNewNode = null;
		private Node activeNode = null;
		private int activeEdge = -1;
		private int activeLength = 0;
		private int reminder = 0;

		private static readonly int leafEndIndex = 0;

		public UkkSuffixTree(Func<int, char> data, int length)
		{
			var sentinel = -1;
			var dictionarySize = 1;
			if (length > 0)
			{
				var min = (int)data(0);
				var max = (int)data(0);
				for (var i = 1; i < length; i += 1)
				{
					var d = data(i);
					if (d < min)
						min = d;
					if (d > max)
						max = d;
				}
				min -= 1;
				dictionarySize = max - min + 1;
				sentinel = min;
			}
			BuildTree(i =>
			{
				if (i == length)
					return 0;
				return data(i) - sentinel;
			}, length + 1, dictionarySize);
		}

		private bool WalkDown(Node node)
		{
			var length = node?.Length ?? 0;
			if (activeLength >= length)
			{
				activeEdge += length;
				activeLength -= length;
				activeNode = node;
				return true;
			}
			return false;
		}

		private void Extend(int pos, Func<int, int> data, int dictionarySize)
		{
			ends[leafEndIndex] = pos;
			reminder += 1;
			lastNewNode = null;
			while (reminder > 0)
			{
				if (activeLength == 0)
					activeEdge = pos;
				var target = data(activeEdge);
				if (activeNode.Children[target] == null)
				{
					activeNode.Children[target] = new Node(dictionarySize, this, pos, leafEndIndex);
					if (lastNewNode != null)
					{
						lastNewNode.SuffixLink = activeNode;
						lastNewNode = null;
					}
				}
				else
				{
					var child = activeNode.Children[target];
					if (WalkDown(child))
						continue;
					if (data(child.Start + activeLength).Equals(data(pos)))
					{
						if ((lastNewNode != null) && (activeNode != root))
						{
							lastNewNode.SuffixLink = activeNode;
							lastNewNode = null;
						}
						activeLength += 1;
						break;
					}
					var splitEndIndex = ends.Count;
					ends.Add(child.Start + activeLength - 1);
					var splitNode = new Node(dictionarySize, this, child.Start, splitEndIndex);
					activeNode.Children[data(activeEdge)] = splitNode;
					splitNode.Children[data(pos)] = new Node(dictionarySize, this, pos, leafEndIndex);
					child.Start += activeLength;
					splitNode.Children[data(child.Start)] = child;
					if (lastNewNode != null)
						lastNewNode.SuffixLink = splitNode;
					lastNewNode = splitNode;
				}
				reminder -= 1;
				if (activeNode != root)
					activeNode = activeNode.SuffixLink;
				else if (activeLength > 0)
				{
					activeLength -= 1;
					activeEdge = pos - reminder + 1;
				}
			}
		}

		private void SetSuffixIndexes(Node node, int index, int length)
		{
			if (node == null)
				return;
			var isLeaf = true;
			foreach (var child in node.Children)
				if (child != null)
				{
					SetSuffixIndexes(child, index + child.Length, length);
					isLeaf = false;
				}
			if (isLeaf)
				node.SuffixIndex = length - index;
		}

		private void BuildTree(Func<int, int> data, int length, int dictionarySize)
		{
			root = new Node(dictionarySize);
			activeNode = root;
			for (var i = 0; i < length; i += 1)
				Extend(i, data, dictionarySize);
			SetSuffixIndexes(root, 0, length);
		}

		private void BuildSuffixArray(Node node, int[] arr, ref int index)
		{
			if (node == null)
				return;
			if (node.SuffixIndex < 0)
			{
				foreach (var child in node.Children)
					if (child != null)
						BuildSuffixArray(child, arr, ref index);
			}
			else if (node.SuffixIndex < arr.Length)
			{
				arr[index] = node.SuffixIndex;
				index += 1;
			}
		}

		public void BuildSuffixArray(int[] arr)
		{
			var index = 0;
			BuildSuffixArray(root, arr, ref index);
		}
	}
}
