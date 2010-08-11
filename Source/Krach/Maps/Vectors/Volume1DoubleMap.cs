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
using Krach.Maps.Scalar;

namespace Krach.Maps.Vectors
{
	public class Volume1DoubleMap : Vector1DoubleMap
	{
		readonly Volume1Double source;
		readonly Volume1Double destination;

		public Volume1Double Source { get { return source; } }
		public Volume1Double Destination { get { return destination; } }

		Volume1DoubleMap(Volume1Double source, Volume1Double destination, IMap<double, double> mapX)
			: base(mapX)
		{
			this.source = source;
			this.destination = destination;
		}

		public static Volume1DoubleMap CreateLinear(Volume1Double source, Volume1Double destination)
		{
			return new Volume1DoubleMap
			(
				source,
				destination,
				RangeMap.CreateLinear(source.RangeX, destination.RangeX)
			);
		}
		public static Volume1DoubleMap CreateCosine(Volume1Double source, Volume1Double destination)
		{
			return new Volume1DoubleMap
			(
				source,
				destination,
				RangeMap.CreateCosine(source.RangeX, destination.RangeX)
			);
		}
	}
}
