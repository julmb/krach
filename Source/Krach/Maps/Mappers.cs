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
using Krach.Maps.Basic;

namespace Krach.Maps
{
	public static class Mappers
	{
		public static IFactory<IMap<double, double>, Range<double>, Range<double>> Linear
		{
			get
			{
				return new Factory<IMap<double, double>, Range<double>, Range<double>>
				(
					(source, destination) => new LinearMap(source, destination)
				);
			}
		}
		public static IFactory<IMap<double, double>, Range<double>, Range<double>> Cosine
		{
			get
			{
				return new Factory<IMap<double, double>, Range<double>, Range<double>>
				(
					(source, destination) => new CosineMap(source, destination)
				);
			}
		}
	}
}