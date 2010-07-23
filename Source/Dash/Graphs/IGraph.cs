using System.Collections.Generic;

namespace Utilities.Graphs
{
	public interface IGraph<T>
	{
		IEnumerable<T> GetChildren(T node);
	}
}
