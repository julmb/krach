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

using System;
using Krach.Basics;
using Krach.Maps.Abstract;

namespace Krach.Extensions
{
	public static class Maps
	{
		public static Range<TDestination> Map<TSource, TDestination>(this IMap<TSource, TDestination> map, Range<TSource> value)
			where TSource : IEquatable<TSource>, IComparable<TSource>
			where TDestination : IEquatable<TDestination>, IComparable<TDestination>
		{
			return new Range<TDestination>(map.Map(value.Start), map.Map(value.End));
		}
		public static Volume1Double Map(this IMap<Vector1Double, Vector1Double> map, Volume1Double volume)
		{
			return new Volume1Double(map.Map(volume.Start), map.Map(volume.End));
		}
		public static Volume2Double Map(this IMap<Vector2Double, Vector2Double> map, Volume2Double volume)
		{
			return new Volume2Double(map.Map(volume.Start), map.Map(volume.End));
		}
	}
}
