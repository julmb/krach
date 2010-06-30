using System;
using System.Collections.Generic;

namespace Utilities
{
	public static class CollectionExtensions
	{
		public static void Remove<TSource>(this ICollection<TSource> source, IEnumerable<TSource> items)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (items == null) throw new ArgumentNullException("items");

			foreach (TSource item in items) source.Remove(item);
		}
	}
}
