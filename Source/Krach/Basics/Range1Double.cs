using System;
using System.Collections.Generic;
using System.Linq;
using Dash.Extensions;

namespace Dash.Basics
{
	public struct Range1Double : IEquatable<Range1Double>
	{
		readonly Range<double> rangeX;

		public static Range1Double Empty { get { return new Range1Double(Range<double>.Default); } }

		public Range<double> RangeX { get { return rangeX; } }
		public double StartX { get { return rangeX.Start; } }
		public double EndX { get { return rangeX.End; } }
		public Vector1Double Size { get { return new Vector1Double(rangeX.Length()); } }
		public double Volume { get { return Size.X; } }
		public bool IsEmpty { get { return Size.X <= 0; } }

		public Range1Double(Range<double> rangeX)
		{
			this.rangeX = rangeX;
		}
		public Range1Double(double startX, double endX)
		{
			this.rangeX = new Range<double>(startX, endX);
		}

		public override bool Equals(object obj)
		{
			return obj is Range1Double && this == (Range1Double)obj;
		}
		public override int GetHashCode()
		{
			return rangeX.GetHashCode();
		}
		public override string ToString()
		{
			return "[" + rangeX + "]";
		}
		public bool Equals(Range1Double other)
		{
			return this == other;
		}
		public Range1Double Inflate(double value)
		{
			return new Range1Double(rangeX.Inflate(value));
		}

		public static bool operator ==(Range1Double range1, Range1Double range2)
		{
			return range1.rangeX == range2.rangeX;
		}
		public static bool operator !=(Range1Double range1, Range1Double range2)
		{
			return range1.rangeX != range2.rangeX;
		}

		public static Range1Double Intersect(IEnumerable<Range1Double> ranges)
		{
			return new Range1Double(Range<double>.Intersect(ranges.Select(range => range.rangeX)));
		}
		public static Range1Double Union(IEnumerable<Range1Double> ranges)
		{
			return new Range1Double(Range<double>.Union(ranges.Select(range => range.rangeX)));
		}
		public static IEnumerable<Range1Double> Exclude(Range1Double range, Range1Double exclusion)
		{
			Range1Double intersection = Intersect(new[] { range, exclusion });

			if (intersection.IsEmpty) yield return range;
			else
			{
				Range1Double left = new Range1Double(range.StartX, intersection.StartX);
				Range1Double right = new Range1Double(intersection.EndX, range.EndX);

				if (!left.IsEmpty) yield return left;
				if (!right.IsEmpty) yield return right;
			}
		}
		public static Range1Double InterpolateLinear(Range1Double range1, Range1Double range2, double fraction)
		{
			return new Range1Double(Ranges.InterpolateLinear(range1.rangeX, range2.rangeX, fraction));
		}
	}
}
