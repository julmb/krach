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
using Krach.Design;
using Krach.Maps.Abstract;

namespace Krach.Maps
{
	public class SymmetricMap<TSourceBounds, TDestinationBounds, TSource, TDestination, TForward, TReverse>
		: IBounded<TSourceBounds, TDestinationBounds>, ISymmetricMap<TSource, TDestination>
		where TForward : IMap<TSource, TDestination>
		where TReverse : IMap<TDestination, TSource>
	{
		readonly TSourceBounds source;
		readonly TDestinationBounds destination;
		readonly TForward forward;
		readonly TReverse reverse;

		public TSourceBounds Source { get { return source; } }
		public TDestinationBounds Destination { get { return destination; } }
		public TForward Forward { get { return forward; } }
		public TReverse Reverse { get { return reverse; } }

		public SymmetricMap(TSourceBounds source, TDestinationBounds destination, IFactory<TForward, TSourceBounds, TDestinationBounds> forwardMapper, IFactory<TReverse, TDestinationBounds, TSourceBounds> reverseMapper)
		{
			if (object.Equals(source, null)) throw new ArgumentNullException("source");
			if (object.Equals(destination, null)) throw new ArgumentNullException("destination");
			if (forwardMapper == null) throw new ArgumentNullException("forwardMapper");
			if (reverseMapper == null) throw new ArgumentNullException("reverseMapper");

			this.source = source;
			this.destination = destination;
			this.forward = forwardMapper.Create(source, destination);
			this.reverse = reverseMapper.Create(destination, source);
		}

		IMap<TSource, TDestination> ISymmetricMap<TSource, TDestination>.Forward { get { return Forward; } }
		IMap<TDestination, TSource> ISymmetricMap<TSource, TDestination>.Reverse { get { return Reverse; } }
	}
}