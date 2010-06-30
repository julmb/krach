namespace Utilities
{
	public static class MathExtensions
	{
		public static int Power2Floor(this float value)
		{
			int result = 1;

			while (result < value) result <<= 1;

			return result >> 1;
		}
		public static int Power2Ceiling(this float value)
		{
			int result = 1;

			while (result < value) result <<= 1;

			return result >> 0;
		}
		public static int Power2Floor(this double value)
		{
			int result = 1;

			while (result < value) result <<= 1;

			return result >> 1;
		}
		public static int Power2Ceiling(this double value)
		{
			int result = 1;

			while (result < value) result <<= 1;

			return result >> 0;
		}
	}
}
