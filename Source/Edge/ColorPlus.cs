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

namespace Utility
{
	struct ColorPlus
	{
		readonly double red;
		readonly double green;
		readonly double blue;

		readonly double hue;
		readonly double saturation;
		readonly double value;

		public double Red { get { return red; } }
		public double Green { get { return green; } }
		public double Blue { get { return blue; } }

		public double Hue { get { return hue; } }
		public double Saturation { get { return saturation; } }
		public double Value { get { return value; } }

		ColorPlus(double red, double green, double blue, double hue, double saturation, double value)
		{
			if (red < 0 || red > 1) throw new ArgumentOutOfRangeException("red");
			if (green < 0 || green > 1) throw new ArgumentOutOfRangeException("green");
			if (blue < 0 || blue > 1) throw new ArgumentOutOfRangeException("blue");

			if (hue < 0 || hue >= 6) throw new ArgumentOutOfRangeException("hue");
			if (saturation < 0 || saturation > 1) throw new ArgumentOutOfRangeException("saturation");
			if (value < 0 || value > 1) throw new ArgumentOutOfRangeException("value");

			this.red = red;
			this.green = green;
			this.blue = blue;

			this.hue = hue;
			this.saturation = saturation;
			this.value = value;
		}

		public Color ToColor()
		{
			return Color.FromArgb(ToByte(red), ToByte(green), ToByte(blue));
		}

		public static ColorPlus FromHsv(double hue, double saturation, double value)
		{
			if (hue < 0 || hue >= 6) throw new ArgumentOutOfRangeException("hue");
			if (saturation < 0 || saturation > 1) throw new ArgumentOutOfRangeException("saturation");
			if (value < 0 || value > 1) throw new ArgumentOutOfRangeException("value");

			int hueIndex = (int)hue;
			double hueFraction = hue - hueIndex;

			double bottom = value * (1 - saturation);
			double top = value;
			double rising = value * (1 - (1 - hueFraction) * saturation);
			double falling = value * (1 - hueFraction * saturation);

			double red, green, blue;

			switch (hueIndex)
			{
				case 0: red = top; green = rising; blue = bottom; break;
				case 1: red = falling; green = top; blue = bottom; break;
				case 2: red = bottom; green = top; blue = rising; break;
				case 3: red = bottom; green = falling; blue = top; break;
				case 4: red = rising; green = bottom; blue = top; break;
				case 5: red = top; green = bottom; blue = falling; break;
				default: throw new InvalidOperationException();
			}

			return new ColorPlus(red, green, blue, hue, saturation, value);
		}
		public static double Distance(ColorPlus color1, ColorPlus color2)
		{
			double red = color1.red - color2.red;
			double green = color1.green - color2.green;
			double blue = color1.blue - color2.blue;

			return Math.Sqrt(red * red + green * green + blue * blue);
		}

		static byte ToByte(double value)
		{
			if (value < 0 || value > 1) throw new ArgumentOutOfRangeException("value");

			return value == 1 ? (byte)0xFF : (byte)(value * 0x100);
		}
	}
}
