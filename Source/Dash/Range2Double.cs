using System;
using System.Collections.Generic;
using System.Linq;

namespace Edge
{
	public struct Range2Double : IEquatable<Range2Double>
	{
		readonly Range<double> range1;
		readonly Range<double> range2;

		public static Range2Double Empty { get { return new Range2Double(Range<double>.Default, Range<double>.Default); } }

		public Range<double> Range1 { get { return range1; } }
		public Range<double> Range2 { get { return range2; } }
		public double Start1 { get { return range1.Start; } }
		public double End1 { get { return range1.End; } }
		public double Start2 { get { return range2.Start; } }
		public double End2 { get { return range2.End; } }
		public Vector2Double Size { get { return new Vector2Double(range1.Length(), range2.Length()); } }
		public double Volume { get { return Size.X * Size.Y; } }
		public bool IsEmpty { get { return Size.X <= 0 || Size.Y <= 0; } }

		public Range2Double(Range<double> range1, Range<double> range2)
		{
			this.range1 = range1;
			this.range2 = range2;
		}
		public Range2Double(double start1, double end1, double start2, double end2)
		{
			this.range1 = new Range<double>(start1, end1);
			this.range2 = new Range<double>(start2, end2);
		}

		public override bool Equals(object obj)
		{
			return obj is Range2Double && this == (Range2Double)obj;
		}
		public override int GetHashCode()
		{
			return range1.GetHashCode() ^ range2.GetHashCode();
		}
		public override string ToString()
		{
			return "[" + range1 + ", " + range2 + "]";
		}
		public bool Equals(Range2Double other)
		{
			return this == other;
		}
		public Range2Double Inflate(double value)
		{
			return new Range2Double(range1.Inflate(value), range2.Inflate(value));
		}

		public static bool operator ==(Range2Double a, Range2Double b)
		{
			return a.range1 == b.range1 && a.range2 == b.range2;
		}
		public static bool operator !=(Range2Double a, Range2Double b)
		{
			return a.range1 != b.range1 || a.range2 != b.range2;
		}

		public static Range2Double Intersect(IEnumerable<Range2Double> ranges)
		{
			return new Range2Double(Range<double>.Intersect(ranges.Select(range => range.range1)), Range<double>.Intersect(ranges.Select(range => range.range2)));
		}
		public static Range2Double Union(IEnumerable<Range2Double> ranges)
		{
			return new Range2Double(Range<double>.Union(ranges.Select(range => range.range1)), Range<double>.Union(ranges.Select(range => range.range2)));
		}
		public static IEnumerable<Range2Double> Exclude(Range2Double range, Range2Double exclusion)
		{
			Range2Double intersection = Intersect(new[] { range, exclusion });

			if (intersection.IsEmpty) yield return range;
			else
			{
				Range2Double top = new Range2Double(range.Start1, range.End1, range.Start2, intersection.Start2);
				Range2Double bottom = new Range2Double(range.Start1, range.End1, intersection.End2, range.End2);
				Range2Double left = new Range2Double(range.Start1, intersection.Start1, intersection.Start2, intersection.End2);
				Range2Double right = new Range2Double(intersection.End1, range.End1, intersection.Start2, intersection.End2);

				if (!top.IsEmpty) yield return top;
				if (!bottom.IsEmpty) yield return bottom;
				if (!left.IsEmpty) yield return left;
				if (!right.IsEmpty) yield return right;
			}
		}
		//public static Range2Double InterpolateLinear(Range2Double a, Range2Double b, double fraction)
		//{
		//    return new Range2Double(Vector2Double.InterpolateLinear(a.a, b.a, fraction), Vector2Double.InterpolateLinear(a.b, b.b, fraction));
		//}
	}
}
