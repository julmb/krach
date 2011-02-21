// Copyright © Julian Brunner 2009 - 2011

// This file is part of Stream Visualizer (streamvis).
// 
// Stream Visualizer is free software: you can redistribute it and/or modify it
// under the terms of the GNU General Public License as published by the Free
// Software Foundation, either version 3 of the License, or (at your option) any
// later version.
// 
// Stream Visualizer is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more
// details.
// 
// You should have received a copy of the GNU General Public License along with
// Stream Visualizer. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;

namespace Krach.Graphs
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