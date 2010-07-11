namespace Edge.Maps.Linear
{
	public class MapRectangle : Vector2DoubleMap
	{
		readonly RectangleDouble source;
		readonly RectangleDouble destination;

		public RectangleDouble Source { get { return source; } }
		public RectangleDouble Destination { get { return destination; } }

		public MapRectangle(RectangleDouble source, RectangleDouble destination)
			: base
			(
				new MapDouble(new Range1Double(source.Left, source.Right), new Range1Double(destination.Left, destination.Right)),
				new MapDouble(new Range1Double(source.Top, source.Bottom), new Range1Double(destination.Top, destination.Bottom))
			)
		{
			this.source = source;
			this.destination = destination;
		}
	}
}
