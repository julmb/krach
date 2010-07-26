using Dash.Extensions;
using Dash.Basics;

namespace Dash.Extensions
{
	public static class Ranges
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
			return new Range<double>(Scalars.InterpolateLinear(range1.Start, range2.Start, fraction), Scalars.InterpolateLinear(range1.End, range2.End, fraction));
		}
	}
}
