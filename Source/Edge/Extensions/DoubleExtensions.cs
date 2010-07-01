using System;
using System.Linq;

namespace Utility.Extensions
{
	public static class DoubleExtensions
	{
		public static double FractionRound(this double value, params double[] targets)
		{
			if (targets == null) throw new ArgumentNullException("targets");
			if (targets.Length == 0) throw new ArgumentException("Argument \"targets\" cannot be empty.");

			int magnitude = (int)Math.Floor(Math.Log10(value));
			double fraction = value * Math.Pow(10, -magnitude);
			return fraction.Round(targets) * Math.Pow(10, magnitude);
		}
		public static double Round(this double value, params double[] targets)
		{
			if (targets == null) throw new ArgumentNullException("targets");
			if (targets.Length == 0) throw new ArgumentException("Argument \"targets\" cannot be empty.");

			return
			(
				from target in targets
				orderby Math.Abs(value - target) ascending
				select target
			)
			.First();
		}
		public static double Floor(this double value, double interval)
		{
			return value.Floor(interval, 0);
		}
		public static double Floor(this double value, double interval, double offset)
		{
			return Math.Floor(value / interval) * interval;
		}
		public static double Ceiling(this double value, double interval)
		{
			return value.Ceiling(interval, 0);
		}
		public static double Ceiling(this double value, double interval, double offset)
		{
			return Math.Ceiling(value / interval) * interval;
		}
		//public static double Clamp(this double value, double minimum, double maximum)
		//{
		//    value = Math.Max(value, minimum);
		//    value = Math.Min(value, maximum);

		//    return value;
		//}
		public static double Square(this double value)
		{
			return value * value;
		}
	}
}
