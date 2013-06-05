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
	public struct Orthotope2Integer : IEquatable<Orthotope2Integer>
	{
		readonly OrderedRange<int> rangeX;
		readonly OrderedRange<int> rangeY;

		public static Orthotope2Integer Empty { get { return new Orthotope2Integer(OrderedRange<int>.Default, OrderedRange<int>.Default); } }

		public OrderedRange<int> RangeX { get { return rangeX; } }
		public OrderedRange<int> RangeY { get { return rangeY; } }
		public Vector2Integer Start { get { return new Vector2Integer(rangeX.Start, rangeY.Start); } }
		public Vector2Integer End { get { return new Vector2Integer(rangeX.End, rangeY.End); } }
		public Vector2Integer Size { get { return new Vector2Integer(rangeX.Length(), rangeY.Length()); } }
		public int Volume { get { return Size.X * Size.Y; } }
		public bool IsEmpty { get { return Size.X <= 0 || Size.Y <= 0; } }

		public Orthotope2Integer(OrderedRange<int> rangeX, OrderedRange<int> rangeY)
		{
			this.rangeX = rangeX;
			this.rangeY = rangeY;
		}
		public Orthotope2Integer(Vector2Integer start, Vector2Integer end)
		{
			this.rangeX = new OrderedRange<int>(start.X, end.X);
			this.rangeY = new OrderedRange<int>(start.Y, end.Y);
		}
		public Orthotope2Integer(int startX, int endX, int startY, int endY)
		{
			this.rangeX = new OrderedRange<int>(startX, endX);
			this.rangeY = new OrderedRange<int>(startY, endY);
		}

		public override bool Equals(object obj)
		{
			return obj is Orthotope2Integer && this == (Orthotope2Integer)obj;
		}
		public override int GetHashCode()
		{
			return rangeX.GetHashCode() ^ rangeY.GetHashCode();
		}
		public override string ToString()
		{
			return "[" + rangeX + ", " + rangeY + "]";
		}
		public bool Equals(Orthotope2Integer other)
		{
			return this == other;
		}
		public Orthotope2Integer Inflate(int value)
		{
			return new Orthotope2Integer(rangeX.Inflate(value), rangeY.Inflate(value));
		}
		public bool Contains(Vector2Integer vector)
		{
			return rangeX.Contains(vector.X) && rangeY.Contains(vector.Y);
		}

		public static bool operator ==(Orthotope2Integer orthotope1, Orthotope2Integer orthotope2)
		{
			return orthotope1.rangeX == orthotope2.rangeX && orthotope1.rangeY == orthotope2.rangeY;
		}
		public static bool operator !=(Orthotope2Integer orthotope1, Orthotope2Integer orthotope2)
		{
			return orthotope1.rangeX != orthotope2.rangeX || orthotope1.rangeY != orthotope2.rangeY;
		}

		public static implicit operator Orthotope2Double(Orthotope2Integer orthotope)
		{
			return new Orthotope2Double(orthotope.Start, orthotope.End);
		}

		public static Orthotope2Integer Intersect(IEnumerable<Orthotope2Integer> orthotopes)
		{
			return new Orthotope2Integer(OrderedRange<int>.Intersect(orthotopes.Select(orthotope => orthotope.rangeX)), OrderedRange<int>.Intersect(orthotopes.Select(orthotope => orthotope.rangeY)));
		}
		public static Orthotope2Integer Union(IEnumerable<Orthotope2Integer> orthotopes)
		{
			return new Orthotope2Integer(OrderedRange<int>.Union(orthotopes.Select(orthotope => orthotope.rangeX)), OrderedRange<int>.Union(orthotopes.Select(orthotope => orthotope.rangeY)));
		}
		public static IEnumerable<Orthotope2Integer> Exclude(Orthotope2Integer orthotope, Orthotope2Integer exclusion)
		{
			Orthotope2Integer intersection = Intersect(new[] { orthotope, exclusion });

			if (intersection.IsEmpty) yield return orthotope;
			else
			{
				Orthotope2Integer left = new Orthotope2Integer(orthotope.Start.X, intersection.Start.X, intersection.Start.Y, intersection.End.Y);
				Orthotope2Integer right = new Orthotope2Integer(intersection.End.X, orthotope.End.X, intersection.Start.Y, intersection.End.Y);
				Orthotope2Integer top = new Orthotope2Integer(orthotope.Start.X, orthotope.End.X, orthotope.Start.Y, intersection.Start.Y);
				Orthotope2Integer bottom = new Orthotope2Integer(orthotope.Start.X, orthotope.End.X, intersection.End.Y, orthotope.End.Y);

				if (!left.IsEmpty) yield return left;
				if (!right.IsEmpty) yield return right;
				if (!top.IsEmpty) yield return top;
				if (!bottom.IsEmpty) yield return bottom;
			}
		}
	}
}
