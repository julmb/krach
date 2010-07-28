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
// A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with
// Krach. If not, see <http://www.gnu.org/licenses/>.

using System;
using Krach.Basics;

namespace Krach.Maps
{
	public class MapRange<T> : IMap<Range<T>, Range<T>>
		where T : IEquatable<T>, IComparable<T>
	{
		readonly IMap<T, T> map;

		public MapRange(IMap<T, T> map)
		{
			if (map == null) throw new ArgumentNullException("map");

			this.map = map;
		}

		public Range<T> ForwardMap(Range<T> value)
		{
			return new Range<T>(map.ForwardMap(value.Start), map.ForwardMap(value.End));
		}
		public Range<T> ReverseMap(Range<T> value)
		{
			return new Range<T>(map.ReverseMap(value.Start), map.ReverseMap(value.End));
		}
	}
}
