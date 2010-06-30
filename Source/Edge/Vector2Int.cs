using System;

namespace Multimedia.Graphics
{
	public struct Vector2Int : IEquatable<Vector2Int>
	{
		readonly int x;
		readonly int y;

		public static Vector2Int Origin { get { return new Vector2Int(0, 0); } }
		public static Vector2Int UnitX { get { return new Vector2Int(1, 0); } }
		public static Vector2Int UnitY { get { return new Vector2Int(0, 1); } }
		public static Vector2Int UnitXY { get { return new Vector2Int(1, 1); } }

		public int X { get { return x; } }
		public int Y { get { return y; } }

		public Vector2Int(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public override bool Equals(object obj)
		{
			return obj is Vector2Int && this == (Vector2Int)obj;
		}
		public override int GetHashCode()
		{
			return x.GetHashCode() ^ y.GetHashCode();
		}
		public override string ToString()
		{
			return "(" + x + "," + y + ")";
		}
		public bool Equals(Vector2Int other)
		{
			return this == other;
		}

		public static bool operator ==(Vector2Int u, Vector2Int v)
		{
			return u.x == v.x && u.y == v.y;
		}
		public static bool operator !=(Vector2Int u, Vector2Int v)
		{
			return u.x != v.x || u.y != v.y;
		}

		public static Vector2Int operator +(Vector2Int u, Vector2Int v)
		{
			return new Vector2Int(u.x + v.x, u.y + v.y);
		}
		public static Vector2Int operator -(Vector2Int u, Vector2Int v)
		{
			return new Vector2Int(u.x - v.x, u.y - v.y);
		}
		public static Vector2Int operator *(Vector2Int v, int factor)
		{
			return new Vector2Int(v.x * factor, v.y * factor);
		}
		public static Vector2Int operator *(int factor, Vector2Int v)
		{
			return new Vector2Int(factor * v.x, factor * v.y);
		}

		public static implicit operator Vector2Float(Vector2Int v)
		{
			return new Vector2Float(v.x, v.y);
		}
	}
}
