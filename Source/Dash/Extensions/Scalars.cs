using System;
using System.Collections.Generic;
using System.Linq;
using Edge;

namespace Dash.Extensions
{
	public static class Scalars
	{
		// Calculation
		public static double Square(this double value)
		{
			return value * value;
		}
		public static double SquareRoot(this double value)
		{
			return Math.Sqrt(value);
		}
		public static int Modulo(this int value, int divisor)
		{
			if (divisor == 0) throw new DivideByZeroException();

			int remainder = value % divisor;
			if (remainder < 0) remainder += divisor;
			return remainder;
		}
		public static double Modulo(this double value, double divisor)
		{
			if (divisor == 0) throw new DivideByZeroException();

			double remainder = value % divisor;
			if (remainder < 0) remainder += divisor;
			return remainder;
		}
		public static double Exponentiate(this double value, double exponent)
		{
			return Math.Pow(value, exponent);
		}
		public static double Exponentiate(double exponent)
		{
			return Math.Exp(exponent);
		}
		public static double Logarithm(this double value, double @base)
		{
			if (@base == 10) return Math.Log10(value);

			return Math.Log(value, @base);
		}
		public static double Logarithm(this double value)
		{
			return Math.Log(value);
		}
		// Clamping
		public static double Clamp(this double value, double minimum, double maximum)
		{
			value = Maximum(value, minimum);
			value = Minimum(value, maximum);

			return value;
		}
		public static double Clamp(this double value, Range<double> range)
		{
			return value.Clamp(range.Start, range.End);
		}
		// Minimum and Maximum
		public static double Minimum(this IEnumerable<double> values)
		{
			return values.Min();
		}
		public static double Minimum(params double[] values)
		{
			return values.Min();
		}
		public static double Maximum(this IEnumerable<double> values)
		{
			return values.Max();
		}
		public static double Maximum(params double[] values)
		{
			return values.Max();
		}
		// Absolute
		public static double Absolute(this double value)
		{
			return Math.Abs(value);
		}
		// Sign
		public static double Sign(this double value)
		{
			if (value == 0) return 0;
			if (value < 0) return -1;
			if (value > 0) return +1;

			throw new InvalidOperationException();
		}
		// Rounding
		public static double Floor(this double value)
		{
			return Math.Floor(value);
		}
		public static double Floor(this double value, double interval)
		{
			return (value / interval).Floor() * interval;
		}
		public static double Power2Floor(this double value)
		{
			if (value <= 0) throw new ArgumentOutOfRangeException("value");

			uint result = 1;

			while (result < value) result <<= 1;

			return result >> 1;
		}
		public static double Ceiling(this double value)
		{
			return Math.Ceiling(value);
		}
		public static double Ceiling(this double value, double interval)
		{
			return (value / interval).Ceiling() * interval;
		}
		public static double Power2Ceiling(this double value)
		{
			if (value <= 0) throw new ArgumentOutOfRangeException("value");

			uint result = 1;

			while (result < value) result <<= 1;

			return result >> 0;
		}
		public static double Round(this double value)
		{
			return Math.Round(value);
		}
		public static double Round(this double value, double interval)
		{
			return (value / interval).Round() * interval;
		}
		public static double Power2Round(this double value)
		{
			if (value <= 0) throw new ArgumentOutOfRangeException("value");

			uint result = 1;

			while (result < value) result <<= 1;

			uint floor = result >> 1;

			return floor << (value.Logarithm(2) - ((double)floor).Logarithm(2) <= 0.5 ? 0 : 1);
		}
		// Target rounding
		public static double Round(this double value, IEnumerable<double> targets)
		{
			if (targets == null) throw new ArgumentNullException("targets");
			if (!targets.Any()) throw new ArgumentException("Argument \"targets\" cannot be empty.");

			return
			(
				from target in targets
				orderby (value - target).Absolute() ascending
				select target
			)
			.First();
		}
		public static double Round(this double value, params double[] targets)
		{
			return value.Round((IEnumerable<double>)targets);
		}
		public static double FractionRound(this double value, IEnumerable<double> targets)
		{
			if (targets == null) throw new ArgumentNullException("targets");
			if (!targets.Any()) throw new ArgumentException("Argument \"targets\" cannot be empty.");

			int magnitude = (int)value.Logarithm(10).Floor();
			double fraction = value * 10.0.Exponentiate(-magnitude);
			return fraction.Round(targets) * 10.0.Exponentiate(magnitude);
		}
		public static double FractionRound(this double value, params double[] targets)
		{
			return value.FractionRound((IEnumerable<double>)targets);
		}
		public static IEnumerable<double> GetMarkers(double start, double end, int count)
		{
			double intervalLength = ((end - start) / count).FractionRound(1, 2, 2.5, 5);

			start = start.Ceiling(intervalLength);
			end = end.Floor(intervalLength);

			for (double value = start; value <= end; value += intervalLength) yield return value;
		}
		// Interpolation
		public static double InterpolateLinear(double value1, double value2, double fraction)
		{
			return (1 - fraction) * value1 + fraction * value2;
		}
	}
}
