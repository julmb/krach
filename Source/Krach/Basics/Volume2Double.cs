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
using Krach.Extensions;

namespace Krach.Basics
{
	public struct Volume2Double : IEquatable<Volume2Double>
	{
		readonly Range<double> rangeX;
		readonly Range<double> rangeY;

		public static Volume2Double Empty { get { return new Volume2Double(Range<double>.Default, Range<double>.Default); } }

		public Range<double> RangeX { get { return rangeX; } }
		public Range<double> RangeY { get { return rangeY; } }
		public Vector2Double Start { get { return new Vector2Double(rangeX.Start, rangeY.Start); } }
		public Vector2Double End { get { return new Vector2Double(rangeX.End, rangeY.End); } }
		public double StartX { get { return rangeX.Start; } }
		public double EndX { get { return rangeX.End; } }
		public double StartY { get { return rangeY.Start; } }
		public double EndY { get { return rangeY.End; } }
		public Vector2Double Size { get { return new Vector2Double(rangeX.Length(), rangeY.Length()); } }
		public double Volume { get { return Size.X * Size.Y; } }
		public bool IsEmpty { get { return Size.X <= 0 || Size.Y <= 0; } }

		public Volume2Double(Range<double> rangeX, Range<double> rangeY)
		{
			this.rangeX = rangeX;
			this.rangeY = rangeY;
		}
		public Volume2Double(Vector2Double start, Vector2Double end)
		{
			this.rangeX = new Range<double>(start.X, end.X);
			this.rangeY = new Range<double>(start.Y, end.Y);
		}
		public Volume2Double(double startX, double endX, double startY, double endY)
		{
			this.rangeX = new Range<double>(startX, endX);
			this.rangeY = new Range<double>(startY, endY);
		}

		public override bool Equals(object obj)
		{
			return obj is Volume2Double && this == (Volume2Double)obj;
		}
		public override int GetHashCode()
		{
			return rangeX.GetHashCode() ^ rangeY.GetHashCode();
		}
		public override string ToString()
		{
			return "[" + rangeX + ", " + rangeY + "]";
		}
		public bool Equals(Volume2Double other)
		{
			return this == other;
		}
		public Volume2Double Inflate(double value)
		{
			return new Volume2Double(rangeX.Inflate(value), rangeY.Inflate(value));
		}

		public static bool operator ==(Volume2Double range1, Volume2Double range2)
		{
			return range1.rangeX == range2.rangeX && range1.rangeY == range2.rangeY;
		}
		public static bool operator !=(Volume2Double range1, Volume2Double range2)
		{
			return range1.rangeX != range2.rangeX || range1.rangeY != range2.rangeY;
		}

		public static Volume2Double Intersect(IEnumerable<Volume2Double> ranges)
		{
			return new Volume2Double(Range<double>.Intersect(ranges.Select(range => range.rangeX)), Range<double>.Intersect(ranges.Select(range => range.rangeY)));
		}
		public static Volume2Double Union(IEnumerable<Volume2Double> ranges)
		{
			return new Volume2Double(Range<double>.Union(ranges.Select(range => range.rangeX)), Range<double>.Union(ranges.Select(range => range.rangeY)));
		}
		public static IEnumerable<Volume2Double> Exclude(Volume2Double range, Volume2Double exclusion)
		{
			Volume2Double intersection = Intersect(new[] { range, exclusion });

			if (intersection.IsEmpty) yield return range;
			else
			{
				Volume2Double left = new Volume2Double(range.StartX, intersection.StartX, intersection.StartY, intersection.EndY);
				Volume2Double right = new Volume2Double(intersection.EndX, range.EndX, intersection.StartY, intersection.EndY);
				Volume2Double top = new Volume2Double(range.StartX, range.EndX, range.StartY, intersection.StartY);
				Volume2Double bottom = new Volume2Double(range.StartX, range.EndX, intersection.EndY, range.EndY);

				if (!left.IsEmpty) yield return left;
				if (!right.IsEmpty) yield return right;
				if (!top.IsEmpty) yield return top;
				if (!bottom.IsEmpty) yield return bottom;
			}
		}
		public static Volume2Double InterpolateLinear(Volume2Double range1, Volume2Double range2, double fraction)
		{
			return new Volume2Double(Ranges.InterpolateLinear(range1.rangeX, range2.rangeX, fraction), Ranges.InterpolateLinear(range1.rangeY, range2.rangeY, fraction));
		}
	}
}
