using System;
using System.Drawing;

namespace Utility.Utilities
{
	public static class RectangleUtility
	{
		public static Rectangle Intersect(Rectangle rectangle1, Rectangle rectangle2)
		{
			int left = Math.Max(rectangle1.Left, rectangle2.Left);
			int top = Math.Max(rectangle1.Top, rectangle2.Top);
			int right = Math.Min(rectangle1.Right, rectangle2.Right);
			int bottom = Math.Min(rectangle1.Bottom, rectangle2.Bottom);

			return new Rectangle(left, top, right - left, bottom - top);
		}
	}
}
