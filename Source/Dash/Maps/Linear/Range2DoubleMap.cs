namespace Edge.Maps.Linear
{
	public class Range2DoubleMap : MapVector2Double
	{
		readonly Range2Double source;
		readonly Range2Double destination;

		public Range2Double Source { get { return source; } }
		public Range2Double Destination { get { return destination; } }

		public Range2DoubleMap(Range2Double source, Range2Double destination)
			: base(new MapDouble(source.RangeX, destination.RangeX), new MapDouble(source.RangeY, destination.RangeY))
		{
			this.source = source;
			this.destination = destination;
		}
	}
}
