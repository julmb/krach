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

using Krach.Basics;
using Krach.Design;
using Krach.Maps.Abstract;

namespace Krach.Maps.Scalar
{
	public class SymmetricRangeMap : SymmetricMap<OrderedRange<double>, OrderedRange<double>, double, double, RangeMap, RangeMap>
	{
		public SymmetricRangeMap(OrderedRange<double> source, OrderedRange<double> destination, IFactory<IMap<double, double>, OrderedRange<double>, OrderedRange<double>> mapper)
			: base(source, destination, GetFactory(mapper), GetFactory(mapper))
		{
		}
		public SymmetricRangeMap(OrderedRange<double> source, IFactory<IMap<double, double>, OrderedRange<double>, OrderedRange<double>> mapper) : this(source, new OrderedRange<double>(0, 1), mapper) { }
		public SymmetricRangeMap(IFactory<IMap<double, double>, OrderedRange<double>, OrderedRange<double>> mapper) : this(new OrderedRange<double>(0, 1), new OrderedRange<double>(0, 1), mapper) { }

		static IFactory<RangeMap, OrderedRange<double>, OrderedRange<double>> GetFactory(IFactory<IMap<double, double>, OrderedRange<double>, OrderedRange<double>> mapper)
		{
			return new Factory<RangeMap, OrderedRange<double>, OrderedRange<double>>
			(
				(source, destination) => new RangeMap(source, destination, mapper)
			);
		}
	}
}
