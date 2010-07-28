// Copyright � Julian Brunner 2010

// This file is part of Krach.
//
// Krach is free software: you can redistribute it and/or modify it under the
// terms of the GNU Lesser General Public License as published by the Free
// Software Foundation, either version 3 of the License, or (at your option) any
// later version.
//
// Krach is distributed in the hope that it will be useful, but WITHOUT ANY
// WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
// A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with
// Krach. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Linq;

namespace Krach.Extensions
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
