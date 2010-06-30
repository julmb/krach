using System;
using Utilities;

namespace Multimedia.Graphics
{
	public struct Vector2Float : IEquatable<Vector2Float>
	{
		readonly float x;
		readonly float y;

		public static Vector2Float Origin { get { return new Vector2Float(0, 0); } }
		public static Vector2Float UnitX { get { return new Vector2Float(1, 0); } }
		public static Vector2Float UnitY { get { return new Vector2Float(0, 1); } }
		public static Vector2Float UnitXY { get { return new Vector2Float(1, 1); } }

		public float X { get { return x; } }
		public float Y { get { return y; } }
		public float Length { get { return (float)Math.Sqrt(x * x + y * y); } }
		public float LengthSquared { get { return x * x + y * y; } }

		public Vector2Float(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public override bool Equals(object obj)
		{
			return obj is Vector2Float && this == (Vector2Float)obj;
		}
		public override int GetHashCode()
		{
			return x.GetHashCode() ^ y.GetHashCode();
		}
		public override string ToString()
		{
			return "(" + x + "," + y + ")";
		}
		public bool Equals(Vector2Float other)
		{
			return this == other;
		}

		public static bool operator ==(Vector2Float u, Vector2Float v)
		{
			return u.x == v.x && u.y == v.y;
		}
		public static bool operator !=(Vector2Float u, Vector2Float v)
		{
			return u.x != v.x || u.y != v.y;
		}

		public static Vector2Float operator +(Vector2Float u, Vector2Float v)
		{
			return new Vector2Float(u.x + v.x, u.y + v.y);
		}
		public static Vector2Float operator -(Vector2Float u, Vector2Float v)
		{
			return new Vector2Float(u.x - v.x, u.y - v.y);
		}
		public static Vector2Float operator *(Vector2Float v, float factor)
		{
			return new Vector2Float(v.x * factor, v.y * factor);
		}
		public static Vector2Float operator *(float factor, Vector2Float v)
		{
			return new Vector2Float(factor * v.x, factor * v.y);
		}

		public static Vector2Float InterpolateLinear(Vector2Float u, Vector2Float v, float fraction)
		{
			return new Vector2Float(MathUtilities.InterpolateLinear(u.x, v.x, fraction), MathUtilities.InterpolateLinear(u.y, v.y, fraction));
		}
	}
}
