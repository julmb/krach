using Utilities;

namespace Edge
{
	public static class RangeExtensions
	{
		public static double Length(this Range<double> range)
		{
			return range.End - range.Start;
		}
		public static Range<double> Inflate(this Range<double> range, double value)
		{
			return new Range<double>(range.Start - value, range.End + value);
		}
		public static Range<double> InterpolateLinear(Range<double> range1, Range<double> range2, double fraction)
		{
			return new Range<double>(MathUtilities.InterpolateLinear(range1.Start, range2.Start, fraction), MathUtilities.InterpolateLinear(range1.End, range2.End, fraction));
		}
	}
}
