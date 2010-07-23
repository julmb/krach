using System;
using Utilities;

namespace Edge
{
	public struct Vector1Double : IEquatable<Vector1Double>
	{
		readonly double x;

		public static Vector1Double Origin { get { return new Vector1Double(0); } }
		public static Vector1Double UnitX { get { return new Vector1Double(1); } }

		public double X { get { return x; } }
		public double Length { get { return Math.Sqrt(x * x); } }
		public double LengthSquared { get { return x * x; } }

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

		public static bool operator ==(Vector1Double u, Vector1Double v)
		{
			return u.x == v.x;
		}
		public static bool operator !=(Vector1Double u, Vector1Double v)
		{
			return u.x != v.x;
		}

		public static Vector1Double operator +(Vector1Double u, Vector1Double v)
		{
			return new Vector1Double(u.x + v.x);
		}
		public static Vector1Double operator -(Vector1Double u, Vector1Double v)
		{
			return new Vector1Double(u.x - v.x);
		}
		public static Vector1Double operator *(Vector1Double v, double factor)
		{
			return new Vector1Double(v.x * factor);
		}
		public static Vector1Double operator *(double factor, Vector1Double v)
		{
			return new Vector1Double(factor * v.x);
		}

		public static Vector1Double InterpolateLinear(Vector1Double u, Vector1Double v, double fraction)
		{
			return new Vector1Double(MathUtilities.InterpolateLinear(u.x, v.x, fraction));
		}
	}
}
