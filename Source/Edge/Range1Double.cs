using System;
using System.Collections.Generic;
using System.Linq;

namespace Edge
{
	public struct Range1Double : IEquatable<Range1Double>
	{
		readonly Range<double> range1;

		public static Range1Double Empty { get { return new Range1Double(Range<double>.Default); } }

		public Range<double> Range1 { get { return range1; } }
		public double Start1 { get { return range1.Start; } }
		public double End1 { get { return range1.End; } }
		public Vector1Double Size { get { return new Vector1Double(range1.Length()); } }
		public double Volume { get { return Size.X; } }
		public bool IsEmpty { get { return Size.X <= 0; } }

		public Range1Double(Range<double> range1)
		{
			this.range1 = range1;
		}
		public Range1Double(double start1, double end1)
		{
			this.range1 = new Range<double>(start1, end1);
		}

		public override bool Equals(object obj)
		{
			return obj is Range1Double && this == (Range1Double)obj;
		}
		public override int GetHashCode()
		{
			return range1.GetHashCode();
		}
		public override string ToString()
		{
			return "[" + range1  + "]";
		}
		public bool Equals(Range1Double other)
		{
			return this == other;
		}
		public Range1Double Inflate(double value)
		{
			return new Range1Double(range1.Inflate(value));
		}

		public static bool operator ==(Range1Double a, Range1Double b)
		{
			return a.range1 == b.range1;
		}
		public static bool operator !=(Range1Double a, Range1Double b)
		{
			return a.range1 != b.range1 ;
		}

		public static Range1Double Intersect(IEnumerable<Range1Double> ranges)
		{
			return new Range1Double(Range<double>.Intersect(ranges.Select(range => range.range1)));
		}
		public static Range1Double Union(IEnumerable<Range1Double> ranges)
		{
			return new Range1Double(Range<double>.Union(ranges.Select(range => range.range1)));
		}
		public static IEnumerable<Range1Double> Exclude(Range1Double range, Range1Double exclusion)
		{
			Range1Double intersection = Intersect(new[] { range, exclusion });

			if (intersection.IsEmpty) yield return range;
			else
			{
				Range1Double left = new Range1Double(range.Start1, intersection.Start1);
				Range1Double right = new Range1Double(intersection.End1, range.End1);

				if (!left.IsEmpty) yield return left;
				if (!right.IsEmpty) yield return right;
			}
		}
		//public static Range1Double InterpolateLinear(Range1Double a, Range1Double b, double fraction)
		//{
		//    return new Range1Double(Vector2Double.InterpolateLinear(a.a, b.a, fraction), Vector2Double.InterpolateLinear(a.b, b.b, fraction));
		//}
	}
}
