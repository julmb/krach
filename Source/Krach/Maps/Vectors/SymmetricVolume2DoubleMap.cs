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

namespace Krach.Maps.Vectors
{
	public class SymmetricVolume2DoubleMap : ISymmetricMap<Vector2Double, Vector2Double>
	{
		readonly Volume2DoubleMap forward;
		readonly Volume2DoubleMap reverse;

		public Volume2DoubleMap Forward { get { return forward; } }
		public Volume2DoubleMap Reverse { get { return reverse; } }

		SymmetricVolume2DoubleMap(Volume2DoubleMap forward, Volume2DoubleMap reverse)
		{
			if (forward == null) throw new ArgumentNullException("forward");
			if (reverse == null) throw new ArgumentNullException("reverse");

			this.forward = forward;
			this.reverse = reverse;
		}

		public static SymmetricVolume2DoubleMap CreateLinear(Volume2Double source, Volume2Double destination)
		{
			return new SymmetricVolume2DoubleMap
			(
				Volume2DoubleMap.CreateLinear(source, destination),
				Volume2DoubleMap.CreateLinear(destination, source)
			);
		}
		public static SymmetricVolume2DoubleMap CreateCosine(Volume2Double source, Volume2Double destination)
		{
			return new SymmetricVolume2DoubleMap
			(
				Volume2DoubleMap.CreateCosine(source, destination),
				Volume2DoubleMap.CreateCosine(destination, source)
			);
		}

		IMap<Vector2Double, Vector2Double> ISymmetricMap<Vector2Double, Vector2Double>.Forward { get { return Forward; } }
		IMap<Vector2Double, Vector2Double> ISymmetricMap<Vector2Double, Vector2Double>.Reverse { get { return Reverse; } }
	}
}
