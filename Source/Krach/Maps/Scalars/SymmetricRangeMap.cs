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

namespace Krach.Maps.Scalar
{
	public class SymmetricRangeMap : ISymmetricMap<double, double>
	{
		readonly RangeMap forward;
		readonly RangeMap reverse;

		public RangeMap Forward { get { return forward; } }
		public RangeMap Reverse { get { return reverse; } }

		SymmetricRangeMap(RangeMap forward, RangeMap reverse)
		{
			if (forward == null) throw new ArgumentNullException("forward");
			if (reverse == null) throw new ArgumentNullException("reverse");

			this.forward = forward;
			this.reverse = reverse;
		}

		public static SymmetricRangeMap CreateLinear(Range<double> source, Range<double> destination)
		{
			return new SymmetricRangeMap
			(
				RangeMap.CreateLinear(source, destination),
				RangeMap.CreateLinear(destination, source)
			);
		}
		public static SymmetricRangeMap CreateCosine(Range<double> source, Range<double> destination)
		{
			return new SymmetricRangeMap
			(
				RangeMap.CreateCosine(source, destination),
				RangeMap.CreateCosine(destination, source)
			);
		}

		IMap<double, double> ISymmetricMap<double, double>.Forward { get { return Forward; } }
		IMap<double, double> ISymmetricMap<double, double>.Reverse { get { return Reverse; } }
	}
}
