using System;
using System.Linq;

namespace Dash.Extensions
{
	public static class Comparables
	{
		public static T Clamp<T>(this T value, T minimum, T maximum) where T : IComparable<T>
		{
			value = Enumerables.Create(value, minimum).Max();
			value = Enumerables.Create(value, maximum).Min();

			return value;
		}
	}
}
