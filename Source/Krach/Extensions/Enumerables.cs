// Copyright © Julian Brunner 2010 - 2011

// This file is part of Krach.
//
// Krach is free software: you can redistribute it and/or modify it under the
// terms of the GNU Lesser General Public License as published by the Free
// Software Foundation, either version 3 of the License, or (at your option) any
// later version.
//
// Krach is distributed in the hope that it will be useful, but WITHOUT ANY
// WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
// A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with
// Krach. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Basics;
using System.Security.Cryptography;

namespace Krach.Extensions
{
	public static class Enumerables
	{
		public static bool ContainsAll<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			if (first == null) throw new ArgumentNullException("first");
			if (second == null) throw new ArgumentNullException("second");

			return second.All(item => first.Contains(item));
		}
		public static bool ContainsAny<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
		{
			if (first == null) throw new ArgumentNullException("first");
			if (second == null) throw new ArgumentNullException("second");

			return second.Any(item => first.Contains(item));
		}
		public static bool IsDistinct<TSource>(this IEnumerable<TSource> source)
		{
			if (source == null) throw new ArgumentNullException("source");

			source = source.ToArray();

			return Enumerable.SequenceEqual(source, source.Distinct());
		}
		public static IEnumerable<TSource> NonDistinct<TSource>(this IEnumerable<TSource> source)
		{
			if (source == null) throw new ArgumentNullException("source");

			return
			(
				from distinctItem in source.Distinct()
				let count = source.Count(item => EqualityComparer<TSource>.Default.Equals(distinctItem, item))
				where count > 1
				select distinctItem
			);
		}
		public static IEnumerable<Tuple<TSource, TSource>> SequenceDifference<TSource>(IEnumerable<TSource> source1, IEnumerable<TSource> source2)
		{
			if (source1 == null) throw new ArgumentNullException("source1");
			if (source2 == null) throw new ArgumentNullException("source2");

			foreach (Tuple<TSource, TSource> item in Enumerable.Zip(source1, source2, Tuple.Create))
				if (!EqualityComparer<TSource>.Default.Equals(item.Item1, item.Item2))
					yield return item;
		}
		public static int GetIndex<TSource>(this IEnumerable<TSource> source, TSource item)
		{
			return GetIndex(source, item, EqualityComparer<TSource>.Default.Equals);
		}
		public static int GetIndex<TSource>(this IEnumerable<TSource> source, TSource item, Func<TSource, TSource, bool> areEqual)
		{
			if (source == null) throw new ArgumentNullException("source");

			int index = 0;

			foreach (TSource currentItem in source)
			{
				if (areEqual(currentItem, item)) return index;

				index++;
			}

			return -1;
		}
		public static IEnumerable<TSource> Rotate<TSource>(this IEnumerable<TSource> source, int offset)
		{
			if (source == null) throw new ArgumentNullException("source");

			TSource[] buffer = source.ToArray();

			offset = offset.Modulo(buffer.Length);

			for (int i = 0; i < buffer.Length; i++) yield return buffer[(i - offset).Modulo(buffer.Length)];
		}
		public static IEnumerable<TSource> Append<TSource>(this IEnumerable<TSource> source, TSource item)
		{
			if (source == null) throw new ArgumentNullException("source");

			return Enumerable.Concat(source, Create(item));
		}
		public static IEnumerable<TSource> SkipLast<TSource>(this IEnumerable<TSource> source, int count)
		{
			if (source == null) throw new ArgumentNullException("source");

			Queue<TSource> queue = new Queue<TSource>();

			foreach (TSource item in source)
			{
				queue.Enqueue(item);

				if (queue.Count > count) yield return queue.Dequeue();
			}
		}
		public static IEnumerable<TSource> TakeLast<TSource>(this IEnumerable<TSource> source, int count)
		{
			if (source == null) throw new ArgumentNullException("source");

			Queue<TSource> queue = new Queue<TSource>();

			foreach (TSource item in source)
			{
				queue.Enqueue(item);

				if (queue.Count > count) queue.Dequeue();
			}

			return queue;
		}
		public static IEnumerable<TSource> GetRange<TSource>(this IEnumerable<TSource> source, OrderedRange<int> range)
		{
			if (source == null) throw new ArgumentNullException("source");
			if (range.Start < 0 || range.Start > source.Count()) throw new ArgumentOutOfRangeException("startIndex");
			if (range.End < 0 || range.End > source.Count()) throw new ArgumentOutOfRangeException("endIndex");

			return source.Skip(range.Start).Take(range.Length());
		}
		public static IEnumerable<TSource> GetRange<TSource>(this IEnumerable<TSource> source, int startIndex, int endIndex)
		{
			return source.GetRange(new OrderedRange<int>(startIndex, endIndex));
		}
		public static IEnumerable<TSource> Separate<TSource>(this IEnumerable<TSource> source, TSource separator)
		{
			if (source == null) throw new ArgumentNullException("source");

			bool first = true;

			foreach (TSource item in source)
			{
				if (first) first = false;
				else yield return separator;

				yield return item;
			}
		}
		public static IEnumerable<Tuple<TSource, TSource>> GetRanges<TSource>(this IEnumerable<TSource> source)
		{
			if (source == null) throw new ArgumentNullException("source");

			IEnumerator<TSource> enumerator = source.GetEnumerator();

			if (!enumerator.MoveNext()) yield break;

			TSource last = enumerator.Current;

			while (enumerator.MoveNext()) yield return Tuple.Create(last, last = enumerator.Current);
		}
		public static IEnumerable<TSource> WhereNot<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
		{
			return source.Where(item => !predicate(item));
		}
		public static IEnumerable<string> ToStrings<TSource>(this IEnumerable<TSource> source)
		{
			if (source == null) throw new ArgumentNullException("source");

			foreach (TSource item in source) yield return item == null ? "<null>" : item.ToString();
		}
		public static string AggregateString<TSource>(this IEnumerable<TSource> source)
		{
			if (source == null) throw new ArgumentNullException("source");

			return source.Aggregate(string.Empty, (seed, current) => seed + current);
		}
		public static IEnumerable<TSource> Concatenate<TSource>(IEnumerable<IEnumerable<TSource>> sources)
		{
			if (sources == null) throw new ArgumentNullException("sources");

			foreach (IEnumerable<TSource> source in sources)
			{
				if (source == null) throw new ArgumentException("sources");

				foreach (TSource item in source)
					yield return item;
			}
		}
		public static IEnumerable<TSource> Concatenate<TSource>(params IEnumerable<TSource>[] sources)
		{
			return Concatenate((IEnumerable<IEnumerable<TSource>>)sources);
		}
		public static IEnumerable<TSource> Union<TSource>(params IEnumerable<TSource>[] sources)
		{
			if (sources == null) throw new ArgumentNullException("sources");

			return sources.Aggregate(Enumerable.Empty<TSource>(), Enumerable.Union);
		}
		public static IEnumerable<TSource> Intersect<TSource>(params IEnumerable<TSource>[] sources)
		{
			if (sources == null) throw new ArgumentNullException("sources");

			return sources.Aggregate(Enumerable.Empty<TSource>(), Enumerable.Intersect);
		}
		public static IEnumerable<IEnumerable<TSource>> PowerSet<TSource>(this IEnumerable<TSource> source)
		{
			if (source == null) throw new ArgumentNullException("source");

			if (source.Any())
			{
				IEnumerable<TSource> head = source.Take(1);
				IEnumerable<TSource> tail = source.Skip(1);

				foreach (IEnumerable<TSource> subset in PowerSet(tail))
				{
					yield return subset;
					yield return Enumerable.Concat(head, subset).ToArray();
				}
			}
			else yield return Enumerable.Empty<TSource>();
		}
		public static IEnumerable<IEnumerable<TSource>> Flip<TSource>(this IEnumerable<IEnumerable<TSource>> source)
		{
			if (source == null) throw new ArgumentNullException("source");

			if (!source.Any()) yield break;

			IEnumerable<IEnumerator<TSource>> enumerators =
			(
				from row in source
				select row.GetEnumerator()
			)
			.ToArray();

			while (enumerators.All(enumerator => enumerator.MoveNext()))
				yield return
				(
					from enumerator in enumerators
					select enumerator.Current
				)
				.ToArray();
		}
		public static IEnumerable<TSource> Create<TSource>(params TSource[] items)
		{
			return items;
		}
		public static IEnumerable<TSource> Consume<TSource>(Func<TSource> getItem)
		{
			while (true) yield return getItem();
		}
		public static int GetSequenceHashCode<TSource>(IEnumerable<TSource> source)
		{
			if (source == null) throw new ArgumentNullException("source");

			const int rotationLength = 7;

			uint result = 0;

			foreach (TSource item in source)
			{
				result = (result << (0 + rotationLength) | (result >> (32 - rotationLength)));
				result ^= item == null ? (uint)0.GetHashCode() : (uint)item.GetHashCode();
			}

			return (int)result;
		}
		public static IEnumerable<TResult> Zip<TSource1, TResult>(this IEnumerable<TSource1> source1, Func<TSource1, TResult> resultSelector)
		{
			IEnumerator<TSource1> enumerator1 = source1.GetEnumerator();

			while (enumerator1.MoveNext()) yield return resultSelector(enumerator1.Current);
		}
		public static IEnumerable<TResult> Zip<TSource1, TSource2, TResult>(this IEnumerable<TSource1> source1, IEnumerable<TSource2> source2, Func<TSource1, TSource2, TResult> resultSelector)
		{
			IEnumerator<TSource1> enumerator1 = source1.GetEnumerator();
			IEnumerator<TSource2> enumerator2 = source2.GetEnumerator();

			while (enumerator1.MoveNext() && enumerator2.MoveNext()) yield return resultSelector(enumerator1.Current, enumerator2.Current);
		}
		public static IEnumerable<TResult> Zip<TSource1, TSource2, TSource3, TResult>(this IEnumerable<TSource1> source1, IEnumerable<TSource2> source2, IEnumerable<TSource3> source3, Func<TSource1, TSource2, TSource3, TResult> resultSelector)
		{
			IEnumerator<TSource1> enumerator1 = source1.GetEnumerator();
			IEnumerator<TSource2> enumerator2 = source2.GetEnumerator();
			IEnumerator<TSource3> enumerator3 = source3.GetEnumerator();

			while (enumerator1.MoveNext() && enumerator2.MoveNext() && enumerator3.MoveNext()) yield return resultSelector(enumerator1.Current, enumerator2.Current, enumerator3.Current);
		}
	}
}
