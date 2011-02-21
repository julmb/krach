// Copyright © Julian Brunner 2009 - 2011

// This file is part of Stream Visualizer (streamvis).
// 
// Stream Visualizer is free software: you can redistribute it and/or modify it
// under the terms of the GNU General Public License as published by the Free
// Software Foundation, either version 3 of the License, or (at your option) any
// later version.
// 
// Stream Visualizer is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more
// details.
// 
// You should have received a copy of the GNU General Public License along with
// Stream Visualizer. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;

namespace Krach.Basics
{
	public struct Volume1Integer : IEquatable<Volume1Integer>
	{
		readonly Range<int> rangeX;

		public static Volume1Integer Empty { get { return new Volume1Integer(Range<int>.Default); } }

		public Range<int> RangeX { get { return rangeX; } }
		public Vector1Integer Start { get { return new Vector1Integer(rangeX.Start); } }
		public Vector1Integer End { get { return new Vector1Integer(rangeX.End); } }
		public int StartX { get { return rangeX.Start; } }
		public int EndX { get { return rangeX.End; } }
		public Vector1Integer Size { get { return new Vector1Integer(rangeX.Length()); } }
		public int Volume { get { return Size.X; } }
		public bool IsEmpty { get { return Size.X <= 0; } }

		public Volume1Integer(Range<int> rangeX)
		{
			this.rangeX = rangeX;
		}
		public Volume1Integer(Vector1Integer start, Vector1Integer end)
		{
			this.rangeX = new Range<int>(start.X, end.X);
		}
		public Volume1Integer(int startX, int endX)
		{
			this.rangeX = new Range<int>(startX, endX);
		}

		public override bool Equals(object obj)
		{
			return obj is Volume1Integer && this == (Volume1Integer)obj;
		}
		public override int GetHashCode()
		{
			return rangeX.GetHashCode();
		}
		public override string ToString()
		{
			return "[" + rangeX + "]";
		}
		public bool Equals(Volume1Integer other)
		{
			return this == other;
		}
		public Volume1Integer Inflate(int value)
		{
			return new Volume1Integer(rangeX.Inflate(value));
		}

		public static bool operator ==(Volume1Integer range1, Volume1Integer range2)
		{
			return range1.rangeX == range2.rangeX;
		}
		public static bool operator !=(Volume1Integer range1, Volume1Integer range2)
		{
			return range1.rangeX != range2.rangeX;
		}

		public static implicit operator Volume1Double(Volume1Integer volume)
		{
			return new Volume1Double(volume.Start, volume.End);
		}

		public static Volume1Integer Intersect(IEnumerable<Volume1Integer> ranges)
		{
			return new Volume1Integer(Range<int>.Intersect(ranges.Select(range => range.rangeX)));
		}
		public static Volume1Integer Union(IEnumerable<Volume1Integer> ranges)
		{
			return new Volume1Integer(Range<int>.Union(ranges.Select(range => range.rangeX)));
		}
		public static IEnumerable<Volume1Integer> Exclude(Volume1Integer range, Volume1Integer exclusion)
		{
			Volume1Integer intersection = Intersect(new[] { range, exclusion });

			if (intersection.IsEmpty) yield return range;
			else
			{
				Volume1Integer left = new Volume1Integer(range.StartX, intersection.StartX);
				Volume1Integer right = new Volume1Integer(intersection.EndX, range.EndX);

				if (!left.IsEmpty) yield return left;
				if (!right.IsEmpty) yield return right;
			}
		}
	}
}
