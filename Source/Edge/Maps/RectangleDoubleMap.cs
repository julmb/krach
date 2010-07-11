using System;

namespace Edge.Maps
{
	public class RectangleDoubleMap : IMap<RectangleDouble, RectangleDouble>
	{
		readonly IMap<Vector2Double, Vector2Double> map;

		public RectangleDoubleMap(IMap<Vector2Double, Vector2Double> map)
		{
			if (map == null) throw new ArgumentNullException("map");

			this.map = map;
		}

		public RectangleDouble ForwardMap(RectangleDouble value)
		{
			return new RectangleDouble(map.ForwardMap(value.A), map.ForwardMap(value.B));
		}
		public RectangleDouble ReverseMap(RectangleDouble value)
		{
			return new RectangleDouble(map.ReverseMap(value.A), map.ReverseMap(value.B));
		}
	}
}
