using System;

namespace Edge.Mathematics.Maps
{
	public class MapVector2Float : IMap<Vector2Float, Vector2Float>
	{
		readonly IMap<float, float> mapX;
		readonly IMap<float, float> mapY;

		public MapVector2Float(IMap<float, float> mapX, IMap<float, float> mapY)
		{
			if (mapX == null) throw new ArgumentNullException("mapX");
			if (mapY == null) throw new ArgumentNullException("mapY");

			this.mapX = mapX;
			this.mapY = mapY;
		}

		public Vector2Float ForwardMap(Vector2Float value)
		{
			return new Vector2Float(mapX.ForwardMap(value.X), mapY.ForwardMap(value.Y));
		}
		public Vector2Float ReverseMap(Vector2Float value)
		{
			return new Vector2Float(mapX.ReverseMap(value.X), mapY.ReverseMap(value.Y));
		}
	}
}
