// Copyright © Julian Brunner 2009 - 2010

// This file is part of Stream Visualizer (streamvis).
// 
// Stream Visualizer is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Stream Visualizer is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Stream Visualizer.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;

namespace Utility.Utilities
{
	public static class EnumerableUtility
	{
		public static IEnumerable<T> Construct<T>(params IEnumerable<T>[] sources)
		{
			if (sources == null) throw new ArgumentNullException("sources");

			foreach (IEnumerable<T> source in sources)
			{
				if (source == null) throw new ArgumentException("sources");

				foreach (T item in source)
					yield return item;
			}
		}
		public static IEnumerable<T> Consume<T>(Func<T> getItem)
		{
			while (true) yield return getItem();
		}
		public static IEnumerable<T> Single<T>(T item)
		{
			yield return item;
		}
	}
}
