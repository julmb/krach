using System;

namespace Edge.Mathematics.Maps
{
	public class MapRange<T> : IMap<Range<T>, Range<T>>
		where T : IComparable<T>
	{
		readonly IMap<T, T> map;

		public MapRange(IMap<T, T> map)
		{
			if (map == null) throw new ArgumentNullException("map");

			this.map = map;
		}

		public Range<T> ForwardMap(Range<T> value)
		{
			return new Range<T>(map.ForwardMap(value.Start), map.ForwardMap(value.End));
		}
		public Range<T> ReverseMap(Range<T> value)
		{
			return new Range<T>(map.ReverseMap(value.Start), map.ReverseMap(value.End));
		}
	}
}
