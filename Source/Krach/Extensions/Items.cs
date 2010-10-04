// Copyright © Julian Brunner 2010

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

namespace Krach.Extensions
{
	public static class Items
	{
		public static T Equal<T>(T item1, T item2) where T : IEquatable<T>
		{
			return Equal(item1, item2, EqualityComparer<T>.Default.Equals);
		}
		public static T Equal<T>(T item1, T item2, EqualityComparison<T> compare)
		{
			if (!compare(item1, item2)) throw new ArgumentException(string.Format("The parameters 'item1' and 'item2' are not equal ({0}, {1}).", item1, item2));

			return item1;
		}
	}
}
