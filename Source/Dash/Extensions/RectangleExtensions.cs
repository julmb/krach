using System;
using System.Drawing;

namespace Utility.Extensions
{
	public static class RectangleExtension
	{
		public static Rectangle Absolute(this Rectangle rectangle)
		{
			int x1 = Math.Min(rectangle.Left, rectangle.Right);
			int y1 = Math.Min(rectangle.Top, rectangle.Bottom);
			int x2 = Math.Max(rectangle.Left, rectangle.Right);
			int y2 = Math.Max(rectangle.Top, rectangle.Bottom);

			return new Rectangle(x1, y1, x2 - x1, y2 - y1);
		}
	}
}
