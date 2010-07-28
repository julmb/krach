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

using System;
using Krach.Basics;

namespace Krach.Maps
{
	public class MapVector1Double : IMap<Vector1Double, Vector1Double>
	{
		readonly IMap<double, double> mapX;

		public MapVector1Double(IMap<double, double> mapX)
		{
			if (mapX == null) throw new ArgumentNullException("mapX");

			this.mapX = mapX;
		}

		public Vector1Double ForwardMap(Vector1Double value)
		{
			return new Vector1Double(mapX.ForwardMap(value.X));
		}
		public Vector1Double ReverseMap(Vector1Double value)
		{
			return new Vector1Double(mapX.ReverseMap(value.X));
		}
	}
}
