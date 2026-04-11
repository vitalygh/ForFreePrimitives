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
}
