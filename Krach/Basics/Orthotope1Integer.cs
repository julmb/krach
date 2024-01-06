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
	public struct Orthotope1Integer : IEquatable<Orthotope1Integer>
	{
		readonly OrderedRange<int> rangeX;

		public static Orthotope1Integer Empty { get { return new Orthotope1Integer(OrderedRange<int>.Default); } }

		public OrderedRange<int> RangeX { get { return rangeX; } }
		public Vector1Integer Start { get { return new Vector1Integer(rangeX.Start); } }
		public Vector1Integer End { get { return new Vector1Integer(rangeX.End); } }
		public Vector1Integer Size { get { return new Vector1Integer(rangeX.Length()); } }
		public int Volume { get { return Size.X; } }
		public bool IsEmpty { get { return Size.X <= 0; } }

		public Orthotope1Integer(OrderedRange<int> rangeX)
		{
			this.rangeX = rangeX;
		}
		public Orthotope1Integer(Vector1Integer start, Vector1Integer end)
		{
			this.rangeX = new OrderedRange<int>(start.X, end.X);
		}
		public Orthotope1Integer(int startX, int endX)
		{
			this.rangeX = new OrderedRange<int>(startX, endX);
		}

		public override bool Equals(object obj)
		{
			return obj is Orthotope1Integer && this == (Orthotope1Integer)obj;
		}
		public override int GetHashCode()
		{
			return rangeX.GetHashCode();
		}
		public override string ToString()
		{
			return "[" + rangeX + "]";
		}
		public bool Equals(Orthotope1Integer other)
		{
			return this == other;
		}
		public Orthotope1Integer Inflate(int value)
		{
			return new Orthotope1Integer(rangeX.Inflate(value));
		}
		public bool Contains(Vector1Integer vector)
		{
			return rangeX.Contains(vector.X);
		}

		public static bool operator ==(Orthotope1Integer orthotope1, Orthotope1Integer orthotope2)
		{
			return orthotope1.rangeX == orthotope2.rangeX;
		}
		public static bool operator !=(Orthotope1Integer orthotope1, Orthotope1Integer orthotope2)
		{
			return orthotope1.rangeX != orthotope2.rangeX;
		}

		public static implicit operator Orthotope1Double(Orthotope1Integer orthotope)
		{
			return new Orthotope1Double(orthotope.Start, orthotope.End);
		}

		public static Orthotope1Integer Intersect(IEnumerable<Orthotope1Integer> orthotopes)
		{
			return new Orthotope1Integer(OrderedRange<int>.Intersect(orthotopes.Select(orthotope => orthotope.rangeX)));
		}
		public static Orthotope1Integer Union(IEnumerable<Orthotope1Integer> orthotope2)
		{
			return new Orthotope1Integer(OrderedRange<int>.Union(orthotope2.Select(orthotope => orthotope.rangeX)));
		}
		public static IEnumerable<Orthotope1Integer> Exclude(Orthotope1Integer orthotope, Orthotope1Integer exclusion)
		{
			Orthotope1Integer intersection = Intersect(new[] { orthotope, exclusion });

			if (intersection.IsEmpty) yield return orthotope;
			else
			{
				Orthotope1Integer left = new Orthotope1Integer(orthotope.Start.X, intersection.Start.X);
				Orthotope1Integer right = new Orthotope1Integer(intersection.End.X, orthotope.End.X);

				if (!left.IsEmpty) yield return left;
				if (!right.IsEmpty) yield return right;
			}
		}
	}
}
