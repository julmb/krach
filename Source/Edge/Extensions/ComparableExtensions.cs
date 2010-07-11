using System;
using System.Collections.Generic;
using System.Linq;
using Edge;

namespace Utility.Extensions
{
	public static class ComparableExtensions
	{
		public static T Minimum<T>(params T[] values) where T : IComparable<T>
		{
			return values.Min();
		}
		public static T Maximum<T>(params T[] values) where T : IComparable<T>
		{
			return values.Max();
		}
		public static T Clamp<T>(this T value, T minimum, T maximum) where T : IComparable<T>
		{
			value = Maximum(value, minimum);
			value = Minimum(value, maximum);

			return value;
		}
		//public static T Clamp<T>(this T value, Range1Double<T> range) where T : IComparable<T>
		//{
		//    return value.Clamp(range.Start, range.End);
		//}
	}
}
