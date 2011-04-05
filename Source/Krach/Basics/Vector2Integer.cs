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

namespace Krach.Basics
{
	public struct Vector2Integer : IEquatable<Vector2Integer>
	{
		readonly int x;
		readonly int y;

		public static Vector2Integer Origin { get { return new Vector2Integer(0, 0); } }
		public static Vector2Integer UnitX { get { return new Vector2Integer(1, 0); } }
		public static Vector2Integer UnitY { get { return new Vector2Integer(0, 1); } }
		public static Vector2Integer UnitXY { get { return new Vector2Integer(1, 1); } }

		public int X { get { return x; } }
		public int Y { get { return y; } }

		public Vector2Integer(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public override bool Equals(object obj)
		{
			return obj is Vector2Integer && this == (Vector2Integer)obj;
		}
		public override int GetHashCode()
		{
			return x.GetHashCode() ^ y.GetHashCode();
		}
		public override string ToString()
		{
			return "(" + x + "," + y + ")";
		}
		public bool Equals(Vector2Integer other)
		{
			return this == other;
		}

		public static bool operator ==(Vector2Integer vector1, Vector2Integer vector2)
		{
			return vector1.x == vector2.x && vector1.y == vector2.y;
		}
		public static bool operator !=(Vector2Integer vector1, Vector2Integer vector2)
		{
			return vector1.x != vector2.x || vector1.y != vector2.y;
		}

		public static implicit operator Vector2Double(Vector2Integer vector)
		{
			return new Vector2Double(vector.x, vector.y);
		}

		public static Vector2Integer operator -(Vector2Integer vector)
		{
			return new Vector2Integer(-vector.x, -vector.y);
		}
		public static Vector2Integer operator +(Vector2Integer vector)
		{
			return new Vector2Integer(+vector.x, +vector.y);
		}
		public static Vector2Integer operator +(Vector2Integer vector1, Vector2Integer vector2)
		{
			return new Vector2Integer(vector1.x + vector2.x, vector1.y + vector2.y);
		}
		public static Vector2Integer operator -(Vector2Integer vector1, Vector2Integer vector2)
		{
			return new Vector2Integer(vector1.x - vector2.x, vector1.y - vector2.y);
		}
		public static Vector2Integer operator *(Vector2Integer vector, int factor)
		{
			return new Vector2Integer(vector.x * factor, vector.y * factor);
		}
		public static Vector2Integer operator *(int factor, Vector2Integer vector)
		{
			return new Vector2Integer(factor * vector.x, factor * vector.y);
		}
	}
}
