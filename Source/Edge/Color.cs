using System;

namespace Multimedia.Graphics
{
	public struct Color
	{
		readonly byte red;
		readonly byte green;
		readonly byte blue;
		readonly byte alpha;

		public byte RedByte { get { return red; } }
		public byte GreenByte { get { return green; } }
		public byte BlueByte { get { return blue; } }
		public byte AlphaByte { get { return alpha; } }

		public float Red { get { return red / 0xFFf; } }
		public float Green { get { return green / 0xFFf; } }
		public float Blue { get { return blue / 0xFFf; } }
		public float Alpha { get { return alpha / 0xFFf; } }

		public Color(byte red, byte green, byte blue, byte alpha)
		{
			this.red = red;
			this.green = green;
			this.blue = blue;
			this.alpha = alpha;
		}
		public Color(byte red, byte green, byte blue) : this(red, green, blue, 0xFF) { }

		public static Color FromRgba(float red, float green, float blue, float alpha)
		{
			if (red < 0 || red > 1) throw new ArgumentOutOfRangeException("red");
			if (green < 0 || green > 1) throw new ArgumentOutOfRangeException("green");
			if (blue < 0 || blue > 1) throw new ArgumentOutOfRangeException("blue");
			if (alpha < 0 || alpha > 1) throw new ArgumentOutOfRangeException("alpha");

			return new Color((byte)(red * 0xFF), (byte)(green * 0xFF), (byte)(blue * 0xFF), (byte)(alpha * 0xFF));
		}
		public static Color FromRgb(float red, float green, float blue)
		{
			return Color.FromRgba(red, green, blue, 1);
		}
		public static Color FromHsva(float hue, float saturation, float value, float alpha)
		{
			if (hue < 0 || hue >= 6) throw new ArgumentOutOfRangeException("hue");
			if (saturation < 0 || saturation > 1) throw new ArgumentOutOfRangeException("saturation");
			if (value < 0 || value > 1) throw new ArgumentOutOfRangeException("value");
			if (alpha < 0 || alpha > 1) throw new ArgumentOutOfRangeException("alpha");

			int hueIndex = (int)hue;
			float hueFraction = hue - hueIndex;

			float bottom = value * (1 - saturation);
			float top = value;
			float rising = value * (1 - (1 - hueFraction) * saturation);
			float falling = value * (1 - hueFraction * saturation);

			float red, green, blue;

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

			return Color.FromRgba(red, green, blue, alpha);
		}
		public static Color FromHsv(float hue, float saturation, float value)
		{
			return Color.FromHsva(hue, saturation, value, 1);
		}
	}
}
