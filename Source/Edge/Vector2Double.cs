using System;
using Utilities;

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
		public double Length { get { return Math.Sqrt(x * x + y * y); } }
		public double LengthSquared { get { return x * x + y * y; } }

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

		public static bool operator ==(Vector2Double u, Vector2Double v)
		{
			return u.x == v.x && u.y == v.y;
		}
		public static bool operator !=(Vector2Double u, Vector2Double v)
		{
			return u.x != v.x || u.y != v.y;
		}

		public static Vector2Double operator +(Vector2Double u, Vector2Double v)
		{
			return new Vector2Double(u.x + v.x, u.y + v.y);
		}
		public static Vector2Double operator -(Vector2Double u, Vector2Double v)
		{
			return new Vector2Double(u.x - v.x, u.y - v.y);
		}
		public static Vector2Double operator *(Vector2Double v, double factor)
		{
			return new Vector2Double(v.x * factor, v.y * factor);
		}
		public static Vector2Double operator *(double factor, Vector2Double v)
		{
			return new Vector2Double(factor * v.x, factor * v.y);
		}

		public static Vector2Double InterpolateLinear(Vector2Double u, Vector2Double v, double fraction)
		{
			return new Vector2Double(MathUtilities.InterpolateLinear(u.x, v.x, fraction), MathUtilities.InterpolateLinear(u.y, v.y, fraction));
		}
	}
}
