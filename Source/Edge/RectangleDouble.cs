using System;
using System.Collections.Generic;
using System.Linq;

namespace Edge
{
	public struct RectangleDouble : IEquatable<RectangleDouble>
	{
		readonly Vector2Double a;
		readonly Vector2Double b;

		public static RectangleDouble Empty { get { return new RectangleDouble(Vector2Double.Origin, Vector2Double.Origin); } }

		public Vector2Double A { get { return a; } }
		public Vector2Double B { get { return b; } }
		public double Left { get { return a.X; } }
		public double Top { get { return a.Y; } }
		public double Right { get { return b.X; } }
		public double Bottom { get { return b.Y; } }
		public Vector2Double Size { get { return b - a; } }
		public double Area { get { return Size.X * Size.Y; } }
		public bool IsEmpty { get { return Size.X <= 0 || Size.Y <= 0; } }

		public RectangleDouble(Vector2Double a, Vector2Double b)
		{
			this.a = a;
			this.b = b;
		}
		public RectangleDouble(double left, double top, double right, double bottom)
		{
			this.a = new Vector2Double(left, top);
			this.b = new Vector2Double(right, bottom);
		}

		public override bool Equals(object obj)
		{
			return obj is RectangleDouble && this == (RectangleDouble)obj;
		}
		public override int GetHashCode()
		{
			return a.GetHashCode() ^ b.GetHashCode();
		}
		public override string ToString()
		{
			return "(" + a + "|" + b + ")";
		}
		public bool Equals(RectangleDouble other)
		{
			return this == other;
		}
		public RectangleDouble Inflate(double size)
		{
			return new RectangleDouble(Left - size, Top - size, Right + size, Bottom + size);
		}

		public static bool operator ==(RectangleDouble a, RectangleDouble b)
		{
			return a.a == b.a && a.b == b.b;
		}
		public static bool operator !=(RectangleDouble a, RectangleDouble b)
		{
			return a.a != b.a || a.b != b.b;
		}

		public static RectangleDouble Intersect(RectangleDouble a, RectangleDouble b)
		{
			double left = Math.Max(a.Left, b.Left);
			double top = Math.Max(a.Top, b.Top);
			double right = Math.Min(a.Right, b.Right);
			double bottom = Math.Min(a.Bottom, b.Bottom);

			return new RectangleDouble(left, top, right, bottom);
		}
		public static IEnumerable<RectangleDouble> Exclude(RectangleDouble rectangle, RectangleDouble exclusion)
		{
			RectangleDouble intersection = Intersect(exclusion, rectangle);

			if (intersection.IsEmpty) yield return rectangle;
			else
			{
				RectangleDouble top = new RectangleDouble(rectangle.Left, rectangle.Top, rectangle.Right, intersection.Top);
				RectangleDouble bottom = new RectangleDouble(rectangle.Left, intersection.Bottom, rectangle.Right, rectangle.Bottom);
				RectangleDouble left = new RectangleDouble(rectangle.Left, intersection.Top, intersection.Left, intersection.Bottom);
				RectangleDouble right = new RectangleDouble(intersection.Right, intersection.Top, rectangle.Right, intersection.Bottom);

				if (!top.IsEmpty) yield return top;
				if (!bottom.IsEmpty) yield return bottom;
				if (!left.IsEmpty) yield return left;
				if (!right.IsEmpty) yield return right;
			}
		}
		public static RectangleDouble GetBounds(IEnumerable<RectangleDouble> rectangles)
		{
			double left = rectangles.Min(rectangle => rectangle.Left);
			double top = rectangles.Min(rectangle => rectangle.Top);
			double right = rectangles.Max(rectangle => rectangle.Right);
			double bottom = rectangles.Max(rectangle => rectangle.Bottom);

			return new RectangleDouble(left, top, right, bottom);
		}
		public static RectangleDouble InterpolateLinear(RectangleDouble a, RectangleDouble b, double fraction)
		{
			return new RectangleDouble(Vector2Double.InterpolateLinear(a.a, b.a, fraction), Vector2Double.InterpolateLinear(a.b, b.b, fraction));
		}
	}
}