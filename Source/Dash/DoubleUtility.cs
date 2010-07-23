using System;
using System.Collections.Generic;
using System.Linq;
using Utility.Extensions;

namespace Utility.Utilities
{
	public static class DoubleUtility
	{
		public static IEnumerable<double> GetMarkers(double start, double end, int count)
		{
			double intervalLength = ((end - start) / count).FractionRound(1, 2, 2.5, 5);

			start = start.Ceiling(intervalLength);
			end = end.Floor(intervalLength);

			for (double value = start; value <= end; value += intervalLength) yield return value;
		}
		public static double Minimum(params double[] values)
		{
			return values.Min();
		}
		public static double Maximum(params double[] values)
		{
			return values.Max();
		}
	}
}
