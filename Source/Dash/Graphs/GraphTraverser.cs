using System;
using System.Collections.Generic;

namespace Utilities.Graphs
{
	public class GraphTraverser<T>
	{
		readonly IGraph<T> graph;

		public GraphTraverser(IGraph<T> graph)
		{
			if (graph == null) throw new ArgumentNullException("graph");

			this.graph = graph;
		}

		public IEnumerable<T> Traverse(IEnumerable<T> nodes)
		{
			List<T> result = new List<T>();

			foreach (T node in nodes) Traverse(result, node);

			return result;
		}

		void Traverse(List<T> result, T node)
		{
			result.Add(node);

			foreach (T child in graph.GetChildren(node))
				if (!result.Contains(child))
					Traverse(result, child);
		}
	}
}