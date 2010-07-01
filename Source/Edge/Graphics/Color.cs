using System;
using Utility.Extensions;

namespace Edge.Graphics
{
	public struct Color
	{
		readonly double red;
		readonly double green;
		readonly double blue;
		readonly double alpha;

		public double Red { get { return red; } }
		public double Green { get { return green; } }
		public double Blue { get { return blue; } }

		//public double Hue { get { return hue; } }
		//public double Saturation { get { return saturation; } }
		//public double Value { get { return value; } }

		public double Alpha { get { return alpha; } }

		Color(double red, double green, double blue, double alpha)
		{
			this.red = red;
			this.green = green;
			this.blue = blue;
			this.alpha = alpha;
		}

		public static Color FromRgba(double red, double green, double blue, double alpha)
		{
			if (red < 0 || red > 1) throw new ArgumentOutOfRangeException("red");
			if (green < 0 || green > 1) throw new ArgumentOutOfRangeException("green");
			if (blue < 0 || blue > 1) throw new ArgumentOutOfRangeException("blue");
			if (alpha < 0 || alpha > 1) throw new ArgumentOutOfRangeException("alpha");

			return new Color(red, green, blue, alpha);
		}
		public static Color FromRgb(double red, double green, double blue)
		{
			return Color.FromRgba(red, green, blue, 1);
		}
		public static Color FromHsva(double hue, double saturation, double value, double alpha)
		{
			if (hue < 0 || hue >= 6) throw new ArgumentOutOfRangeException("hue");
			if (saturation < 0 || saturation > 1) throw new ArgumentOutOfRangeException("saturation");
			if (value < 0 || value > 1) throw new ArgumentOutOfRangeException("value");
			if (alpha < 0 || alpha > 1) throw new ArgumentOutOfRangeException("alpha");

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

			return new Color(red, green, blue, alpha);
		}
		public static Color FromHsv(double hue, double saturation, double value)
		{
			return Color.FromHsva(hue, saturation, value, 1);
		}
		public static double Distance(Color color1, Color color2)
		{
			double red = color1.red - color2.red;
			double green = color1.green - color2.green;
			double blue = color1.blue - color2.blue;
			double alpha = color1.alpha - color2.alpha;

			return Math.Sqrt(red.Square() + green.Square() + blue.Square() + alpha.Square());
		}
	}
}
