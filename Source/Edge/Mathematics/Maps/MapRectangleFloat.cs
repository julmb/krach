using System;

namespace Edge.Mathematics.Maps
{
	public class MapRectangleFloat : IMap<RectangleFloat, RectangleFloat>
	{
		readonly IMap<Vector2Float, Vector2Float> map;

		public MapRectangleFloat(IMap<Vector2Float, Vector2Float> map)
		{
			if (map == null) throw new ArgumentNullException("map");

			this.map = map;
		}

		public RectangleFloat ForwardMap(RectangleFloat value)
		{
			return new RectangleFloat(map.ForwardMap(value.A), map.ForwardMap(value.B));
		}
		public RectangleFloat ReverseMap(RectangleFloat value)
		{
			return new RectangleFloat(map.ReverseMap(value.A), map.ReverseMap(value.B));
		}
	}
}
