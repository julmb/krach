using System;
using System.Collections.Generic;
using System.Linq;
using Edge;

namespace Utility.Extensions
{
	public static class ComparableExtensions
	{
		public static T Clamp<T>(this T value, T minimum, T maximum) where T : IComparable<T>
		{
			value = new[] { value, minimum }.Max();
			value = new[] { value, maximum }.Min();

			return value;
		}
	}
}
