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
			this.offset = (source.EndX * destination.StartX - source.StartX * destination.EndX) / (source.EndX - source.StartX);
			this.factor = (destination.EndX - destination.StartX) / (source.EndX - source.StartX);
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
