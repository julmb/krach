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
	public struct Orthotope2Double : IEquatable<Orthotope2Double>
	{
		readonly Range<double> rangeX;
		readonly Range<double> rangeY;

		public static Orthotope2Double Empty { get { return new Orthotope2Double(Range<double>.Default, Range<double>.Default); } }

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

		public Orthotope2Double(Range<double> rangeX, Range<double> rangeY)
		{
			this.rangeX = rangeX;
			this.rangeY = rangeY;
		}
		public Orthotope2Double(Vector2Double start, Vector2Double end)
		{
			this.rangeX = new Range<double>(start.X, end.X);
			this.rangeY = new Range<double>(start.Y, end.Y);
		}
		public Orthotope2Double(double startX, double endX, double startY, double endY)
		{
			this.rangeX = new Range<double>(startX, endX);
			this.rangeY = new Range<double>(startY, endY);
		}

		public override bool Equals(object obj)
		{
			return obj is Orthotope2Double && this == (Orthotope2Double)obj;
		}
		public override int GetHashCode()
		{
			return rangeX.GetHashCode() ^ rangeY.GetHashCode();
		}
		public override string ToString()
		{
			return "[" + rangeX + ", " + rangeY + "]";
		}
		public bool Equals(Orthotope2Double other)
		{
			return this == other;
		}
		public Orthotope2Double Inflate(double value)
		{
			return new Orthotope2Double(rangeX.Inflate(value), rangeY.Inflate(value));
		}

		public static bool operator ==(Orthotope2Double orthotope1, Orthotope2Double orthotope2)
		{
			return orthotope1.rangeX == orthotope2.rangeX && orthotope1.rangeY == orthotope2.rangeY;
		}
		public static bool operator !=(Orthotope2Double orthotope1, Orthotope2Double orthotope2)
		{
			return orthotope1.rangeX != orthotope2.rangeX || orthotope1.rangeY != orthotope2.rangeY;
		}

		public static Orthotope2Double CreateFromCenter(Vector2Double center, Vector2Double size)
		{
			return new Orthotope2Double(center - 0.5 * size, center + 0.5 * size);
		}
		public static Orthotope2Double Intersect(IEnumerable<Orthotope2Double> orthotopes)
		{
			return new Orthotope2Double(Range<double>.Intersect(orthotopes.Select(orthotope => orthotope.rangeX)), Range<double>.Intersect(orthotopes.Select(orthotope => orthotope.rangeY)));
		}
		public static Orthotope2Double Union(IEnumerable<Orthotope2Double> orthotopes)
		{
			return new Orthotope2Double(Range<double>.Union(orthotopes.Select(orthotope => orthotope.rangeX)), Range<double>.Union(orthotopes.Select(orthotope => orthotope.rangeY)));
		}
		public static IEnumerable<Orthotope2Double> Exclude(Orthotope2Double orthotope, Orthotope2Double exclusion)
		{
			Orthotope2Double intersection = Intersect(new[] { orthotope, exclusion });

			if (intersection.IsEmpty) yield return orthotope;
			else
			{
				Orthotope2Double left = new Orthotope2Double(orthotope.StartX, intersection.StartX, intersection.StartY, intersection.EndY);
				Orthotope2Double right = new Orthotope2Double(intersection.EndX, orthotope.EndX, intersection.StartY, intersection.EndY);
				Orthotope2Double top = new Orthotope2Double(orthotope.StartX, orthotope.EndX, orthotope.StartY, intersection.StartY);
				Orthotope2Double bottom = new Orthotope2Double(orthotope.StartX, orthotope.EndX, intersection.EndY, orthotope.EndY);

				if (!left.IsEmpty) yield return left;
				if (!right.IsEmpty) yield return right;
				if (!top.IsEmpty) yield return top;
				if (!bottom.IsEmpty) yield return bottom;
			}
		}
		public static Orthotope2Double Interpolate(Orthotope2Double orthotope1, Orthotope2Double orthotope2, Interpolation<double> interpolate, double fraction)
		{
			return new Orthotope2Double(Ranges.Interpolate(orthotope1.rangeX, orthotope2.rangeX, interpolate, fraction), Ranges.Interpolate(orthotope1.rangeY, orthotope2.rangeY, interpolate, fraction));
		}
	}
}
