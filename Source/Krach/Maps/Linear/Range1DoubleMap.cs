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

using Krach.Basics;

namespace Krach.Maps.Linear
{
	public class Range1DoubleMap : MapVector1Double
	{
		readonly Volume1Double source;
		readonly Volume1Double destination;

		public Volume1Double Source { get { return source; } }
		public Volume1Double Destination { get { return destination; } }

		public Range1DoubleMap(Volume1Double source, Volume1Double destination)
			: base(new MapDouble(source.RangeX, destination.RangeX))
		{
			this.source = source;
			this.destination = destination;
		}
	}
}
