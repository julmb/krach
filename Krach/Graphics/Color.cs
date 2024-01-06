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
using System.Globalization;
using Krach.Basics;

namespace Krach.Graphics
{
	public struct Color
	{
		readonly RgbColor rgbColor;
		readonly HsvColor hsvColor;
		readonly double alpha;

		public double Red { get { return rgbColor.Red; } }
		public double Green { get { return rgbColor.Green; } }
		public double Blue { get { return rgbColor.Blue; } }

		public double Hue { get { return hsvColor.Hue; } }
		public double Saturation { get { return hsvColor.Saturation; } }
		public double Value { get { return hsvColor.Value; } }

		public double Alpha { get { return alpha; } }

		public Color Inverse { get { return new Color(new RgbColor(1 - rgbColor.Red, 1 - rgbColor.Green, 1 - rgbColor.Blue), alpha); } }

		Color(RgbColor rgbColor, HsvColor hsvColor, double alpha)
		{
			this.rgbColor = rgbColor;
			this.hsvColor = hsvColor;
			this.alpha = alpha;
		}
		Color(RgbColor color, double alpha)
		{
			if (alpha < 0 || alpha > 1) throw new ArgumentOutOfRangeException("alpha");

			this.rgbColor = color;
			this.hsvColor = HsvColor.FromRgb(rgbColor);
			this.alpha = alpha;
		}
		Color(RgbColor color) : this(color, 1) { }
		Color(HsvColor color, double alpha)
		{
			if (alpha < 0 || alpha > 1) throw new ArgumentOutOfRangeException("alpha");

			this.rgbColor = RgbColor.FromHsv(color);
			this.hsvColor = color;
			this.alpha = alpha;
		}
		Color(HsvColor color) : this(color, 1) { }

		public override string ToString()
		{
			return ToHtmlString(this);
		}
		public Color ReplaceAlpha(double alpha)
		{
			return Color.FromRgba(Red, Green, Blue, alpha);
		}

		public static Color FromRgba(double red, double green, double blue, double alpha)
		{
			return new Color(new RgbColor(red, green, blue), alpha);
		}
		public static Color FromRgb(double red, double green, double blue)
		{
			return new Color(new RgbColor(red, green, blue));
		}
		public static Color FromHsva(double hue, double saturation, double value, double alpha)
		{
			return new Color(new HsvColor(hue, saturation, value), alpha);
		}
		public static Color FromHsv(double hue, double saturation, double value)
		{
			return new Color(new HsvColor(hue, saturation, value));
		}
		public static Color FromArgbBinary(uint binary)
		{
			byte alpha = (byte)((binary & 0xFF000000) >> 24);
			byte red = (byte)((binary & 0x00FF0000) >> 16);
			byte green = (byte)((binary & 0x0000FF00) >> 8);
			byte blue = (byte)((binary & 0x000000FF) >> 0);
			return Color.FromRgba(red / 255.0, green / 255.0, blue / 255.0, alpha / 255.0);
		}
		public static Color FromRgbaBinary(uint binary)
		{
			byte red = (byte)((binary & 0xFF000000) >> 24);
			byte green = (byte)((binary & 0x00FF0000) >> 16);
			byte blue = (byte)((binary & 0x0000FF00) >> 8);
			byte alpha = (byte)((binary & 0x000000FF) >> 0);
			return Color.FromRgba(red / 255.0, green / 255.0, blue / 255.0, alpha / 255.0);
		}
		public static Color FromHtmlString(string text)
		{
			if (text.Length != 6) throw new ArgumentException("Parameter 'text' must be exactly 6 characters long.");

			try
			{
				byte red = byte.Parse(text.Substring(0, 2), NumberStyles.HexNumber);
				byte green = byte.Parse(text.Substring(2, 2), NumberStyles.HexNumber);
				byte blue = byte.Parse(text.Substring(4, 2), NumberStyles.HexNumber);

				return Color.FromRgb((double)red / 0xFF, (double)green / 0xFF, (double)blue / 0xFF);
			}
			catch (FormatException) { throw new ArgumentException("Parameter 'text' contains invalid characters."); }
		}
		public static string ToHtmlString(Color color)
		{
			return
				((byte)(color.Red * 0xFF)).ToString("X2") +
				((byte)(color.Green * 0xFF)).ToString("X2") +
				((byte)(color.Blue * 0xFF)).ToString("X2");
		}
		public static double DistanceRgb(Color color1, Color color2)
		{
			return RgbColor.Distance(color1.rgbColor, color2.rgbColor);
		}
		public static double DistanceHsv(Color color1, Color color2)
		{
			return HsvColor.Distance(color1.hsvColor, color2.hsvColor);
		}
		public static Color InterpolateRgb(Color color1, Color color2, Interpolation<double> interpolate, double fraction)
		{
			return new Color(RgbColor.Interpolate(color1.rgbColor, color2.rgbColor, interpolate, fraction), interpolate(color1.alpha, color2.alpha, fraction));
		}
		public static Color InterpolateHsv(Color color1, Color color2, Interpolation<double> interpolate, double fraction, Direction direction)
		{
			return new Color(HsvColor.Interpolate(color1.hsvColor, color2.hsvColor, interpolate, fraction, direction), interpolate(color1.alpha, color2.alpha, fraction));
		}
	}
}
