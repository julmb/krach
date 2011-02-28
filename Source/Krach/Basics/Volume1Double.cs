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
	public struct Volume1Double : IEquatable<Volume1Double>
	{
		readonly Range<double> rangeX;

		public static Volume1Double Empty { get { return new Volume1Double(Range<double>.Default); } }

		public Range<double> RangeX { get { return rangeX; } }
		public Vector1Double Start { get { return new Vector1Double(rangeX.Start); } }
		public Vector1Double End { get { return new Vector1Double(rangeX.End); } }
		public double StartX { get { return rangeX.Start; } }
		public double EndX { get { return rangeX.End; } }
		public Vector1Double Size { get { return new Vector1Double(rangeX.Length()); } }
		public double Volume { get { return Size.X; } }
		public bool IsEmpty { get { return Size.X <= 0; } }

		public Volume1Double(Range<double> rangeX)
		{
			this.rangeX = rangeX;
		}
		public Volume1Double(Vector1Double start, Vector1Double end)
		{
			this.rangeX = new Range<double>(start.X, end.X);
		}
		public Volume1Double(double startX, double endX)
		{
			this.rangeX = new Range<double>(startX, endX);
		}

		public override bool Equals(object obj)
		{
			return obj is Volume1Double && this == (Volume1Double)obj;
		}
		public override int GetHashCode()
		{
			return rangeX.GetHashCode();
		}
		public override string ToString()
		{
			return "[" + rangeX + "]";
		}
		public bool Equals(Volume1Double other)
		{
			return this == other;
		}
		public Volume1Double Inflate(double value)
		{
			return new Volume1Double(rangeX.Inflate(value));
		}

		public static bool operator ==(Volume1Double range1, Volume1Double range2)
		{
			return range1.rangeX == range2.rangeX;
		}
		public static bool operator !=(Volume1Double range1, Volume1Double range2)
		{
			return range1.rangeX != range2.rangeX;
		}

		public static Volume1Double Intersect(IEnumerable<Volume1Double> ranges)
		{
			return new Volume1Double(Range<double>.Intersect(ranges.Select(range => range.rangeX)));
		}
		public static Volume1Double Union(IEnumerable<Volume1Double> ranges)
		{
			return new Volume1Double(Range<double>.Union(ranges.Select(range => range.rangeX)));
		}
		public static IEnumerable<Volume1Double> Exclude(Volume1Double range, Volume1Double exclusion)
		{
			Volume1Double intersection = Intersect(new[] { range, exclusion });

			if (intersection.IsEmpty) yield return range;
			else
			{
				Volume1Double left = new Volume1Double(range.StartX, intersection.StartX);
				Volume1Double right = new Volume1Double(intersection.EndX, range.EndX);

				if (!left.IsEmpty) yield return left;
				if (!right.IsEmpty) yield return right;
			}
		}
		public static Volume1Double InterpolateLinear(Volume1Double range1, Volume1Double range2, double fraction)
		{
			return new Volume1Double(Ranges.InterpolateLinear(range1.rangeX, range2.rangeX, fraction));
		}
	}
}
