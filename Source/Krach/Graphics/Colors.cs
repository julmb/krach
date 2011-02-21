// Copyright © Julian Brunner 2009 - 2011

// This file is part of Stream Visualizer (streamvis).
// 
// Stream Visualizer is free software: you can redistribute it and/or modify it
// under the terms of the GNU General Public License as published by the Free
// Software Foundation, either version 3 of the License, or (at your option) any
// later version.
// 
// Stream Visualizer is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more
// details.
// 
// You should have received a copy of the GNU General Public License along with
// Stream Visualizer. If not, see <http://www.gnu.org/licenses/>.

namespace Krach.Graphics
{
	public static class Colors
	{
		public static Color Transparent { get { return Color.FromHsva(0, 0, 0, 0); } }
		public static Color Black { get { return Color.FromHsv(0, 0, 0); } }
		public static Color White { get { return Color.FromHsv(0, 0, 1); } }
		public static Color Red { get { return Color.FromHsv(0, 1, 1); } }
		public static Color Yellow { get { return Color.FromHsv(1, 1, 1); } }
		public static Color Green { get { return Color.FromHsv(2, 1, 1); } }
		public static Color Cyan { get { return Color.FromHsv(3, 1, 1); } }
		public static Color Blue { get { return Color.FromHsv(4, 1, 1); } }
		public static Color Magenta { get { return Color.FromHsv(5, 1, 1); } }
	}
}
