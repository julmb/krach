using Utilities;
using Utility;

namespace Multimedia.Graphics
{
	public class RectangleFloatMapping
	{
		readonly RectangleFloat source;
		readonly RectangleFloat destination;
		readonly Mapping mappingX;
		readonly Mapping mappingY;

		public RectangleFloat Source { get { return source; } }
		public RectangleFloat Destination { get { return destination; } }

		public RectangleFloatMapping(RectangleFloat source, RectangleFloat destination)
		{
			this.source = source;
			this.destination = destination;

			this.mappingX = new Mapping(new Range<float>(source.Left, source.Right), new Range<float>(destination.Left, destination.Right));
			this.mappingY = new Mapping(new Range<float>(source.Top, source.Bottom), new Range<float>(destination.Top, destination.Bottom));
		}

		public Vector2Float ForwardMap(Vector2Float position)
		{
			return new Vector2Float(mappingX.ForwardMap(position.X), mappingY.ForwardMap(position.Y));
		}
		public RectangleFloat ForwardMap(RectangleFloat rectangle)
		{
			return new RectangleFloat(ForwardMap(rectangle.A), ForwardMap(rectangle.B));
		}
		public Vector2Float ReverseMap(Vector2Float position)
		{
			return new Vector2Float(mappingX.ReverseMap(position.X), mappingY.ReverseMap(position.Y));
		}
		public RectangleFloat ReverseMap(RectangleFloat rectangle)
		{
			return new RectangleFloat(ReverseMap(rectangle.A), ReverseMap(rectangle.B));
		}
	}
}
