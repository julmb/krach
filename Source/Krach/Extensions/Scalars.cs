// Copyright © Julian Brunner 2010 - 2011

// This file is part of Krach.
//
// Krach is free software: you can redistribute it and/or modify it under the
// terms of the GNU Lesser General Public License as published by the Free
// Software Foundation, either version 3 of the License, or (at your option) any
// later version.
//
// Krach is distributed in the hope that it will be useful, but WITHOUT ANY
// WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
// A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with
// Krach. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Basics;

namespace Krach.Extensions
{
	public static class Scalars
	{
		public const double Tau = 2 * Math.PI;

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
			if (remainder == divisor) remainder = 0; //For small negative remainder values in combination with rounding the above sum (falsely) yields the divisor.
			return remainder;
		}
		public static double Exponentiate(this double @base, double exponent)
		{
			if (@base <= 0 && exponent < 0) throw new ArgumentOutOfRangeException("exponent");

			return Math.Pow(@base, exponent);
		}
		public static double Exponentiate(double exponent)
		{
			return Math.Exp(exponent);
		}
		public static Complex Exponentiate(Complex exponent)
		{
			return Exponentiate(exponent.Real) * new Complex(Cosine(exponent.Imaginary), Sine(exponent.Imaginary));
		}
		public static double Logarithm(this double @base, double value)
		{
			if (@base <= 0) throw new ArgumentOutOfRangeException("base");
			if (value <= 0) throw new ArgumentOutOfRangeException("value");

			if (@base == 10) return Math.Log10(value);

			return Math.Log(value, @base);
		}
		public static double Logarithm(double value)
		{
			if (value <= 0) throw new ArgumentOutOfRangeException("value");

			return Math.Log(value);
		}
		public static int Factorial(int n)
		{
			if (n == 0) return 1;

			return n * Factorial(n - 1);
		}
		public static int BinomialCoefficient(int n, int k)
		{
			if (n < k) throw new ArgumentException("parameter 'n' was less than parameter 'k'");

			if (k == 0) return 1;

			return n * BinomialCoefficient(n - 1, k - 1) / k;
		}
		// Trigonometry
		public static double PSine(double value)
		{
			return Math.Sin(value * 2 * Math.PI);
		}
		public static double Sine(this double value)
		{
			return Math.Sin(value);
		}
		public static double ArcSine(this double value)
		{
			return Math.Asin(value);
		}
		public static double Cosine(this double value)
		{
			return Math.Cos(value);
		}
		public static double ArcCosine(this double value)
		{
			return Math.Acos(value);
		}
		public static double Tangent(this double value)
		{
			return Math.Tan(value);
		}
		public static double ArcTangent(this double value)
		{
			return Math.Atan(value);
		}
		public static double ArcTangent(double y, double x)
		{
			return Math.Atan2(y, x);
		}
		// Absolute
		public static int Absolute(this int value)
		{
			return Math.Abs(value);
		}
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
		public static double PowerOf2Floor(this double value)
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
		public static double PowerOf2Ceiling(this double value)
		{
			if (value <= 0) throw new ArgumentOutOfRangeException("value");

			uint result = 1;

			while (result < value) result <<= 1;

			return result >> 0;
		}
		public static double Round(this double value)
		{
			double fraction = (value - (int)value).Absolute();

			if (fraction < 0.5) return value.Floor();
			if (fraction >= 0.5) return value.Ceiling();

			throw new InvalidOperationException();
		}
		public static double Round(this double value, double interval)
		{
			return (value / interval).Round() * interval;
		}
		public static double PowerOf2Round(this double value)
		{
			if (value <= 0) throw new ArgumentOutOfRangeException("value");

			uint result = 1;

			while (result < value) result <<= 1;

			uint floor = result >> 1;

			return floor << (2.0.Logarithm(value) - 2.0.Logarithm((double)floor) <= 0.5 ? 0 : 1);
		}
		public static bool AreWithinBound(double value1, double value2, double bound)
		{
			return Absolute(value1 - value2) <= bound;
		}
		public static bool IsPowerOf2(this int value)
		{
			if (value <= 0) throw new ArgumentOutOfRangeException("value");

			uint result = 1;

			while (result < value) result <<= 1;

			return result == value;
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
			if (value == 0) throw new ArgumentOutOfRangeException("value");
			if (targets == null) throw new ArgumentNullException("targets");
			if (!targets.Any()) throw new ArgumentException("Argument \"targets\" cannot be empty.");

			int magnitude = (int)10.0.Logarithm(value).Floor() + 1;
			double fraction = value * 10.0.Exponentiate(-magnitude);
			return fraction.Round(targets) * 10.0.Exponentiate(+magnitude);
		}
		public static double FractionRound(this double value, params double[] targets)
		{
			return value.FractionRound((IEnumerable<double>)targets);
		}
		public static IEnumerable<double> GetMarkers(double start, double end, int count)
		{
			double intervalLength = ((end - start) / count).FractionRound(0.1, 0.2, 0.25, 0.5, 1.0);

			start = start.Ceiling(intervalLength);
			end = end.Floor(intervalLength);

			return GetIntermediateValuesSymmetric(start, end, (int)((end - start) / intervalLength).Round());
		}
		// Interpolation
		public static double InterpolateLinear(double value1, double value2, double fraction)
		{
			if (fraction < 0 || fraction > 1) throw new ArgumentOutOfRangeException("fraction");

			return (1 - fraction) * value1 + fraction * value2;
		}
		public static double InterpolateSine(double value1, double value2, double fraction)
		{
			if (fraction < 0 || fraction > 1) throw new ArgumentOutOfRangeException("fraction");

			return InterpolateLinear(value1, value2, 0.5 * (1 - Scalars.PSine(0.25 + 0.5 * fraction)));
		}
		public static IEnumerable<double> GetIntermediateValues(double start, double end, int count)
		{
			for (int index = 0; index < count; index++) yield return start + index * (end - start) / count;
		}
		public static IEnumerable<double> GetIntermediateValuesSymmetric(double start, double end, int count)
		{
			if (count == 1)
			{
				yield return (start + end) / 2;

				yield break;
			}

			for (int index = 0; index < count; index++) yield return start + index * (end - start) / (count - 1);
		}
	}
}
