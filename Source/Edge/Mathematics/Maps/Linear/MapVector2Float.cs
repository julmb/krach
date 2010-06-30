using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Edge.Mathematics.Maps.Linear
{
	public class MapVector2Float : Edge.Mathematics.Maps.MapVector2Float
	{
		readonly RectangleFloat source;
		readonly RectangleFloat destination;

		public RectangleFloat Source { get { return source; } }
		public RectangleFloat Destination { get { return destination; } }

		public MapVector2Float(RectangleFloat source, RectangleFloat destination)
			: base
			(
				new MapFloat(new Range<float>(source.Left, source.Right), new Range<float>(destination.Left, destination.Right)),
				new MapFloat(new Range<float>(source.Top, source.Bottom), new Range<float>(destination.Top, destination.Bottom))
			)
		{
			this.source = source;
			this.destination = destination;
		}
	}
}
