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
			return new Vector1Double(MathUtilities.InterpolateLinear(vector1.x, vector2.x, fraction));
		}
	}
}
