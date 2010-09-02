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
// A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with
// Krach. If not, see <http://www.gnu.org/licenses/>.

using System;
using Krach.Basics;
using Krach.Maps.Abstract;

namespace Krach.Maps.Vectors
{
	public class Vector2DoubleMap : IMap<Vector2Double, Vector2Double>
	{
		readonly IMap<double, double> mapX;
		readonly IMap<double, double> mapY;

		public Vector2DoubleMap(IMap<double, double> mapX, IMap<double, double> mapY)
		{
			if (mapX == null) throw new ArgumentNullException("mapX");
			if (mapY == null) throw new ArgumentNullException("mapY");

			this.mapX = mapX;
			this.mapY = mapY;
		}

		public Vector2Double Map(Vector2Double value)
		{
			return new Vector2Double(mapX.Map(value.X), mapY.Map(value.Y));
		}
	}
}
