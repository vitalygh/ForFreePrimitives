using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ForFreePrimitives;

namespace ForFreePrimitivesTests
{
	[TestClass]
	public class BinaryLiftingTests
	{
		private static Random random = new Random();

		[TestMethod]
		public void Tests()
		{
			ProcessDefinedTestcases();
			ProcessRandomTestcases();
		}

		private static readonly (int root, (int, int)[] edges, (int, int)[] query, int[] lca)[] testcases = new[]
		{
			(0, new[] { (0,1),(1,3),(1,8),(0,2),(5,7),(4,5),(2,4),(7,9),(3,6) },
				new[] { (8,8), (7,2), (9,6) },
				new[] { 8, 2, 0 } ),
			(0, new[] { (10,15),(72,84),(4,19),(25,29),(51,81),(18,51),(57,72),(29,40),(51,59),(0,3),(3,25),(11,75),(15,20),(34,79),
				(22,64),(49,63),(7,18),(43,48),(5,6),(34,83),(34,54),(16,61),(16,46),(34,39),(15,38),(34,57),(41,88),(43,53),(2,5),
				(47,76),(52,62),(12,67),(31,43),(20,27),(7,14),(62,87),(24,74),(44,82),(18,42),(35,52),(0,22),(74,77),(0,1),(8,21),
				(7,68),(8,60),(16,86),(6,32),(4,17),(21,28),(33,58),(28,47),(23,78),(31,34),(14,66),(27,80),(27,30),(27,31),(15,50),
				(59,85),(0,8),(32,70),(30,65),(16,71),(2,11),(22,69),(22,41),(1,16),(8,35),(17,24),(8,9),(37,49),(2,36),(28,33),
				(22,37),(23,73),(8,23),(27,56),(17,55),(19,26),(38,44),(1,4),(10,13),(1,2),(68,89),(4,7),(4,12),(2,10),(11,45) },
				new[] { (86,86), (86,10), (47,26), (62,49), (72,13), (38,36), (46,17), (72,36), (36,11), (24,61), (51,21) },
				new [] { 86, 1, 0, 0, 10, 2, 1, 2, 2, 1, 0 } ),
		};

		private void ProcessDefinedTestcases()
		{
			foreach (var (root, edges, query, lca) in testcases)
				Validate(root, edges, query, lca);
		}

		private (int, int)[] MakeTreeEdges(int n)
		{
			var nodes = new int[n];
			for (var i = 0; i < nodes.Length; i += 1)
				nodes[i] = i;
			for (var i = 0; i < nodes.Length; i += 1)
			{
				var j = random.Next(i, nodes.Length);
				(nodes[i], nodes[j]) = (nodes[j], nodes[i]);
			}			
			var rooted = new List<int>();
			rooted.Add(nodes[0]);
			var edges = new (int, int)[nodes.Length - 1];
			for (var i = 1; i < nodes.Length; i += 1)
			{
				var index = random.Next(0, rooted.Count);
				edges[i - 1] = (nodes[i], rooted[index]);
				rooted.Add(nodes[i]);
			}
			return edges;
		}

		private List<int>[] MakeGraph((int, int)[] edges)
		{
			var graph = new List<int>[edges.Length + 1];
			for (var i = 0; i < graph.Length; i += 1)
				graph[i] = new List<int>();
			foreach (var (u, v) in edges)
			{
				graph[u].Add(v);
				graph[v].Add(u);
			}
			return graph;
		}

		private (List<int> children, int parent)[] MakeTree(List<int>[] graph, int root)
		{
			var tree = new (List<int> children, int parent)[graph.Length];
			var q = new Queue<(int, int)>();
			q.Enqueue((root, -1));
			while (q.Count > 0)
			{
				var (n, p) = q.Dequeue();
				tree[n] = (new List<int>(), p);
				foreach (var next in graph[n])
				{
					if (next == p)
						continue;
					tree[n].children.Add(next);
					q.Enqueue((next, n));
				}
			}
			return tree;
		}

