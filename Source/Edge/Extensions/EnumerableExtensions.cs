using System;
using System.Collections.Generic;
using System.Linq;
using Utility.Utilities;

namespace Utility.Extensions
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<T> Separate<T>(this IEnumerable<T> source, T separator)
		{
			if (source == null) throw new ArgumentNullException("source");

			bool first = true;

			foreach (T item in source)
			{
				if (first) first = false;
				else yield return separator;

				yield return item;
			}
		}
		public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, T item)
		{
			if (source == null) throw new ArgumentNullException("source");

			return source.Concat(EnumerableUtility.Single(item));
		}
		public static IEnumerable<T> Except<T>(this IEnumerable<T> source, T item)
		{
			if (source == null) throw new ArgumentNullException("source");

			return source.Except(EnumerableUtility.Single(item));
		}
		//public static IEnumerable<Range<T>> GetRanges<T>(this IEnumerable<T> source)
		//{
		//    if (source == null) throw new ArgumentNullException("source");

		//    IEnumerator<T> enumerator = source.GetEnumerator();

		//    if (!enumerator.MoveNext()) yield break;

		//    T last = enumerator.Current;

		//    while (enumerator.MoveNext()) yield return new Range<T>(last, last = enumerator.Current);
		//}
		public static IEnumerable<string> ToStrings<T>(this IEnumerable<T> source)
		{
			if (source == null) throw new ArgumentNullException("source");

			foreach (T item in source) yield return item == null ? "<null>" : item.ToString();
		}
		public static string AggregateString<T>(this IEnumerable<T> source)
		{
			if (source == null) throw new ArgumentNullException("source");

			return source.Aggregate(string.Empty, (seed, current) => seed + current);
		}
		public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source, int count)
		{
			if (source == null) throw new ArgumentNullException("source");

			Queue<T> queue = new Queue<T>();

			foreach (T item in source)
			{
				queue.Enqueue(item);

				if (queue.Count > count) yield return queue.Dequeue();
			}
		}
		public static IEnumerable<T> TakeLast<T>(this IEnumerable<T> source, int count)
		{
			if (source == null) throw new ArgumentNullException("source");

			Queue<T> queue = new Queue<T>();

			foreach (T item in source)
			{
				queue.Enqueue(item);

				if (queue.Count > count) queue.Dequeue();
			}

			return queue;
		}
	}
}
