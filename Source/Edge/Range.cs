using System;
using System.Collections.Generic;
using System.Linq;

namespace Edge
{
	public struct Range<T> : IEquatable<Range<T>>
		where T : IEquatable<T>, IComparable<T>
	{
		readonly T start;
		readonly T end;

		public static Range<T> Default { get { return new Range<T>(default(T), default(T)); } }

		public T Start { get { return start; } }
		public T End { get { return end; } }
		public bool IsEmpty { get { return EqualityComparer<T>.Default.Equals(start, end); } }

		public Range(T start, T end)
		{
			if (Comparer<T>.Default.Compare(start, end) > 0) throw new ArgumentException("Parameter 'start' cannot be greater than parameter 'end'.");

			this.start = start;
			this.end = end;
		}

		public override bool Equals(object obj)
		{
			return obj is Range<T> && this == (Range<T>)obj;
		}
		public override int GetHashCode()
		{
			return start.GetHashCode() ^ end.GetHashCode();
		}
		public override string ToString()
		{
			return start + " - " + end;
		}
		public bool Equals(Range<T> other)
		{
			return this == other;
		}

		public static bool operator ==(Range<T> range1, Range<T> range2)
		{
			EqualityComparer<T> comparer = EqualityComparer<T>.Default;

			return comparer.Equals(range1.start, range2.start) && comparer.Equals(range1.end, range2.end);
		}
		public static bool operator !=(Range<T> range1, Range<T> range2)
		{
			EqualityComparer<T> comparer = EqualityComparer<T>.Default;

			return !comparer.Equals(range1.start, range2.start) || !comparer.Equals(range1.end, range2.end);
		}

		public static Range<T> Intersect(IEnumerable<Range<T>> ranges)
		{
			return new Range<T>(ranges.Max(range => range.start), ranges.Min(range => range.end));
		}
		public static Range<T> Union(IEnumerable<Range<T>> ranges)
		{
			return new Range<T>(ranges.Min(range => range.start), ranges.Max(range => range.end));
		}
	}
	public static class RangeExtensions
	{
		public static double Length(this Range<double> range)
		{
			return range.End - range.Start;
		}
		public static Range<double> Inflate(this Range<double> range, double value)
		{
			return new Range<double>(range.Start - value, range.End + value);
		}
	}
}
