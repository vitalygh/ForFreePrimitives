using System;
using System.Collections.Generic;
using System.Linq;

namespace ForFreePrimitives
{
    public class BinaryLifting
    {
        private int[][] up = null;
        private int[] tin = null;
        private int[] tout = null;
        private int maxShift = 0;
        private int time = 0;

        public int MaxShift { get => maxShift; }

        public BinaryLifting(int n, int root, Func<int, IEnumerable<int>> children)
        {
            maxShift = (int)Math.Log(n, 2) + 1;
            up = new int[n][];
            tin = new int[n];
            tout = new int[n];
            Build(root, root, children);
        }

        private void Build(int node, int parent, Func<int, IEnumerable<int>> children)
        {
            time += 1;
            tin[node] = time;
            up[node] = new int[maxShift + 1];
            up[node][0] = parent;
            for (var i = 1; i <= maxShift; i += 1)
                up[node][i] = up[up[node][i - 1]][i - 1];
            foreach (var child in children(node))
            {
                if (child == parent)
                    continue;
                Build(child, node, children);
            }
            time += 1;
            tout[node] = time;
        }

        private static IEnumerable<(int u, int v)> ToEdges(IList<int> parents)
        {
            for (var i = 0; i < parents.Count; i += 1)
                if (parents[i] >= 0)
                    yield return (parents[i], i);
        }

        public static List<int>[] BuildGraph(IList<int> parents, bool directed = false)
        {
            return BuildGraph(parents.Count, ToEdges(parents), directed);
        }

        public static List<int>[] BuildGraph(int n, IEnumerable<IList<int>> edges, bool directed = false)
        {
            return BuildGraph(n, edges.Select((x) => (x[0], x[1])), directed);
        }

        public static List<int>[] BuildGraph(int n, IEnumerable<(int u, int v)> edges, bool directed = false)
        {
            var graph = new List<int>[n];
            for (var i = 0; i < graph.Length; i += 1)
                graph[i] = new List<int>();
            foreach (var (u, v) in edges)
            {
                graph[u].Add(v);
                if (!directed)
                    graph[v].Add(u);
            }
            return graph;
        }

        public int GetTimeIn(int v) => tin[v];
        public int GetTimeOut(int v) => tout[v];
        public int GetParent(int v, int shift) => up[v][shift];

        public bool IsAncestor(int u, int v)
        {
            return (tin[u] <= tin[v]) && (tout[u] >= tout[v]);
        }

        public int GetLCA(int u, int v)
        {
            if (IsAncestor(u, v))
                return u;
            if (IsAncestor(v, u))
                return v;
            for (var i = maxShift; i >= 0; i -= 1)
                if (!IsAncestor(up[u][i], v))
                    u = up[u][i];
            return up[u][0];
        }

        public int GetAncestor(int node, int distance)
        {
            if (distance <= 0)
                return node;
            distance -= 1;
            var shift = 0;
            while (distance > 0)
            {
                if (shift >= maxShift)
                    return -1;
                if ((distance & 1) != 0)
                    node = up[node][shift];
                distance >>= 1;
                shift += 1;
            }
            if (up[node][0] != node)
                return up[node][0];
            return -1;
        }
    }
}
