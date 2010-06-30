using System;

namespace Utility.Extensions
{
	public static class RangeExtensions
	{
		public static bool IsEmpty<T>(this Range<T> range) where T : IComparable<T>
		{
			return range.End.CompareTo(range.Start) <= 0;
		}
		public static double Clamp(this Range<double> range, double value)
		{
			return value.Clamp(range.Start, range.End);
		}
	}
}
