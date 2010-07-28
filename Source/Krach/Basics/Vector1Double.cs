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
using Krach.Extensions;

namespace Krach.Basics
{
	public struct Vector1Double : IEquatable<Vector1Double>
	{
		readonly double x;

		public static Vector1Double Origin { get { return new Vector1Double(0); } }
		public static Vector1Double UnitX { get { return new Vector1Double(1); } }

		public double X { get { return x; } }
		public double Length { get { return LengthSquared.SquareRoot(); } }
		public double LengthSquared { get { return x.Square(); } }

		public Vector1Double(double x)
		{
			this.x = x;
		}

		public override bool Equals(object obj)
		{
			return obj is Vector1Double && this == (Vector1Double)obj;
		}
		public override int GetHashCode()
		{
			return x.GetHashCode();
		}
		public override string ToString()
		{
			return "(" + x + ")";
		}
		public bool Equals(Vector1Double other)
		{
			return this == other;
		}

		public static bool operator ==(Vector1Double vector1, Vector1Double vector2)
		{
			return vector1.x == vector2.x;
		}
		public static bool operator !=(Vector1Double vector1, Vector1Double vector2)
		{
			return vector1.x != vector2.x;
		}

		public static Vector1Double operator +(Vector1Double vector1, Vector1Double vector2)
		{
			return new Vector1Double(vector1.x + vector2.x);
		}
		public static Vector1Double operator -(Vector1Double vector1, Vector1Double vector2)
		{
			return new Vector1Double(vector1.x - vector2.x);
		}
		public static Vector1Double operator *(Vector1Double vector, double factor)
		{
			return new Vector1Double(vector.x * factor);
		}
		public static Vector1Double operator *(double factor, Vector1Double vector)
		{
			return new Vector1Double(factor * vector.x);
		}

		public static Vector1Double InterpolateLinear(Vector1Double vector1, Vector1Double vector2, double fraction)
		{
			return new Vector1Double(Scalars.InterpolateLinear(vector1.x, vector2.x, fraction));
		}
	}
}
