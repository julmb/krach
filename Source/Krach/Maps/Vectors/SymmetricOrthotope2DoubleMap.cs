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
	public class SymmetricOrthotope2DoubleMap : SymmetricMap<Orthotope2Double, Orthotope2Double, Vector2Double, Vector2Double, Orthotope2DoubleMap, Orthotope2DoubleMap>
	{
		public SymmetricOrthotope2DoubleMap(Orthotope2Double source, Orthotope2Double destination, IFactory<IMap<double, double>, Range<double>, Range<double>> mapper)
			: base(source, destination, GetFactory(mapper), GetFactory(mapper))
		{
		}
		public SymmetricOrthotope2DoubleMap(Orthotope2Double source, IFactory<IMap<double, double>, Range<double>, Range<double>> mapper)
			: this(source, new Orthotope2Double(new Range<double>(0, 1), new Range<double>(0, 1)), mapper)
		{
		}

		static IFactory<Orthotope2DoubleMap, Orthotope2Double, Orthotope2Double> GetFactory(IFactory<IMap<double, double>, Range<double>, Range<double>> mapper)
		{
			return new Factory<Orthotope2DoubleMap, Orthotope2Double, Orthotope2Double>
			(
				(source, destination) => new Orthotope2DoubleMap(source, destination, mapper)
			);
		}
	}
}
