using System;
using System.Collections.Generic;
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
		public static double Modulo(double a, double b)
		{
			if (b == 0) throw new DivideByZeroException();

			double remainder = a % b;
			if (remainder < 0) remainder += b;
			return remainder;
		}
	}
}
