using System;
using Utility.Extensions;
using Utility.Utilities;
using Dash.Extensions;

namespace Edge.Graphics
{
	public struct HsvColor
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
			double value = Scalars.Maximum(color.Red, color.Green, color.Blue);
			double chroma = value - Scalars.Minimum(color.Red, color.Green, color.Blue);

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
	}
}
