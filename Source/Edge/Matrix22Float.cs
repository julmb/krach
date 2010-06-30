using System;

namespace Multimedia.Graphics
{
	public struct Matrix22Float : IEquatable<Matrix22Float>
	{
		readonly float x11;
		readonly float x12;
		readonly float x21;
		readonly float x22;

		public static Matrix22Float Identity { get { return new Matrix22Float(1, 0, 0, 1); } }

		public float X11 { get { return x11; } }
		public float X12 { get { return x12; } }
		public float X21 { get { return x21; } }
		public float X22 { get { return x22; } }

		public Matrix22Float(float x11, float x12, float x21, float x22)
		{
			this.x11 = x11;
			this.x12 = x12;
			this.x21 = x21;
			this.x22 = x22;
		}

		public override bool Equals(object obj)
		{
			return obj is Matrix22Float && this == (Matrix22Float)obj;
		}
		public override int GetHashCode()
		{
			return x11.GetHashCode() ^ x12.GetHashCode() ^ x21.GetHashCode() ^ x22.GetHashCode();
		}
		public override string ToString()
		{
			return string.Format("({0}, {1})\n({2}, {3})", x11, x12, x21, x22);
		}
		public bool Equals(Matrix22Float other)
		{
			return this == other;
		}
		public Matrix22Float Transpose(Matrix22Float matrix)
		{
			return new Matrix22Float(x11, x21, x12, x22);
		}

		public static bool operator ==(Matrix22Float a, Matrix22Float b)
		{
			return a.x11 == b.x11 && a.x12 == b.x12 && a.x21 == b.x21 && a.x22 == b.x22;
		}
		public static bool operator !=(Matrix22Float a, Matrix22Float b)
		{
			return a.x11 != b.x11 || a.x12 != b.x12 || a.x21 != b.x21 || a.x22 != b.x22;
		}

		public static Matrix22Float operator +(Matrix22Float a, Matrix22Float b)
		{
			return new Matrix22Float(a.x11 + b.x11, a.x12 + b.x12, a.x21 + b.x21, a.x22 + b.x22);
		}
		public static Matrix22Float operator -(Matrix22Float a, Matrix22Float b)
		{
			return new Matrix22Float(a.x11 - b.x11, a.x12 - b.x12, a.x21 - b.x21, a.x22 - b.x22);
		}
		public static Matrix22Float operator *(Matrix22Float a, float factor)
		{
			return new Matrix22Float(a.x11 * factor, a.x12 * factor, a.x21 * factor, a.x22 * factor);
		}
		public static Matrix22Float operator *(float factor, Matrix22Float a)
		{
			return new Matrix22Float(factor * a.x11, factor * a.x12, factor * a.x21, factor * a.x22);
		}
		public static Matrix22Float operator *(Matrix22Float a, Matrix22Float b)
		{
			return new Matrix22Float
			(
				a.x11 * b.x11 + a.x12 * b.x21, a.x11 * b.x12 + a.x12 * b.x22,
				a.x21 * b.x11 + a.x22 * b.x21, a.x21 * b.x12 + a.x22 * b.x22
			);
		}
		public static Vector2Float operator *(Matrix22Float a, Vector2Float v)
		{
			return new Vector2Float(a.x11 * v.X + a.x12 * v.Y, a.x21 * v.X + a.x22 * v.Y);
		}
	}
}
