using System;
using Utilities;
using Dash.Extensions;

namespace Edge
{
	public struct Vector2Double : IEquatable<Vector2Double>
	{
		readonly double x;
		readonly double y;

		public static Vector2Double Origin { get { return new Vector2Double(0, 0); } }
		public static Vector2Double UnitX { get { return new Vector2Double(1, 0); } }
		public static Vector2Double UnitY { get { return new Vector2Double(0, 1); } }
		public static Vector2Double UnitXY { get { return new Vector2Double(1, 1); } }

		public double X { get { return x; } }
		public double Y { get { return y; } }
		public double Length { get { return LengthSquared.SquareRoot(); } }
		public double LengthSquared { get { return x.Square() + y.Square(); } }

		public Vector2Double(double x, double y)
		{
			this.x = x;
			this.y = y;
		}

		public override bool Equals(object obj)
		{
			return obj is Vector2Double && this == (Vector2Double)obj;
		}
		public override int GetHashCode()
		{
			return x.GetHashCode() ^ y.GetHashCode();
		}
		public override string ToString()
		{
			return "(" + x + "," + y + ")";
		}
		public bool Equals(Vector2Double other)
		{
			return this == other;
		}

		public static bool operator ==(Vector2Double vector1, Vector2Double vector2)
		{
			return vector1.x == vector2.x && vector1.y == vector2.y;
		}
		public static bool operator !=(Vector2Double vector1, Vector2Double vector2)
		{
			return vector1.x != vector2.x || vector1.y != vector2.y;
		}

		public static Vector2Double operator +(Vector2Double vector1, Vector2Double vector2)
		{
			return new Vector2Double(vector1.x + vector2.x, vector1.y + vector2.y);
		}
		public static Vector2Double operator -(Vector2Double vector1, Vector2Double vector2)
		{
			return new Vector2Double(vector1.x - vector2.x, vector1.y - vector2.y);
		}
		public static Vector2Double operator *(Vector2Double vector, double factor)
		{
			return new Vector2Double(vector.x * factor, vector.y * factor);
		}
		public static Vector2Double operator *(double factor, Vector2Double vector)
		{
			return new Vector2Double(factor * vector.x, factor * vector.y);
		}

		public static Vector2Double InterpolateLinear(Vector2Double vector1, Vector2Double vector2, double fraction)
		{
			return new Vector2Double(Scalar.InterpolateLinear(vector1.x, vector2.x, fraction), Scalar.InterpolateLinear(vector1.y, vector2.y, fraction));
		}
	}
}
