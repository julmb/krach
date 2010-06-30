using System;
using System.Collections.Generic;
using System.Linq;

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
		/// Selects all items from the beginning of the collection to the specified position.
		/// </summary>
		/// <typeparam name="TSource">The type of the source collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="end">The position at which to stop taking items.</param>
		/// <returns>All items from the beginning of the collection to the specified end.</returns>
		public static IEnumerable<TSource> RangeFromStart<TSource>(this IEnumerable<TSource> source, int end)
		{
			if (source == null) throw new ArgumentNullException("source");

			int index = 0;
			foreach (TSource item in source)
			{
				if (index > end) break;
				yield return item;
				index++;
			}
		}
		/// <summary>
		/// Selects all items from the specified position to the end of the collection.
		/// </summary>
		/// <typeparam name="TSource">The type of the source collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="start">The position at which to start taking items.</param>
		/// <returns>All items from the specified position to the end of the collection.</returns>
		public static IEnumerable<TSource> RangeToEnd<TSource>(this IEnumerable<TSource> source, int start)
		{
			if (source == null) throw new ArgumentNullException("source");

			int index = 0;
			bool started = false;
			foreach (TSource item in source)
			{
				if (index == start) started = true;
				if (started) yield return item;
				index++;
			}
		}
		/// <summary>
		/// Selects all items from the beginning of the collection to the specified item.
		/// </summary>
		/// <typeparam name="TSource">The type of the source collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="endItem">The item at which to stop taking items.</param>
		/// <returns>All items from the beginning of the collection to the specified item.</returns>
		public static IEnumerable<TSource> RangeFromStart<TSource>(this IEnumerable<TSource> source, TSource endItem)
		{
			if (source == null) throw new ArgumentNullException("source");

			IEqualityComparer<TSource> equalityComparer = EqualityComparer<TSource>.Default;
			int index = 0;
			foreach (TSource item in source)
			{
				if (equalityComparer.Equals(item, endItem)) break;
				yield return item;
				index++;
			}
		}
		/// <summary>
		/// Selects all items from the specified item to the end of the collection.
		/// </summary>
		/// <typeparam name="TSource">The type of the source collection.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <param name="startItem">The item at which to start taking items.</param>
		/// <returns>All items from the specified item to the end of the collection.</returns>
		public static IEnumerable<TSource> RangeToEnd<TSource>(this IEnumerable<TSource> source, TSource startItem)
		{
			if (source == null) throw new ArgumentNullException("source");

			IEqualityComparer<TSource> equalityComparer = EqualityComparer<TSource>.Default;
			int index = 0;
			bool started = false;
			foreach (TSource item in source)
			{
				if (equalityComparer.Equals(item, startItem)) started = true;
				if (started) yield return item;
				index++;
			}
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

			offset %= buffer.Length;

			for (int i = 0; i < buffer.Length; i++) yield return buffer[(i - offset + buffer.Length) % buffer.Length];
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
		/// <summary>
		/// Filters a sequence of values based on a type.
		/// </summary>
		/// <typeparam name="TSource">The type of the elements of source.</typeparam>
		/// <typeparam name="TType">The type to filter with.</typeparam>
		/// <param name="source">The source collection.</param>
		/// <returns>A collection that contains elements from the input sequence that don't have the given type.</returns>
		public static IEnumerable<TSource> NotOfType<TSource, TType>(this IEnumerable<TSource> source)
		{
			if (source == null) throw new ArgumentNullException("source");

			foreach (TSource item in source)
				if (!(item is TType))
					yield return item;
		}
		public static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> source, TSource item)
		{
			return Enumerable.Concat(source, EnumerableUtilities.Single(item));
		}
		public static IEnumerable<T> Randomize<T>(this IEnumerable<T> source, Random random)
		{
			List<T> remainingItems = new List<T>(source);
			List<T> result = new List<T>();

			while (remainingItems.Any())
			{
				int index = random.Next(remainingItems.Count);
				T item = remainingItems[index];
				remainingItems.RemoveAt(index);
				result.Add(item);
			}

			return result;
		}
	}
}
