using System;

namespace Edge
{
	public struct Range1Double
	{
		readonly Range<double> range1;

		public double Start1 { get { return range1.Start; } }
		public double End1 { get { return range1.End; } }

		public Range1Double(double start, double end)
		{
			this.range1 = new Range<double>(start, end);
		}

		public override string ToString()
		{
			return "[" + range1 + "]";
		}
	}
}
