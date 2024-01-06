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
		readonly OrderedRange<double> rangeX;
		readonly OrderedRange<double> rangeY;

		public static Orthotope2Double Empty { get { return new Orthotope2Double(OrderedRange<double>.Default, OrderedRange<double>.Default); } }

		public OrderedRange<double> RangeX { get { return rangeX; } }
		public OrderedRange<double> RangeY { get { return rangeY; } }
		public Vector2Double Start { get { return new Vector2Double(rangeX.Start, rangeY.Start); } }
		public Vector2Double End { get { return new Vector2Double(rangeX.End, rangeY.End); } }
		public Vector2Double Size { get { return new Vector2Double(rangeX.Length(), rangeY.Length()); } }
		public double Volume { get { return Size.X * Size.Y; } }
		public bool IsEmpty { get { return Size.X <= 0 || Size.Y <= 0; } }

		public Orthotope2Double(OrderedRange<double> rangeX, OrderedRange<double> rangeY)
		{
			this.rangeX = rangeX;
			this.rangeY = rangeY;
		}
		public Orthotope2Double(Vector2Double start, Vector2Double end)
		{
			this.rangeX = new OrderedRange<double>(start.X, end.X);
			this.rangeY = new OrderedRange<double>(start.Y, end.Y);
		}
		public Orthotope2Double(double startX, double endX, double startY, double endY)
		{
			this.rangeX = new OrderedRange<double>(startX, endX);
			this.rangeY = new OrderedRange<double>(startY, endY);
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
		public bool Contains(Vector2Double vector)
		{
			return rangeX.Contains(vector.X) && rangeY.Contains(vector.Y);
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
			return new Orthotope2Double(OrderedRange<double>.Intersect(orthotopes.Select(orthotope => orthotope.rangeX)), OrderedRange<double>.Intersect(orthotopes.Select(orthotope => orthotope.rangeY)));
		}
		public static Orthotope2Double Union(IEnumerable<Orthotope2Double> orthotopes)
		{
			return new Orthotope2Double(OrderedRange<double>.Union(orthotopes.Select(orthotope => orthotope.rangeX)), OrderedRange<double>.Union(orthotopes.Select(orthotope => orthotope.rangeY)));
		}
		public static IEnumerable<Orthotope2Double> Exclude(Orthotope2Double orthotope, Orthotope2Double exclusion)
		{
			Orthotope2Double intersection = Intersect(new[] { orthotope, exclusion });

			if (intersection.IsEmpty) yield return orthotope;
			else
			{
				Orthotope2Double left = new Orthotope2Double(orthotope.Start.X, intersection.Start.X, intersection.Start.Y, intersection.End.Y);
				Orthotope2Double right = new Orthotope2Double(intersection.End.X, orthotope.End.X, intersection.Start.Y, intersection.End.Y);
				Orthotope2Double top = new Orthotope2Double(orthotope.Start.X, orthotope.End.X, orthotope.Start.Y, intersection.Start.Y);
				Orthotope2Double bottom = new Orthotope2Double(orthotope.Start.X, orthotope.End.X, intersection.End.Y, orthotope.End.Y);

				if (!left.IsEmpty) yield return left;
				if (!right.IsEmpty) yield return right;
				if (!top.IsEmpty) yield return top;
				if (!bottom.IsEmpty) yield return bottom;
			}
		}
		public static Orthotope2Double Interpolate(Orthotope2Double orthotope1, Orthotope2Double orthotope2, Interpolation<double> interpolate, double fraction)
		{
			return new Orthotope2Double(OrderedRanges.Interpolate(orthotope1.rangeX, orthotope2.rangeX, interpolate, fraction), OrderedRanges.Interpolate(orthotope1.rangeY, orthotope2.rangeY, interpolate, fraction));
		}
	}
}
