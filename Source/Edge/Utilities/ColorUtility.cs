using System;
using System.Drawing;
using System.Globalization;

namespace Utility.Utilities
{
	public static class ColorUtility
	{
		public static Color FromHtmlString(string htmlString)
		{
			if (htmlString.Length != 6) throw new ArgumentOutOfRangeException("htmlString");

			try
			{
				byte red = byte.Parse(htmlString.Substring(0, 2), NumberStyles.HexNumber);
				byte green = byte.Parse(htmlString.Substring(2, 2), NumberStyles.HexNumber);
				byte blue = byte.Parse(htmlString.Substring(4, 2), NumberStyles.HexNumber);
				return Color.FromArgb(red, green, blue);
			}
			catch (FormatException) { throw new ArgumentOutOfRangeException("htmlString"); }
		}
	}
}
