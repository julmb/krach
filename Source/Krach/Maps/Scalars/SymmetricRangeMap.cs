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
using Krach.Design;
using Krach.Maps.Abstract;

namespace Krach.Maps.Scalar
{
	public class SymmetricRangeMap : SymmetricMap<Range<double>, Range<double>, double, double, RangeMap, RangeMap>
	{
		public SymmetricRangeMap(Range<double> source, Range<double> destination, IFactory<IMap<double, double>, Range<double>, Range<double>> mapper)
			: base(source, destination, GetFactory(mapper), GetFactory(mapper))
		{
		}
		public SymmetricRangeMap(Range<double> source, IFactory<IMap<double, double>, Range<double>, Range<double>> mapper)
			: this(source, new Range<double>(0, 1), mapper)
		{
		}

		static IFactory<RangeMap, Range<double>, Range<double>> GetFactory(IFactory<IMap<double, double>, Range<double>, Range<double>> mapper)
		{
			return new Factory<RangeMap, Range<double>, Range<double>>
			(
				(source, destination) => new RangeMap(source, destination, mapper)
			);
		}
	}
}
