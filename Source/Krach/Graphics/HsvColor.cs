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
	struct HsvColor
	{
		readonly double hue;
		readonly double saturation;
		readonly double value;

		public double Hue { get { return hue; } }
		public double Saturation { get { return saturation; } }
		public double Value { get { return value; } }

		public HsvColor(double hue, double saturation, double value)
		{
			if (hue < 0 || hue >= 6) throw new ArgumentOutOfRangeException("hue");
			if (saturation < 0 || saturation > 1) throw new ArgumentOutOfRangeException("saturation");
			if (value < 0 || value > 1) throw new ArgumentOutOfRangeException("value");

			this.hue = hue;
			this.saturation = saturation;
			this.value = value;
		}

		public static HsvColor FromRgb(RgbColor color)
		{
			double value = Comparables.Maximum(color.Red, color.Green, color.Blue);
			double chroma = value - Comparables.Minimum(color.Red, color.Green, color.Blue);

			if (chroma == 0) return new HsvColor(0, 0, value);

			double hue = 0;

			if (value == color.Red) hue = (color.Green - color.Blue) / chroma + 0;
			if (value == color.Green) hue = (color.Blue - color.Red) / chroma + 2;
			if (value == color.Blue) hue = (color.Red - color.Green) / chroma + 4;

			return new HsvColor(hue.Modulo(6), chroma / value, value);
		}
		public static double Distance(HsvColor color1, HsvColor color2)
		{
			double hue = color1.hue - color2.hue;
			double saturation = color1.saturation - color2.saturation;
			double value = color1.value - color2.value;

			return (hue.Square() + saturation.Square() + value.Square()).SquareRoot();
		}
		public static HsvColor Interpolate(HsvColor color1, HsvColor color2, Interpolation<double> interpolate, double fraction)
		{
			if (interpolate == null) throw new ArgumentNullException("interpolate");
			if (fraction < 0 || fraction > 1) throw new ArgumentOutOfRangeException("fraction");

			return new HsvColor
			(
				interpolate(color1.hue, color2.hue, fraction),
				interpolate(color1.saturation, color2.saturation, fraction),
				interpolate(color1.value, color2.value, fraction)
			);
		}
	}
}
