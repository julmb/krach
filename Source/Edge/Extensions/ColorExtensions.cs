using System.Drawing;

namespace Utility.Extensions
{
	public static class ColorExtesions
	{
		public static Color Invert(this Color color)
		{
			return Color.FromArgb(0xFF - color.R, 0xFF - color.G, 0xFF - color.B);
		}
		public static string ToHtmlString(this Color color)
		{
			return color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
		}
	}
}
