using System.Collections.Generic;

namespace Utilities
{
	/// <summary>
	/// Provides static methods for working with types that implement the IEnumerable interface.
	/// </summary>
	public static class EnumerableUtilities
	{
		public static IEnumerable<T> Single<T>(T item)
		{
			yield return item;
		}
	}
}
