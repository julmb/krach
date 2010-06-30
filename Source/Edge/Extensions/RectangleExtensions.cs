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
