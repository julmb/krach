namespace Utilities
{
	public static class MathUtilities
	{
		public static float InterpolateLinear(float a, float b, float fraction)
		{
			return (1 - fraction) * a + fraction * b;
		}
		public static double InterpolateLinear(double a, double b, double fraction)
		{
			return (1 - fraction) * a + fraction * b;
		}
	}
}
