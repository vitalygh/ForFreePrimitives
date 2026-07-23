using System;
using System.Collections.Generic;

namespace ForFreePrimitives
{
    public class BitTrie
    {
        private class Node
        {
            public Node[] children = new Node[2];
            public int count = 0;
        }

        private readonly int maxLength = 8 * sizeof(long);
        private readonly Node root = new Node();

        public int Count { get => root.count; }

        public BitTrie()
        {
        }

        public BitTrie(int maxLength)
        {
            this.maxLength = maxLength;
        }

        private static int GetBit(long num, int index)
        {
            var bit = 1L << index;
            return ((num & bit) == bit) ? 1 : 0;
        }

        public void Add(long num)
        {
            var node = root;
            node.count += 1;
            for (var i = maxLength - 1; i >= 0; i -= 1)
            {
                var bit = GetBit(num, i);
                if (node.children[bit] == null)
                    node.children[bit] = new Node();
                node = node.children[bit];
                node.count += 1;
            }
        }

        public bool Remove(long num)
        {
            if (!Contains(num))
                return false;
            var node = root;
            node.count -= 1;
            for (var i = maxLength - 1; i >= 0; i -= 1)
            {
                var bit = GetBit(num, i);
                node.children[bit].count -= 1;
                if (node.children[bit].count <= 0)
                {
                    node.children[bit] = null;
                    return true;
                }
                node = node.children[bit];
            }
            return true;
        }

        public bool Contains(long num)
        {
            var node = root;
            for (var i = maxLength - 1; i >= 0; i -= 1)
            {
                var bit = GetBit(num, i);
                if (node.children[bit] == null)
                    return false;
                node = node.children[bit];
            }
            return true;
        }

        public long GetMaxXor(long num)
        {
            if (Count <= 0)
                return num;
            var maxVal = 0L;
            var node = root;
            for (var i = maxLength - 1; i >= 0; i -= 1)
            {
                var bit = GetBit(num, i);
                var target = bit ^ 1;
                if (node.children[target] != null)
                {
                    maxVal += 1L << i;
                    node = node.children[target];
                    continue;
                }
                node = node.children[target ^ 1];
            }
            return maxVal;
        }

        public int GetGreaterXorCount(int n, int xorWith = 0)
        {
            var count = 0;
            var node = root;
            for (var i = maxLength - 1; i >= 0; i -= 1)
            {
                var bit = GetBit(n, i);
                var xbit = bit ^ GetBit(xorWith, i);
                if (bit == 0)
                    count += node.children[xbit ^ 1]?.count ?? 0;
                if (node.children[xbit] == null)
                    break;
                node = node.children[xbit];
            }
            return count;
        }
    }

    public class ACTrie<T>
    {
        private readonly Node root = null;

        private class Node
        {
            public readonly Node[] children = null;
            public Node failure = null;
            public Node dictionary = null;
            public List<T> data = new List<T>();

            public Node(int dictionarySize)
            {
                children = new Node[dictionarySize];
            }
        }

        public ACTrie(int dictionarySize)
        {
            root = new Node(dictionarySize);
        }

        public void Add(Func<int, int> word, int length, T data)
        {
            if (length <= 0)
                return;
            if (root.failure != null)
                throw new InvalidOperationException("Can't add words after building links");
            var node = root;
            for (var i = 0; i < length; i += 1)
            {
                var index = word(i);
                if (node.children[index] == null)
                    node.children[index] = new Node(root.children.Length);
                node = node.children[index];
            }
            node.data.Add(data);
        }

        public void Build()
        {
            var q = new Queue<Node>();
            root.failure = root;
            root.dictionary = root;
            q.Enqueue(root);
            while (q.Count > 0)
            {
                var node = q.Dequeue();
                for (var i = 0; i < node.children.Length; i += 1)
                {
                    var next = node.children[i];
                    if (next == null)
                        continue;
                    var f = node.failure;
                    while ((f != root) && (f.children[i] == null))
                        f = f.failure;
                    if ((node != root) && (f.children[i] != null))
                        next.failure = f.children[i];
                    else
                        next.failure = f;
                    if (next.failure.data.Count > 0)
                        next.dictionary = next.failure;
                    else
                        next.dictionary = next.failure.dictionary;
                    q.Enqueue(next);
                }
            }
        }

        public void Find(Func<int, int> word, int length, Action<int, T> found)
        {
            if (root.failure == null)
                Build();
            var node = root;
            for (var i = 0; i < length; i += 1)
            {
                var index = word(i);
                if ((index < 0) || (index >= root.children.Length))
				{
                    node = root;
                    continue;
				}
                while ((node != root) && (node.children[index] == null))
                    node = node.failure;
                node = node.children[index] ?? root;
                var dataNode = node.data.Count > 0 ? node : node.dictionary;
                while (dataNode != root)
                {
                    foreach (var data in dataNode.data)
                        found.Invoke(i, data);
                    dataNode = dataNode.dictionary;
                }
            }
        }
    }
}
