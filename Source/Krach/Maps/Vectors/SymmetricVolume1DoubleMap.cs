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
	public class SymmetricVolume1DoubleMap : ISymmetricMap<Vector1Double, Vector1Double>
	{
		readonly Volume1Double source;
		readonly Volume1Double destination;
		readonly Volume1DoubleMap forward;
		readonly Volume1DoubleMap reverse;

		public Volume1Double Source { get { return source; } }
		public Volume1Double Destination { get { return destination; } }
		public Volume1DoubleMap Forward { get { return forward; } }
		public Volume1DoubleMap Reverse { get { return reverse; } }

		SymmetricVolume1DoubleMap(Volume1Double source, Volume1Double destination, Volume1DoubleMap forward, Volume1DoubleMap reverse)
		{
			if (forward == null) throw new ArgumentNullException("forward");
			if (reverse == null) throw new ArgumentNullException("reverse");

			this.source = source;
			this.destination = destination;
			this.forward = forward;
			this.reverse = reverse;
		}

		public static SymmetricVolume1DoubleMap CreateLinear(Volume1Double source, Volume1Double destination)
		{
			return new SymmetricVolume1DoubleMap
			(
				source,
				destination,
				Volume1DoubleMap.CreateLinear(source, destination),
				Volume1DoubleMap.CreateLinear(destination, source)
			);
		}
		public static SymmetricVolume1DoubleMap CreateLinear(Volume1Double source)
		{
			return CreateLinear(source, new Volume1Double(new Range<double>(0, 1)));
		}
		public static SymmetricVolume1DoubleMap CreateCosine(Volume1Double source, Volume1Double destination)
		{
			return new SymmetricVolume1DoubleMap
			(
				source,
				destination,
				Volume1DoubleMap.CreateCosine(source, destination),
				Volume1DoubleMap.CreateCosine(destination, source)
			);
		}
		public static SymmetricVolume1DoubleMap CreateCosine(Volume1Double source)
		{
			return CreateCosine(source, new Volume1Double(new Range<double>(0, 1)));
		}

		IMap<Vector1Double, Vector1Double> ISymmetricMap<Vector1Double, Vector1Double>.Forward { get { return Forward; } }
		IMap<Vector1Double, Vector1Double> ISymmetricMap<Vector1Double, Vector1Double>.Reverse { get { return Reverse; } }
	}
}
