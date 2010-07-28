namespace Dash.Graphics
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
