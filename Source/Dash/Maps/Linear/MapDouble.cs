namespace Edge.Maps.Linear
{
	public class MapDouble : IMap<double, double>
	{
		readonly Range1Double source;
		readonly Range1Double destination;
		readonly double offset;
		readonly double factor;

		public Range1Double Source { get { return source; } }
		public Range1Double Destination { get { return destination; } }
		public double Offset { get { return offset; } }
		public double Factor { get { return factor; } }

		public MapDouble(Range1Double source, Range1Double destination)
		{
			this.source = source;
			this.destination = destination;
			this.offset = (source.End1 * destination.Start1 - source.Start1 * destination.End1) / (source.End1 - source.Start1);
			this.factor = (destination.End1 - destination.Start1) / (source.End1 - source.Start1);
		}
		public MapDouble(Range1Double source) : this(source, new Range1Double(0, 1)) { }

		public double ForwardMap(double value)
		{
			return offset + value * factor;
		}
		public double ReverseMap(double value)
		{
			return (value - offset) / factor;
		}
	}
}
