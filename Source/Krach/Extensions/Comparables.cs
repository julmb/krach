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
	public static class Comparables
	{
		public static TSource Minimum<TSource>(this IEnumerable<TSource> source) where TSource : IComparable<TSource>
		{
			return source.Min();
		}
		public static TSource Minimum<TSource>(params TSource[] source) where TSource : IComparable<TSource>
		{
			return source.Minimum();
		}
		public static TSource Maximum<TSource>(this IEnumerable<TSource> source) where TSource : IComparable<TSource>
		{
			return source.Max();
		}
		public static TSource Maximum<TSource>(params TSource[] source) where TSource : IComparable<TSource>
		{
			return source.Maximum();
		}
		public static T Clamp<T>(this T value, T minimum, T maximum) where T : IComparable<T>
		{
			value = Enumerables.Create(value, minimum).Maximum();
			value = Enumerables.Create(value, maximum).Minimum();

			return value;
		}
		public static T Clamp<T>(this T value, OrderedRange<T> range) where T : IEquatable<T>, IComparable<T>
		{
			return value.Clamp(range.Start, range.End);
		}
	}
}
