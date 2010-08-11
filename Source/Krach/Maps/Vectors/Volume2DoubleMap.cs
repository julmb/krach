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
	public class Volume2DoubleMap : Vector2DoubleMap
	{
		readonly Volume2Double source;
		readonly Volume2Double destination;

		public Volume2Double Source { get { return source; } }
		public Volume2Double Destination { get { return destination; } }

		Volume2DoubleMap(Volume2Double source, Volume2Double destination, IMap<double, double> mapX, IMap<double, double> mapY)
			: base(mapX, mapY)
		{
			this.source = source;
			this.destination = destination;
		}

		public static Volume2DoubleMap CreateLinear(Volume2Double source, Volume2Double destination)
		{
			return new Volume2DoubleMap
			(
				source,
				destination,
				RangeMap.CreateLinear(source.RangeX, destination.RangeX),
				RangeMap.CreateLinear(source.RangeY, destination.RangeY)
			);
		}
		public static Volume2DoubleMap CreateLinear(Volume2Double source)
		{
			return CreateLinear(source, new Volume2Double(new Range<double>(0, 1), new Range<double>(0, 1)));
		}
		public static Volume2DoubleMap CreateCosine(Volume2Double source, Volume2Double destination)
		{
			return new Volume2DoubleMap
			(
				source,
				destination,
				RangeMap.CreateCosine(source.RangeX, destination.RangeX),
				RangeMap.CreateCosine(source.RangeY, destination.RangeY)
			);
		}
		public static Volume2DoubleMap CreateCosine(Volume2Double source)
		{
			return CreateCosine(source, new Volume2Double(new Range<double>(0, 1), new Range<double>(0, 1)));
		}
	}
}
