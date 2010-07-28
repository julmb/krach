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
	public struct Range1Integer : IEquatable<Range1Integer>
	{
		readonly Range<int> rangeX;

		public static Range1Integer Empty { get { return new Range1Integer(Range<int>.Default); } }

		public Range<int> RangeX { get { return rangeX; } }
		public int StartX { get { return rangeX.Start; } }
		public int EndX { get { return rangeX.End; } }
		public Vector1Integer Size { get { return new Vector1Integer(rangeX.Length()); } }
		public int Volume { get { return Size.X; } }
		public bool IsEmpty { get { return Size.X <= 0; } }

		public Range1Integer(Range<int> rangeX)
		{
			this.rangeX = rangeX;
		}
		public Range1Integer(int startX, int endX)
		{
			this.rangeX = new Range<int>(startX, endX);
		}

		public override bool Equals(object obj)
		{
			return obj is Range1Integer && this == (Range1Integer)obj;
		}
		public override int GetHashCode()
		{
			return rangeX.GetHashCode();
		}
		public override string ToString()
		{
			return "[" + rangeX + "]";
		}
		public bool Equals(Range1Integer other)
		{
			return this == other;
		}
		public Range1Integer Inflate(int value)
		{
			return new Range1Integer(rangeX.Inflate(value));
		}

		public static bool operator ==(Range1Integer range1, Range1Integer range2)
		{
			return range1.rangeX == range2.rangeX;
		}
		public static bool operator !=(Range1Integer range1, Range1Integer range2)
		{
			return range1.rangeX != range2.rangeX;
		}

		public static Range1Integer Intersect(IEnumerable<Range1Integer> ranges)
		{
			return new Range1Integer(Range<int>.Intersect(ranges.Select(range => range.rangeX)));
		}
		public static Range1Integer Union(IEnumerable<Range1Integer> ranges)
		{
			return new Range1Integer(Range<int>.Union(ranges.Select(range => range.rangeX)));
		}
		public static IEnumerable<Range1Integer> Exclude(Range1Integer range, Range1Integer exclusion)
		{
			Range1Integer intersection = Intersect(new[] { range, exclusion });

			if (intersection.IsEmpty) yield return range;
			else
			{
				Range1Integer left = new Range1Integer(range.StartX, intersection.StartX);
				Range1Integer right = new Range1Integer(intersection.EndX, range.EndX);

				if (!left.IsEmpty) yield return left;
				if (!right.IsEmpty) yield return right;
			}
		}
	}
}
