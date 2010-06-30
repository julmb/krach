namespace Edge.Mathematics.Maps.Linear
{
	public class MapDouble : IMap<double, double>
	{
		readonly Range<double> source;
		readonly Range<double> destination;
		readonly double offset;
		readonly double factor;

		public Range<double> Source { get { return source; } }
		public Range<double> Destination { get { return destination; } }
		public double Offset { get { return offset; } }
		public double Factor { get { return factor; } }

		public MapDouble(Range<double> source, Range<double> destination)
		{
			this.source = source;
			this.destination = destination;

			double divisor = source.End - source.Start;
			this.offset = (source.End * destination.Start - source.Start * destination.End) / divisor;
			this.factor = (destination.End - destination.Start) / divisor;
		}
		public MapDouble(Range<double> source) : this(source, new Range<double>(0, 1)) { }

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
