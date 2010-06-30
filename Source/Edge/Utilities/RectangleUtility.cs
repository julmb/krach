// Copyright © Julian Brunner 2009 - 2010

// This file is part of Stream Visualizer (streamvis).
// 
// Stream Visualizer is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Stream Visualizer is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Stream Visualizer.  If not, see <http://www.gnu.org/licenses/>.

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
