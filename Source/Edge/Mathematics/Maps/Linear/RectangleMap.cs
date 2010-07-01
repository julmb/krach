namespace Edge.Mathematics.Maps.Linear
{
	public class RectangleMap : MapVector2Float
	{
		readonly RectangleFloat source;
		readonly RectangleFloat destination;

		public RectangleFloat Source { get { return source; } }
		public RectangleFloat Destination { get { return destination; } }

		public RectangleMap(RectangleFloat source, RectangleFloat destination)
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
