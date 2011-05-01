// Copyright © Julian Brunner 2010 - 2011

// This file is part of Krach.
//
// Krach is free software: you can redistribute it and/or modify it under the
// terms of the GNU Lesser General Public License as published by the Free
// Software Foundation, either version 3 of the License, or (at your option) any
// later version.
//
// Krach is distributed in the hope that it will be useful, but WITHOUT ANY
// WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
// A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with
// Krach. If not, see <http://www.gnu.org/licenses/>.

using System;
using Krach.Extensions;

namespace Krach.Graphics
{
	struct RgbColor
	{
		readonly double red;
		readonly double green;
		readonly double blue;

		public double Red { get { return red; } }
		public double Green { get { return green; } }
		public double Blue { get { return blue; } }

		public RgbColor(double red, double green, double blue)
		{
			if (red < 0 || red > 1) throw new ArgumentOutOfRangeException("red");
			if (green < 0 || green > 1) throw new ArgumentOutOfRangeException("green");
			if (blue < 0 || blue > 1) throw new ArgumentOutOfRangeException("blue");

			this.red = red;
			this.green = green;
			this.blue = blue;
		}

		public static RgbColor FromHsv(HsvColor color)
		{
			int hueIndex = (int)color.Hue;
			double hueFraction = color.Hue - hueIndex;

			double bottom = color.Value * (1 - color.Saturation);
			double top = color.Value;
			double rising = color.Value * (1 - (1 - hueFraction) * color.Saturation);
			double falling = color.Value * (1 - hueFraction * color.Saturation);

			switch (hueIndex)
			{
				case 0: return new RgbColor(top, rising, bottom);
				case 1: return new RgbColor(falling, top, bottom);
				case 2: return new RgbColor(bottom, top, rising);
				case 3: return new RgbColor(bottom, falling, top);
				case 4: return new RgbColor(rising, bottom, top);
				case 5: return new RgbColor(top, bottom, falling);
				default: throw new InvalidOperationException();
			}
		}
		public static double Distance(RgbColor color1, RgbColor color2)
		{
			double red = color1.red - color2.red;
			double green = color1.green - color2.green;
			double blue = color1.blue - color2.blue;

			return (red.Square() + green.Square() + blue.Square()).SquareRoot();
		}
		public static RgbColor Interpolate(RgbColor color1, RgbColor color2, Interpolation<double> interpolate, double fraction)
		{
			if (interpolate == null) throw new ArgumentNullException("interpolate");
			if (fraction < 0 || fraction > 1) throw new ArgumentOutOfRangeException("fraction");

			return new RgbColor
			(
				interpolate(color1.red, color2.red, fraction),
				interpolate(color1.green, color2.green, fraction),
				interpolate(color1.blue, color2.blue, fraction)
			);
		}
	}
}
