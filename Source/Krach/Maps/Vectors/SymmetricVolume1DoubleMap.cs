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

namespace Krach.Maps.Vectors
{
	public class SymmetricVolume1DoubleMap : SymmetricMap<Orthotope1Double, Orthotope1Double, Vector1Double, Vector1Double, Volume1DoubleMap, Volume1DoubleMap>
	{
		public SymmetricVolume1DoubleMap(Orthotope1Double source, Orthotope1Double destination, IFactory<IMap<double, double>, Range<double>, Range<double>> mapper)
			: base(source, destination, GetFactory(mapper), GetFactory(mapper))
		{
		}
		public SymmetricVolume1DoubleMap(Orthotope1Double source, IFactory<IMap<double, double>, Range<double>, Range<double>> mapper)
			: this(source, new Orthotope1Double(new Range<double>(0, 1)), mapper)
		{
		}

		static IFactory<Volume1DoubleMap, Orthotope1Double, Orthotope1Double> GetFactory(IFactory<IMap<double, double>, Range<double>, Range<double>> mapper)
		{
			return new Factory<Volume1DoubleMap, Orthotope1Double, Orthotope1Double>
			(
				(source, destination) => new Volume1DoubleMap(source, destination, mapper)
			);
		}
	}
}
