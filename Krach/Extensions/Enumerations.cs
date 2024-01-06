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
using System.Reflection;

namespace Krach.Extensions
{
	public static class Enumerations
	{
		public static TEnumeration ToEnumeration<TEnumeration>(this int value)
		{
			TEnumeration result = (TEnumeration)Enum.ToObject(typeof(TEnumeration), value);

			if (!Enum.IsDefined(typeof(TEnumeration), value)) throw new ArgumentOutOfRangeException("value");

			return result;
		}
		public static IEnumerable<TEnumeration> GetValues<TEnumeration>()
		{
			if (!typeof(TEnumeration).IsEnum) throw new ArgumentException("Parameter TEnumeration is not an enumeration.");

			return typeof(TEnumeration).GetFields(BindingFlags.Public | BindingFlags.Static).Select(field => (TEnumeration)field.GetValue(null)).ToArray();
		}
	}
}
