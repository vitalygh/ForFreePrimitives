using System;
using System.Collections.Generic;

namespace ForFreePrimitives
{
    public class ConvexHullTrick
    {
        private class Line
        {
            public readonly long a = 0;
            public readonly long b = 0;
            public Line(long a, long b)
            {
                this.a = a;
                this.b = b;
            }
            public long Evaluate(long x)
            {
                return a * x + b;
            }
            public double IntersectionX(Line other)
            {
                return (double)(other.b - b) / (a - other.a);
            }
        }

        private readonly List<Line> lines = new List<Line>();
        private int head = 0;

        public void Update(long a, long b)
        {
            var line = new Line(a, b);
            while (lines.Count >= 2)
            {
                var last = lines[lines.Count - 1];
                var prev = lines[lines.Count - 2];
                if (prev.IntersectionX(line) > prev.IntersectionX(last))
                    break;
                lines.RemoveAt(lines.Count - 1);
            }
            lines.Add(line);
        }

        public long Query(long x)
        {
            head = Math.Min(head, lines.Count - 1);
            while ((head < lines.Count - 1) && (lines[head + 1].Evaluate(x) < lines[head].Evaluate(x)))
                head += 1;
            return lines[head].Evaluate(x);
        }

        public long QueryBinary(long x)
        {
            var start = 0;
            var end = lines.Count - 2;
            while (start <= end)
            {
                var mid = start + (end - start) / 2;
                if (lines[mid].IntersectionX(lines[mid + 1]) < x)
                    start = mid + 1;
                else
                    end = mid - 1;
            }
            return lines[start].Evaluate(x);
        }
    }

    public class LiChaoTree
    {
        private class Line
        {
            public readonly long a = 0;
            public readonly long b = 0;
            public Line(long a, long b)
            {
                this.a = a;
                this.b = b;
            }
            public long Evaluate(long x)
            {
                return a * x + b;
            }
            public double IntersectionX(Line other)
            {
                return (double)(other.b - b) / (a - other.a);
            }
        }

        private readonly Line[] tree = null;

        private readonly int minX = 0;
        private readonly int maxX = 0;

        public LiChaoTree(int minX, int maxX)
        {
            this.minX = minX;
            this.maxX = maxX;
            tree = new Line[4 * (maxX - minX + 1)];
        }

        public void Update(long a, long b)
        {
            Update(new Line(a, b), 0, minX, maxX);
        }

        public long Query(long x)
        {
            return Query(x, 0, minX, maxX);
        }

        private void Update(Line line, int index, int left, int right)
        {
            var node = tree[index];
            if (node == null)
            {
                tree[index] = line;
                return;
            }
            if ((line.Evaluate(left) >= node.Evaluate(left)) && (line.Evaluate(right) >= node.Evaluate(right)))
                return;
            var mid = left + (right - left) / 2;
            var leftBetter = line.Evaluate(left) < node.Evaluate(left);
            var midBetter = line.Evaluate(mid) < node.Evaluate(mid);
            if (midBetter)
                (line, tree[index]) = (tree[index], line);
            if (left == right)
                return;
            if (leftBetter != midBetter)
                Update(line, 2 * index + 1, left, mid);
            else
                Update(line, 2 * index + 2, mid + 1, right);
        }

        private long Query(long x, int index, int left, int right)
        {
            if ((index >= tree.Length) || (tree[index] == null))
                return long.MaxValue;
            var y = tree[index].Evaluate(x);
            if (left == right)
                return y;
            var mid = left + (right - left) / 2;
            if (x <= mid)
                return Math.Min(y, Query(x, 2 * index + 1, left, mid));
            return Math.Min(y, Query(x, 2 * index + 2, mid + 1, right));
        }
    }

    public class LiChaoDynamicTree
    {
        private class Line
        {
            public readonly long a = 0;
            public readonly long b = 0;
            public Line(long a, long b)
            {
                this.a = a;
                this.b = b;
            }
            public long Evaluate(long x)
            {
                return a * x + b;
            }
            public double IntersectionX(Line other)
            {
                return (double)(other.b - b) / (a - other.a);
            }
        }

        private class Node
        {
            public readonly long left = 0;
            public readonly long right = 0;
            public Line line = null;
            public Node leftChild = null;
            public Node rightChild = null;

            public Node(Line line, long left, long right)
            {
                this.line = line;
                this.left = left;
                this.right = right;
            }
        }

        private readonly Node root = null;

        public LiChaoDynamicTree(long minX, long maxX)
        {
            root = new Node(null, minX, maxX);
        }

        public void Update(long a, long b)
        {
            Update(root, new Line(a, b));
        }

        public long Query(long x)
        {
            return Query(root, x);
        }

        private void Update(Node node, Line line)
        {
            if (node.line == null)
            {
                node.line = line;
                return;
            }
            var left = node.left;
            var right = node.right;

            if ((line.Evaluate(left) >= node.line.Evaluate(left)) && (line.Evaluate(right) >= node.line.Evaluate(right)))
                return;

            var mid = left + (right - left) / 2;
            var leftBetter = line.Evaluate(left) < node.line.Evaluate(left);
            var midBetter = line.Evaluate(mid) < node.line.Evaluate(mid);
            if (midBetter)
                (line, node.line) = (node.line, line);
            if (left == right)
                return;
            if (leftBetter != midBetter)
            {
                if (node.leftChild == null)
                    node.leftChild = new Node(null, left, mid);
                Update(node.leftChild, line);
            }
            else
            {
                if (node.rightChild == null)
                    node.rightChild = new Node(null, mid + 1, right);
                Update(node.rightChild, line);
            }
        }

        private long Query(Node node, long x)
        {
            if (node == null)
                return long.MaxValue;
            var y = (node.line == null) ? long.MaxValue : node.line.Evaluate(x);
            var mid = node.left + (node.right - node.left) / 2;
            if (x == mid)
                return y;
            if (x < mid)
                return Math.Min(y, Query(node.leftChild, x));
            return Math.Min(y, Query(node.rightChild, x));
        }
    }
}
