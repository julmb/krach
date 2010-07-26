using System.Collections.Generic;

namespace Dash.Graphs
{
	public interface IGraph<T>
	{
		IEnumerable<T> GetChildren(T node);
	}
}
