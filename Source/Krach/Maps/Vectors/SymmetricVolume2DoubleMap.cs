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

namespace Krach.Maps.Vectors
{
	public class SymmetricVolume2DoubleMap : SymmetricMap<Volume2Double, Volume2Double, Vector2Double, Vector2Double, Volume2DoubleMap, Volume2DoubleMap>
	{
		public SymmetricVolume2DoubleMap(Volume2Double source, Volume2Double destination, IFactory<IMap<double, double>, Range<double>, Range<double>> mapper)
			: base(source, destination, GetFactory(mapper), GetFactory(mapper))
		{
		}
		public SymmetricVolume2DoubleMap(Volume2Double source, IFactory<IMap<double, double>, Range<double>, Range<double>> mapper)
			: this(source, new Volume2Double(new Range<double>(0, 1), new Range<double>(0, 1)), mapper)
		{
		}

		static IFactory<Volume2DoubleMap, Volume2Double, Volume2Double> GetFactory(IFactory<IMap<double, double>, Range<double>, Range<double>> mapper)
		{
			return new Factory<Volume2DoubleMap, Volume2Double, Volume2Double>
			(
				(source, destination) => new Volume2DoubleMap(source, destination, mapper)
			);
		}
	}
}
