using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
	}
}
