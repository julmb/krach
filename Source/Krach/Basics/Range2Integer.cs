// Copyright © Julian Brunner 2010

// This file is part of Krach.
//
// Krach is free software: you can redistribute it and/or modify it under the
// terms of the GNU Lesser General Public License as published by the Free
// Software Foundation, either version 3 of the License, or (at your option) any
// later version.
//
// Krach is distributed in the hope that it will be useful, but WITHOUT ANY
// WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
// A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with
// Krach. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;

namespace Krach.Basics
{
	public struct Range2Integer : IEquatable<Range2Integer>
	{
		readonly Range<int> rangeX;
		readonly Range<int> rangeY;

		public static Range2Integer Empty { get { return new Range2Integer(Range<int>.Default, Range<int>.Default); } }

		public Range<int> RangeX { get { return rangeX; } }
		public Range<int> RangeY { get { return rangeY; } }
		public int StartX { get { return rangeX.Start; } }
		public int EndX { get { return rangeX.End; } }
		public int StartY { get { return rangeY.Start; } }
		public int EndY { get { return rangeY.End; } }
		public Vector2Integer Size { get { return new Vector2Integer(rangeX.Length(), rangeY.Length()); } }
		public int Volume { get { return Size.X * Size.Y; } }
		public bool IsEmpty { get { return Size.X <= 0 || Size.Y <= 0; } }

		public Range2Integer(Range<int> rangeX, Range<int> rangeY)
		{
			this.rangeX = rangeX;
			this.rangeY = rangeY;
		}
		public Range2Integer(Vector2Integer start, Vector2Integer end)
		{
			this.rangeX = new Range<int>(start.X, end.X);
			this.rangeY = new Range<int>(start.Y, end.Y);
		}
		public Range2Integer(int startX, int endX, int startY, int endY)
		{
			this.rangeX = new Range<int>(startX, endX);
			this.rangeY = new Range<int>(startY, endY);
		}

		public override bool Equals(object obj)
		{
			return obj is Range2Integer && this == (Range2Integer)obj;
		}
		public override int GetHashCode()
		{
			return rangeX.GetHashCode() ^ rangeY.GetHashCode();
		}
		public override string ToString()
		{
			return "[" + rangeX + ", " + rangeY + "]";
		}
		public bool Equals(Range2Integer other)
		{
			return this == other;
		}
		public Range2Integer Inflate(int value)
		{
			return new Range2Integer(rangeX.Inflate(value), rangeY.Inflate(value));
		}

		public static bool operator ==(Range2Integer range1, Range2Integer range2)
		{
			return range1.rangeX == range2.rangeX && range1.rangeY == range2.rangeY;
		}
		public static bool operator !=(Range2Integer range1, Range2Integer range2)
		{
			return range1.rangeX != range2.rangeX || range1.rangeY != range2.rangeY;
		}

		public static Range2Integer Intersect(IEnumerable<Range2Integer> ranges)
		{
			return new Range2Integer(Range<int>.Intersect(ranges.Select(range => range.rangeX)), Range<int>.Intersect(ranges.Select(range => range.rangeY)));
		}
		public static Range2Integer Union(IEnumerable<Range2Integer> ranges)
		{
			return new Range2Integer(Range<int>.Union(ranges.Select(range => range.rangeX)), Range<int>.Union(ranges.Select(range => range.rangeY)));
		}
		public static IEnumerable<Range2Integer> Exclude(Range2Integer range, Range2Integer exclusion)
		{
			Range2Integer intersection = Intersect(new[] { range, exclusion });

			if (intersection.IsEmpty) yield return range;
			else
			{
				Range2Integer left = new Range2Integer(range.StartX, intersection.StartX, intersection.StartY, intersection.EndY);
				Range2Integer right = new Range2Integer(intersection.EndX, range.EndX, intersection.StartY, intersection.EndY);
				Range2Integer top = new Range2Integer(range.StartX, range.EndX, range.StartY, intersection.StartY);
				Range2Integer bottom = new Range2Integer(range.StartX, range.EndX, intersection.EndY, range.EndY);

				if (!left.IsEmpty) yield return left;
				if (!right.IsEmpty) yield return right;
				if (!top.IsEmpty) yield return top;
				if (!bottom.IsEmpty) yield return bottom;
			}
		}
	}
}
