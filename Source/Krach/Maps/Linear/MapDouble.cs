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
	public class MapDouble : IMap<double, double>
	{
		readonly Range<double> source;
		readonly Range<double> destination;
		readonly double offset;
		readonly double factor;

		public Range<double> Source { get { return source; } }
		public Range<double> Destination { get { return destination; } }
		public double Offset { get { return offset; } }
		public double Factor { get { return factor; } }

		public MapDouble(Range<double> source, Range<double> destination)
		{
			this.source = source;
			this.destination = destination;
			this.offset = (source.End * destination.Start - source.Start * destination.End) / (source.End - source.Start);
			this.factor = (destination.End - destination.Start) / (source.End - source.Start);
		}
		public MapDouble(Range<double> source) : this(source, new Range<double>(0, 1)) { }

		public double ForwardMap(double value)
		{
			return offset + value * factor;
		}
		public double ReverseMap(double value)
		{
			return (value - offset) / factor;
		}
	}
}
