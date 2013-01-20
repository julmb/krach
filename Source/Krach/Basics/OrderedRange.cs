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

namespace Krach.Basics
{
	public struct OrderedRange<T> : IEquatable<OrderedRange<T>>
		where T : IEquatable<T>, IComparable<T>
	{
		readonly T start;
		readonly T end;

		public static OrderedRange<T> Default { get { return new OrderedRange<T>(default(T), default(T)); } }

		public T Start { get { return start; } }
		public T End { get { return end; } }
		public bool IsEmpty { get { return EqualityComparer<T>.Default.Equals(start, end); } }

		public OrderedRange(T start, T end)
		{
			if (Comparer<T>.Default.Compare(start, end) > 0) throw new ArgumentException("Parameter 'start' cannot be greater than parameter 'end'.");

			this.start = start;
			this.end = end;
		}
		public OrderedRange(T position) : this(position, position) { }

		public override bool Equals(object obj)
		{
			return obj is OrderedRange<T> && this == (OrderedRange<T>)obj;
		}
		public override int GetHashCode()
		{
			return start.GetHashCode() ^ end.GetHashCode();
		}
		public override string ToString()
		{
			return start + " - " + end;
		}
		public bool Equals(OrderedRange<T> other)
		{
			return this == other;
		}
		public bool Contains(T value)
		{
			return Comparer<T>.Default.Compare(value, start) >= 0 && Comparer<T>.Default.Compare(value, end) <= 0;
		}

		public static bool operator ==(OrderedRange<T> range1, OrderedRange<T> range2)
		{
			EqualityComparer<T> comparer = EqualityComparer<T>.Default;

			return comparer.Equals(range1.start, range2.start) && comparer.Equals(range1.end, range2.end);
		}
		public static bool operator !=(OrderedRange<T> range1, OrderedRange<T> range2)
		{
			EqualityComparer<T> comparer = EqualityComparer<T>.Default;

			return !comparer.Equals(range1.start, range2.start) || !comparer.Equals(range1.end, range2.end);
		}

		public static OrderedRange<T> Intersect(IEnumerable<OrderedRange<T>> ranges)
		{
			T start = ranges.Max(range => range.start);
			T end = ranges.Min(range => range.end);

			if (Comparer<T>.Default.Compare(start, end) > 0) return OrderedRange<T>.Default;

			return new OrderedRange<T>(start, end);
		}
		public static OrderedRange<T> Intersect(params OrderedRange<T>[] ranges)
		{
			return Intersect((IEnumerable<OrderedRange<T>>)ranges);
		}
		public static OrderedRange<T> Union(IEnumerable<OrderedRange<T>> ranges)
		{
			T start = ranges.Min(range => range.start);
			T end = ranges.Max(range => range.end);

			if (Comparer<T>.Default.Compare(start, end) > 0) return OrderedRange<T>.Default;

			return new OrderedRange<T>(start, end);
		}
		public static OrderedRange<T> Union(params OrderedRange<T>[] ranges)
		{
			return Union((IEnumerable<OrderedRange<T>>)ranges);
		}
	}
}
