namespace Edge.Mathematics.Maps.Linear
{
	public class MapFloat : IMap<float, float>
	{
		readonly Range<float> source;
		readonly Range<float> destination;
		readonly float offset;
		readonly float factor;

		public Range<float> Source { get { return source; } }
		public Range<float> Destination { get { return destination; } }
		public float Offset { get { return offset; } }
		public float Factor { get { return factor; } }

		public MapFloat(Range<float> source, Range<float> destination)
		{
			this.source = source;
			this.destination = destination;

			float divisor = source.End - source.Start;
			this.offset = (source.End * destination.Start - source.Start * destination.End) / divisor;
			this.factor = (destination.End - destination.Start) / divisor;
		}
		public MapFloat(Range<float> source) : this(source, new Range<float>(0, 1)) { }

		public float ForwardMap(float value)
		{
			return offset + value * factor;
		}
		public float ReverseMap(float value)
		{
			return (value - offset) / factor;
		}
	}
}