		private int GoDown(List<int> nodes, int parent)
		{
			if (nodes.Count < 2)
				return -1;
			var index = random.Next(0, nodes.Count);
			if (nodes[index] != parent)
				return nodes[index];
			return index == 0 ? nodes[index + 1] : nodes[index - 1];
		}

		private (int, int) GoDown(List<int>[] graph, int node, int parent, int depth)
		{
			for (var i = 0; i < depth; i += 1)
			{
				var next = GoDown(graph[node], parent);
				if (next < 0)
					break;
				parent = node;
				node = next;
			}
			return (node, parent);
		}

		private bool ValidateTree(List<int>[] graph, int root = 0)
		{
			var visited = new BitArray(graph.Length);
			var q = new Queue<(int, int)>();
			q.Enqueue((root, root));
			var count = 0;
			while (q.Count > 0)
			{
				var (n, p) = q.Dequeue();
				if (visited[n])
					return false;
				count += 1;
				visited[n] = true;
				foreach (var next in graph[n])
				{
					if (next == p)
						continue;
					q.Enqueue((next, n));
				}
			}
			return count == graph.Length;
		}

		private void ProcessRandomTestcases()
		{
			var count = 20;
			var treeSize = 1000;
			var qCount = 100;
			var maxDepth = 10;
			for (var i = 0; i < count; i += 1)
			{
				var edges = MakeTreeEdges(treeSize);
				var graph = MakeGraph(edges);
				var root = random.Next(0, graph.Length);
				var tree = MakeTree(graph, root);
				var queries = new (int, int)[qCount];
				var lcas = new int[queries.Length];
				for (var j = 0; j < queries.Length; j += 1)
				{
					var lca = random.Next(0, tree.Length);
					var u = lca;
					var v = lca;
					var c = tree[lca].children.Count;
					if (c > 1)
					{
						var ui = random.Next(0, c - 1);
						var vi = random.Next(ui + 1, c);
						(u, _) = GoDown(graph, tree[lca].children[ui], lca, random.Next(0, maxDepth));
						(v, _) = GoDown(graph, tree[lca].children[vi], lca, random.Next(0, maxDepth));
					}
					else if (c > 0)
						(v, _) = GoDown(graph, tree[lca].children[0], lca, random.Next(0, maxDepth));
					queries[j] = (u, v);
					lcas[j] = lca;
				}
				Validate(root, edges, graph, queries, lcas);
			}
		}

		private string DumpEdges((int, int)[] edges)
		{
			var sb = new StringBuilder();
			foreach (var (u, v) in edges)
			{
				if (sb.Length > 0)
					sb.Append(",");
				sb.Append($"({u},{v})");
			}
			return sb.ToString();
		}

		private string DumpTestcase(int root, (int, int)[] edges, int u, int v, int lca)
		{
			return $"({root}, new[] {{{DumpEdges(edges)}}}, new[] {{ ({u}, {v}) }}, new[] {{ {lca} }})";
		}

		private void Validate(int root, (int, int)[] edges, (int, int)[] query, int[] lca)
		{
			Validate(root, edges, MakeGraph(edges), query, lca);
		}

		private void Validate(int root, (int, int)[] edges, List<int>[] graph, (int, int)[] query, int[] lca)
		{
			Assert.IsTrue(ValidateTree(graph, root));
			var binaryLifting = new BinaryLifting(graph.Length, root, (x) => graph[x]);
			for (var i = 0; i < query.Length; i += 1)
			{
				var (u, v) = query[i];
				var blLCA = binaryLifting.GetLCA(u, v);
				if (blLCA != lca[i])
					Assert.Fail($"{DumpTestcase(root, edges, u, v, lca[i])} {lca[i]} != {blLCA}");
				if (!binaryLifting.IsAncestor(lca[i], u))
					Assert.Fail($"{DumpTestcase(root, edges, u, v, lca[i])} IsAncestor({lca[i]}, {u}) == false");
				if (!binaryLifting.IsAncestor(lca[i], v))
					Assert.Fail($"{DumpTestcase(root, edges, u, v, lca[i])} IsAncestor({lca[i]}, {v}) == false");
			}
		}
	}
}
