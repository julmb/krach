using System;
using System.Collections.Generic;

namespace Utility.Utilities
{
	public static class EnumerableUtility
	{
		public static IEnumerable<T> Construct<T>(params IEnumerable<T>[] sources)
		{
			if (sources == null) throw new ArgumentNullException("sources");

			foreach (IEnumerable<T> source in sources)
			{
				if (source == null) throw new ArgumentException("sources");

				foreach (T item in source)
					yield return item;
			}
		}
		public static IEnumerable<T> Consume<T>(Func<T> getItem)
		{
			while (true) yield return getItem();
		}
		public static IEnumerable<T> Single<T>(T item)
		{
			yield return item;
		}
	}
}
