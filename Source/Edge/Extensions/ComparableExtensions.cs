using System;
using System.Collections.Generic;
using Edge.Mathematics;

namespace Utility.Extensions
{
	public static class ComparableExtensions
	{
		public static T Minimum<T>(T value1, T value2) where T : IComparable<T>
		{
			return Comparer<T>.Default.Compare(value1, value2) <= 0 ? value1 : value2;
		}
		public static T Maximum<T>(T value1, T value2) where T : IComparable<T>
		{
			return Comparer<T>.Default.Compare(value1, value2) >= 0 ? value1 : value2;
		}
		public static T Clamp<T>(this T value, T minimum, T maximum) where T : IComparable<T>
		{
			value = Maximum(value, minimum);
			value = Minimum(value, maximum);

			return value;
		}
		public static T Clamp<T>(this T value, Range<T> range) where T : IComparable<T>
		{
			return value.Clamp(range.Start, range.End);
		}
	}
}
