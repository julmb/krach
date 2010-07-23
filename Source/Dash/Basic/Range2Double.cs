using System;
using System.Collections.Generic;
using System.Linq;

namespace Edge
{
	public struct Range2Double : IEquatable<Range2Double>
	{
		readonly Range<double> rangeX;
		readonly Range<double> rangeY;

		public static Range2Double Empty { get { return new Range2Double(Range<double>.Default, Range<double>.Default); } }

		public Range<double> RangeX { get { return rangeX; } }
		public Range<double> RangeY { get { return rangeY; } }
		public double StartX { get { return rangeX.Start; } }
		public double EndX { get { return rangeX.End; } }
		public double StartY { get { return rangeY.Start; } }
		public double EndY { get { return rangeY.End; } }
		public Vector2Double Size { get { return new Vector2Double(rangeX.Length(), rangeY.Length()); } }
		public double Volume { get { return Size.X * Size.Y; } }
		public bool IsEmpty { get { return Size.X <= 0 || Size.Y <= 0; } }

		public Range2Double(Range<double> rangeX, Range<double> rangeY)
		{
			this.rangeX = rangeX;
			this.rangeY = rangeY;
		}
		public Range2Double(double startX, double endX, double startY, double endY)
		{
			this.rangeX = new Range<double>(startX, endX);
			this.rangeY = new Range<double>(startY, endY);
		}

		public override bool Equals(object obj)
		{
			return obj is Range2Double && this == (Range2Double)obj;
		}
		public override int GetHashCode()
		{
			return rangeX.GetHashCode() ^ rangeY.GetHashCode();
		}
		public override string ToString()
		{
			return "[" + rangeX + ", " + rangeY + "]";
		}
		public bool Equals(Range2Double other)
		{
			return this == other;
		}
		public Range2Double Inflate(double value)
		{
			return new Range2Double(rangeX.Inflate(value), rangeY.Inflate(value));
		}

		public static bool operator ==(Range2Double range1, Range2Double range2)
		{
			return range1.rangeX == range2.rangeX && range1.rangeY == range2.rangeY;
		}
		public static bool operator !=(Range2Double range1, Range2Double range2)
		{
			return range1.rangeX != range2.rangeX || range1.rangeY != range2.rangeY;
		}

		public static Range2Double Intersect(IEnumerable<Range2Double> ranges)
		{
			return new Range2Double(Range<double>.Intersect(ranges.Select(range => range.rangeX)), Range<double>.Intersect(ranges.Select(range => range.rangeY)));
		}
		public static Range2Double Union(IEnumerable<Range2Double> ranges)
		{
			return new Range2Double(Range<double>.Union(ranges.Select(range => range.rangeX)), Range<double>.Union(ranges.Select(range => range.rangeY)));
		}
		public static IEnumerable<Range2Double> Exclude(Range2Double range, Range2Double exclusion)
		{
			Range2Double intersection = Intersect(new[] { range, exclusion });

			if (intersection.IsEmpty) yield return range;
			else
			{
				Range2Double left = new Range2Double(range.StartX, intersection.StartX, intersection.StartY, intersection.EndY);
				Range2Double right = new Range2Double(intersection.EndX, range.EndX, intersection.StartY, intersection.EndY);
				Range2Double top = new Range2Double(range.StartX, range.EndX, range.StartY, intersection.StartY);
				Range2Double bottom = new Range2Double(range.StartX, range.EndX, intersection.EndY, range.EndY);

				if (!left.IsEmpty) yield return left;
				if (!right.IsEmpty) yield return right;
				if (!top.IsEmpty) yield return top;
				if (!bottom.IsEmpty) yield return bottom;
			}
		}
		public static Range2Double InterpolateLinear(Range2Double range1, Range2Double range2, double fraction)
		{
			return new Range2Double(RangeExtensions.InterpolateLinear(range1.rangeX, range2.rangeX, fraction), RangeExtensions.InterpolateLinear(range1.rangeY, range2.rangeY, fraction));
		}
	}
}
