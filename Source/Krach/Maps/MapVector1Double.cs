using System;
using Dash.Basics;

namespace Dash.Maps
{
	public class MapVector1Double : IMap<Vector1Double, Vector1Double>
	{
		readonly IMap<double, double> mapX;

		public MapVector1Double(IMap<double, double> mapX)
		{
			if (mapX == null) throw new ArgumentNullException("mapX");

			this.mapX = mapX;
		}

		public Vector1Double ForwardMap(Vector1Double value)
		{
			return new Vector1Double(mapX.ForwardMap(value.X));
		}
		public Vector1Double ReverseMap(Vector1Double value)
		{
			return new Vector1Double(mapX.ReverseMap(value.X));
		}
	}
}
