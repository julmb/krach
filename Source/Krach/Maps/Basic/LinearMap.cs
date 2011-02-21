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

using Krach.Basics;
using Krach.Extensions;
using Krach.Maps.Abstract;

namespace Krach.Maps.Basic
{
	public class LinearMap : IMap<double, double>
	{
		readonly double offset;
		readonly double factor;

		public LinearMap(Range<double> source, Range<double> destination)
		{
			this.offset = (source.End * destination.Start - source.Start * destination.End) / source.Length();
			this.factor = destination.Length() / source.Length();
		}

		public double Map(double value)
		{
			return offset + value * factor;
		}
	}
}
