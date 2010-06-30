using System;
using System.Collections.Generic;
using System.Linq;

namespace Multimedia.Graphics
{
	public struct RectangleFloat : IEquatable<RectangleFloat>
	{
		readonly Vector2Float a;
		readonly Vector2Float b;

		public static RectangleFloat Empty { get { return new RectangleFloat(Vector2Float.Origin, Vector2Float.Origin); } }

		public Vector2Float A { get { return a; } }
		public Vector2Float B { get { return b; } }
		public float Left { get { return a.X; } }
		public float Top { get { return a.Y; } }
		public float Right { get { return b.X; } }
		public float Bottom { get { return b.Y; } }
		public Vector2Float Size { get { return b - a; } }
		public float Area { get { return Size.X * Size.Y; } }
		public bool IsEmpty { get { return Size.X <= 0 || Size.Y <= 0; } }

		public RectangleFloat(Vector2Float a, Vector2Float b)
		{
			this.a = a;
			this.b = b;
		}
		public RectangleFloat(float left, float top, float right, float bottom)
		{
			this.a = new Vector2Float(left, top);
			this.b = new Vector2Float(right, bottom);
		}

		public override bool Equals(object obj)
		{
			return obj is RectangleFloat && this == (RectangleFloat)obj;
		}
		public override int GetHashCode()
		{
			return a.GetHashCode() ^ b.GetHashCode();
		}
		public override string ToString()
		{
			return "(" + a + "|" + b + ")";
		}
		public bool Equals(RectangleFloat other)
		{
			return this == other;
		}
		public RectangleFloat Inflate(float size)
		{
			return new RectangleFloat(Left - size, Top - size, Right + size, Bottom + size);
		}

		public static bool operator ==(RectangleFloat a, RectangleFloat b)
		{
			return a.a == b.a && a.b == b.b;
		}
		public static bool operator !=(RectangleFloat a, RectangleFloat b)
		{
			return a.a != b.a || a.b != b.b;
		}

		public static RectangleFloat Intersect(RectangleFloat a, RectangleFloat b)
		{
			float left = Math.Max(a.Left, b.Left);
			float top = Math.Max(a.Top, b.Top);
			float right = Math.Min(a.Right, b.Right);
			float bottom = Math.Min(a.Bottom, b.Bottom);

			return new RectangleFloat(left, top, right, bottom);
		}
		public static IEnumerable<RectangleFloat> Exclude(RectangleFloat rectangle, RectangleFloat exclusion)
		{
			RectangleFloat intersection = Intersect(exclusion, rectangle);

			if (intersection.IsEmpty) yield return rectangle;
			else
			{
				RectangleFloat top = new RectangleFloat(rectangle.Left, rectangle.Top, rectangle.Right, intersection.Top);
				RectangleFloat bottom = new RectangleFloat(rectangle.Left, intersection.Bottom, rectangle.Right, rectangle.Bottom);
				RectangleFloat left = new RectangleFloat(rectangle.Left, intersection.Top, intersection.Left, intersection.Bottom);
				RectangleFloat right = new RectangleFloat(intersection.Right, intersection.Top, rectangle.Right, intersection.Bottom);

				if (!top.IsEmpty) yield return top;
				if (!bottom.IsEmpty) yield return bottom;
				if (!left.IsEmpty) yield return left;
				if (!right.IsEmpty) yield return right;
			}
		}
		public static RectangleFloat GetBounds(IEnumerable<RectangleFloat> rectangles)
		{
			float left = rectangles.Min(rectangle => rectangle.Left);
			float top = rectangles.Min(rectangle => rectangle.Top);
			float right = rectangles.Max(rectangle => rectangle.Right);
			float bottom = rectangles.Max(rectangle => rectangle.Bottom);

			return new RectangleFloat(left, top, right, bottom);
		}
		public static RectangleFloat InterpolateLinear(RectangleFloat a, RectangleFloat b, float fraction)
		{
			return new RectangleFloat(Vector2Float.InterpolateLinear(a.a, b.a, fraction), Vector2Float.InterpolateLinear(a.b, b.b, fraction));
		}
	}
}