using System;
using System.Collections.Generic;

namespace Krach.Extensions
{
	public static class Items
	{
		public static T Equal<T>(T item1, T item2) where T : IEquatable<T>
		{
			if (!EqualityComparer<T>.Default.Equals(item1, item2)) throw new ArgumentException("The two items are not equal.");

			return item1;
		}
	}
}
