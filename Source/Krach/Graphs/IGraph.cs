using System.Collections.Generic;

namespace Krach.Graphs
{
	public interface IGraph<T>
	{
		IEnumerable<T> GetChildren(T node);
	}
}
