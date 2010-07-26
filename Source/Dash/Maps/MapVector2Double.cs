using System;
using Dash.Basics;

namespace Dash.Maps
{
	public class MapVector2Double : IMap<Vector2Double, Vector2Double>
	{
		readonly IMap<double, double> mapX;
		readonly IMap<double, double> mapY;

		public MapVector2Double(IMap<double, double> mapX, IMap<double, double> mapY)
		{
			if (mapX == null) throw new ArgumentNullException("mapX");
			if (mapY == null) throw new ArgumentNullException("mapY");

			this.mapX = mapX;
			this.mapY = mapY;
		}

		public Vector2Double ForwardMap(Vector2Double value)
		{
			return new Vector2Double(mapX.ForwardMap(value.X), mapY.ForwardMap(value.Y));
		}
		public Vector2Double ReverseMap(Vector2Double value)
		{
			return new Vector2Double(mapX.ReverseMap(value.X), mapY.ReverseMap(value.Y));
		}
	}
}
