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
	public struct Orthotope1Double : IEquatable<Orthotope1Double>
	{
		readonly Range<double> rangeX;

		public static Orthotope1Double Empty { get { return new Orthotope1Double(Range<double>.Default); } }

		public Range<double> RangeX { get { return rangeX; } }
		public Vector1Double Start { get { return new Vector1Double(rangeX.Start); } }
		public Vector1Double End { get { return new Vector1Double(rangeX.End); } }
		public double StartX { get { return rangeX.Start; } }
		public double EndX { get { return rangeX.End; } }
		public Vector1Double Size { get { return new Vector1Double(rangeX.Length()); } }
		public double Volume { get { return Size.X; } }
		public bool IsEmpty { get { return Size.X <= 0; } }

		public Orthotope1Double(Range<double> rangeX)
		{
			this.rangeX = rangeX;
		}
		public Orthotope1Double(Vector1Double start, Vector1Double end)
		{
			this.rangeX = new Range<double>(start.X, end.X);
		}
		public Orthotope1Double(double startX, double endX)
		{
			this.rangeX = new Range<double>(startX, endX);
		}

		public override bool Equals(object obj)
		{
			return obj is Orthotope1Double && this == (Orthotope1Double)obj;
		}
		public override int GetHashCode()
		{
			return rangeX.GetHashCode();
		}
		public override string ToString()
		{
			return "[" + rangeX + "]";
		}
		public bool Equals(Orthotope1Double other)
		{
			return this == other;
		}
		public Orthotope1Double Inflate(double value)
		{
			return new Orthotope1Double(rangeX.Inflate(value));
		}
		public bool Contains(Vector1Double vector)
		{
			return rangeX.Contains(vector.X);
		}

		public static bool operator ==(Orthotope1Double orthotope1, Orthotope1Double orthotope2)
		{
			return orthotope1.rangeX == orthotope2.rangeX;
		}
		public static bool operator !=(Orthotope1Double orthotope1, Orthotope1Double orthotope2)
		{
			return orthotope1.rangeX != orthotope2.rangeX;
		}

		public static Orthotope1Double CreateFromCenter(Vector1Double center, Vector1Double size)
		{
			return new Orthotope1Double(center - 0.5 * size, center + 0.5 * size);
		}
		public static Orthotope1Double Intersect(IEnumerable<Orthotope1Double> orthotopes)
		{
			return new Orthotope1Double(Range<double>.Intersect(orthotopes.Select(orthotope => orthotope.rangeX)));
		}
		public static Orthotope1Double Union(IEnumerable<Orthotope1Double> orthotopes)
		{
			return new Orthotope1Double(Range<double>.Union(orthotopes.Select(orthotope => orthotope.rangeX)));
		}
		public static IEnumerable<Orthotope1Double> Exclude(Orthotope1Double orthotope, Orthotope1Double exclusion)
		{
			Orthotope1Double intersection = Intersect(new[] { orthotope, exclusion });

			if (intersection.IsEmpty) yield return orthotope;
			else
			{
				Orthotope1Double left = new Orthotope1Double(orthotope.StartX, intersection.StartX);
				Orthotope1Double right = new Orthotope1Double(intersection.EndX, orthotope.EndX);

				if (!left.IsEmpty) yield return left;
				if (!right.IsEmpty) yield return right;
			}
		}
		public static Orthotope1Double Interpolate(Orthotope1Double orthotope1, Orthotope1Double orthotope2, Interpolation<double> interpolate, double fraction)
		{
			return new Orthotope1Double(Ranges.Interpolate(orthotope1.rangeX, orthotope2.rangeX, interpolate, fraction));
		}
	}
}
