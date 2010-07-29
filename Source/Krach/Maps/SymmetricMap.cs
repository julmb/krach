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

namespace Krach.Maps
{
	public abstract class SymmetricMap<TSource, TDestination> : ISymmetricMap<TSource, TDestination>
	{
		readonly IMap<TSource, TDestination> forward;
		readonly IMap<TDestination, TSource> reverse;

		public IMap<TSource, TDestination> Forward { get { return forward; } }
		public IMap<TDestination, TSource> Reverse { get { return reverse; } }

		public SymmetricMap(IMap<TSource, TDestination> forward, IMap<TDestination, TSource> reverse)
		{
			if (forward == null) throw new ArgumentNullException("forward");
			if (reverse == null) throw new ArgumentNullException("reverse");

			this.forward = forward;
			this.reverse = reverse;
		}
	}
}