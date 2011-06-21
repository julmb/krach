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

namespace Krach.Design
{
	public class InstantEqualityComparer<TItem> : IEqualityComparer<TItem>
	{
		readonly Func<TItem, TItem, bool> equals;
		readonly Func<TItem, int> getHashCode;

		public InstantEqualityComparer(Func<TItem, TItem, bool> equals, Func<TItem, int> getHashCode)
		{
			if (equals == null) throw new ArgumentNullException("equals");
			if (getHashCode == null) throw new ArgumentNullException("getHashCode");

			this.equals = equals;
			this.getHashCode = getHashCode;
		}
		public InstantEqualityComparer(Func<TItem, TItem, bool> equals) : this(equals, item => item.GetHashCode()) { }

		public bool Equals(TItem item1, TItem item2)
		{
			return equals(item1, item2);
		}
		public int GetHashCode(TItem item)
		{
			return getHashCode(item);
		}
	}
}
