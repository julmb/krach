// Copyright © Julian Brunner 2009 - 2011

// This file is part of Stream Visualizer (streamvis).
// 
// Stream Visualizer is free software: you can redistribute it and/or modify it
// under the terms of the GNU General Public License as published by the Free
// Software Foundation, either version 3 of the License, or (at your option) any
// later version.
// 
// Stream Visualizer is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more
// details.
// 
// You should have received a copy of the GNU General Public License along with
// Stream Visualizer. If not, see <http://www.gnu.org/licenses/>.

using System;
using Krach.Basics;
using Krach.Extensions;
using Krach.Maps.Abstract;

namespace Krach.Maps.Basic
{
	public class CosineMap : IMap<double, double>
	{
		readonly Range<double> source;
		readonly Range<double> destination;

		public CosineMap(Range<double> source, Range<double> destination)
		{
			this.source = source;
			this.destination = destination;
		}

		public double Map(double value)
		{
			if (value < source.Start || value > source.End) throw new ArgumentOutOfRangeException("value");

			double fraction = (value - source.Start) / (source.End - source.Start);
			return Scalars.InterpolateCosine(destination.Start, destination.End, fraction);
		}
	}
}
