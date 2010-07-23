using System;
using System.Collections.Generic;
using System.Linq;
using Dash.Extensions;
using Utility.Utilities;

namespace Utilities
{
	/// <summary>
	/// Provides extension methods for working with types that implement the IEnumerable interface.
	/// </summary>
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Tests whether a collection contains all elements of a second collection.
		/// </summary>
		/// <typeparam name="TSource">The type of the collections.</typeparam>
		/// <param name="first">The first collection.</param>
		/// <param name="second">The second collection.</param>
		/// <returns>A value indicating whether the first collection contains all the elements of the second collection.</returns>
		public static bool ContainsAll<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			if (first == null) throw new ArgumentNullException("first");
			if (second == null) throw new ArgumentNullException("second");

			return second.All(item => first.Contains(item));
		}
		/// <summary>
		/// Tests whether a collection contains any of the elements of a second collection.
		/// </summary>
		/// <typeparam name="TSource">The type of the collections.</typeparam>
		/// <param name="first">The first collection.</param>
		/// <param name="second">The second collection.</param>
		/// <returns>A value indicating whether the first collection contains any of the elements of the second collection.</returns>
		public static bool ContainsAny<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			if (first == null) throw new ArgumentNullException("first");
			if (second == null) throw new ArgumentNullException("second");

			return second.Any(item => first.Contains(item));
		}
		/// <summary>
		/// Tests whether all of the items in a collection are distinct.
		/// </summary>
		/// <typeparam name="TSource">The type of the collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>A value indicating whether all of the items in the collection are distinct.</returns>
		public static bool IsDistinct<TSource>(this IEnumerable<TSource> source)
		{
			if (source == null) throw new ArgumentNullException("source");

			source = source.ToArray();

			return Enumerable.SequenceEqual(source, source.Distinct());
		}
		/// <summary>
		/// Rotates all items in the source collection to the right by a specified offset.
		/// </summary>
		/// <typeparam name="TSource">The type of the source collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="offset">The offset by which to rotate to the right.</param>
		/// <returns>The collection with all items rotated to the right by a specified offset.</returns>
		public static IEnumerable<TSource> Rotate<TSource>(this IEnumerable<TSource> source, int offset)
		{
			if (source == null) throw new ArgumentNullException("source");

			TSource[] buffer = source.ToArray();

			offset = offset.Modulo(buffer.Length);

			for (int i = 0; i < buffer.Length; i++) yield return buffer[(i - offset).Modulo(buffer.Length)];
		}
		/// <summary>
		/// Gets the index of the specified item in the source collection or -1 if the item was not found.
		/// </summa
		/// <typeparam name="TSource">The type of the source collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="item">The item of which to get the index.</param>
		/// <returns>The index of the specified item or -1 if the item was not found.</returns>
		public static int GetIndex<TSource>(this IEnumerable<TSource> source, TSource item)
		{
			if (source == null) throw new ArgumentNullException("source");

			IEqualityComparer<TSource> comparer = EqualityComparer<TSource>.Default;

			int index = 0;

			foreach (TSource currentItem in source)
			{
				if (comparer.Equals(currentItem, item)) return index;

				index++;
			}

			return -1;
		}
		/// <summary>
		/// Filters a sequence of values based on a predicate.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="predicate">A function to test each element for a condition.</param>
		/// <returns>A collection that contains elements from the input sequence that don't satisfy the condition.</returns>
		public static IEnumerable<TSource> WhereNot<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			if (source == null) throw new ArgumentNullException("source");

			foreach (TSource item in source)
				if (!predicate(item))
					yield return item;
		}
		public static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> source, TSource item)
		{
			return Enumerable.Concat(source, EnumerableUtilities.Single(item));
		}
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
		public static IEnumerable<T> Except<T>(this IEnumerable<T> source, T item)
		{
			if (source == null) throw new ArgumentNullException("source");

			return source.Except(EnumerableUtility.Single(item));
		}
		public static IEnumerable<Tuple<T, T>> GetRanges<T>(this IEnumerable<T> source)
		{
			if (source == null) throw new ArgumentNullException("source");

			IEnumerator<T> enumerator = source.GetEnumerator();

			if (!enumerator.MoveNext()) yield break;

			T last = enumerator.Current;

			while (enumerator.MoveNext()) yield return Tuple.Create(last, last = enumerator.Current);
		}
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
