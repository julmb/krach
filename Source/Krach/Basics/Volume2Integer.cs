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
	public struct Volume2Integer : IEquatable<Volume2Integer>
	{
		readonly Range<int> rangeX;
		readonly Range<int> rangeY;

		public static Volume2Integer Empty { get { return new Volume2Integer(Range<int>.Default, Range<int>.Default); } }

		public Range<int> RangeX { get { return rangeX; } }
		public Range<int> RangeY { get { return rangeY; } }
		public Vector2Integer Start { get { return new Vector2Integer(rangeX.Start, rangeY.Start); } }
		public Vector2Integer End { get { return new Vector2Integer(rangeX.End, rangeY.End); } }
		public int StartX { get { return rangeX.Start; } }
		public int EndX { get { return rangeX.End; } }
		public int StartY { get { return rangeY.Start; } }
		public int EndY { get { return rangeY.End; } }
		public Vector2Integer Size { get { return new Vector2Integer(rangeX.Length(), rangeY.Length()); } }
		public int Volume { get { return Size.X * Size.Y; } }
		public bool IsEmpty { get { return Size.X <= 0 || Size.Y <= 0; } }

		public Volume2Integer(Range<int> rangeX, Range<int> rangeY)
		{
			this.rangeX = rangeX;
			this.rangeY = rangeY;
		}
		public Volume2Integer(Vector2Integer start, Vector2Integer end)
		{
			this.rangeX = new Range<int>(start.X, end.X);
			this.rangeY = new Range<int>(start.Y, end.Y);
		}
		public Volume2Integer(int startX, int endX, int startY, int endY)
		{
			this.rangeX = new Range<int>(startX, endX);
			this.rangeY = new Range<int>(startY, endY);
		}

		public override bool Equals(object obj)
		{
			return obj is Volume2Integer && this == (Volume2Integer)obj;
		}
		public override int GetHashCode()
		{
			return rangeX.GetHashCode() ^ rangeY.GetHashCode();
		}
		public override string ToString()
		{
			return "[" + rangeX + ", " + rangeY + "]";
		}
		public bool Equals(Volume2Integer other)
		{
			return this == other;
		}
		public Volume2Integer Inflate(int value)
		{
			return new Volume2Integer(rangeX.Inflate(value), rangeY.Inflate(value));
		}

		public static bool operator ==(Volume2Integer range1, Volume2Integer range2)
		{
			return range1.rangeX == range2.rangeX && range1.rangeY == range2.rangeY;
		}
		public static bool operator !=(Volume2Integer range1, Volume2Integer range2)
		{
			return range1.rangeX != range2.rangeX || range1.rangeY != range2.rangeY;
		}

		public static implicit operator Volume2Double(Volume2Integer volume)
		{
			return new Volume2Double(volume.Start, volume.End);
		}

		public static Volume2Integer Intersect(IEnumerable<Volume2Integer> ranges)
		{
			return new Volume2Integer(Range<int>.Intersect(ranges.Select(range => range.rangeX)), Range<int>.Intersect(ranges.Select(range => range.rangeY)));
		}
		public static Volume2Integer Union(IEnumerable<Volume2Integer> ranges)
		{
			return new Volume2Integer(Range<int>.Union(ranges.Select(range => range.rangeX)), Range<int>.Union(ranges.Select(range => range.rangeY)));
		}
		public static IEnumerable<Volume2Integer> Exclude(Volume2Integer range, Volume2Integer exclusion)
		{
			Volume2Integer intersection = Intersect(new[] { range, exclusion });

			if (intersection.IsEmpty) yield return range;
			else
			{
				Volume2Integer left = new Volume2Integer(range.StartX, intersection.StartX, intersection.StartY, intersection.EndY);
				Volume2Integer right = new Volume2Integer(intersection.EndX, range.EndX, intersection.StartY, intersection.EndY);
				Volume2Integer top = new Volume2Integer(range.StartX, range.EndX, range.StartY, intersection.StartY);
				Volume2Integer bottom = new Volume2Integer(range.StartX, range.EndX, intersection.EndY, range.EndY);

				if (!left.IsEmpty) yield return left;
				if (!right.IsEmpty) yield return right;
				if (!top.IsEmpty) yield return top;
				if (!bottom.IsEmpty) yield return bottom;
			}
		}
	}
}
