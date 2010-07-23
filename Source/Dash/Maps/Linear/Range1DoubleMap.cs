namespace Edge.Maps.Linear
{
	public class Range1DoubleMap : MapVector1Double
	{
		readonly Range1Double source;
		readonly Range1Double destination;

		public Range1Double Source { get { return source; } }
		public Range1Double Destination { get { return destination; } }

		public Range1DoubleMap(Range1Double source, Range1Double destination)
			: base(new MapDouble(source.RangeX, destination.RangeX))
		{
			this.source = source;
			this.destination = destination;
		}
	}
}
