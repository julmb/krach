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
using Krach.Maps.Mappers;

namespace Krach.Maps.Scalar
{
	public class RangeMap : DoubleMap
	{
		readonly Range<double> source;
		readonly Range<double> destination;

		public Range<double> Source { get { return source; } }
		public Range<double> Destination { get { return destination; } }

		RangeMap(Range<double> source, Range<double> destination, IMap<double, double> map)
			: base(map)
		{
			this.source = source;
			this.destination = destination;
		}

		public static RangeMap CreateLinear(Range<double> source, Range<double> destination)
		{
			return new RangeMap(source, destination, new LinearMap(source, destination));
		}
		public static RangeMap CreateLinear(Range<double> source)
		{
			return CreateLinear(source, new Range<double>(0, 1));
		}
		public static RangeMap CreateCosine(Range<double> source, Range<double> destination)
		{
			return new RangeMap(source, destination, new CosineMap(source, destination));
		}
		public static RangeMap CreateCosine(Range<double> source)
		{
			return CreateCosine(source, new Range<double>(0, 1));
		}
	}
}
