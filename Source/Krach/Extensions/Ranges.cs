// Copyright © Julian Brunner 2009 - 2011

// This file is part of Stream Visualizer (streamvis).
// 
// Stream Visualizer is free software: you can redistribute it and/or modify it
// under the terms of the GNU General Public License as published by the Free
// Software Foundation, either version 3 of the License, or (at your option) any
// later version.
// 
// Stream Visualizer is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more
// details.
// 
// You should have received a copy of the GNU General Public License along with
// Stream Visualizer. If not, see <http://www.gnu.org/licenses/>.

using Krach.Basics;

namespace Krach.Extensions
{
	public static class Ranges
	{
		public static int Length(this Range<int> range)
		{
			return range.End - range.Start;
		}
		public static Range<int> Inflate(this Range<int> range, int value)
		{
			return new Range<int>(range.Start - value, range.End + value);
		}
		public static double Length(this Range<double> range)
		{
			return range.End - range.Start;
		}
		public static Range<double> Inflate(this Range<double> range, double value)
		{
			return new Range<double>(range.Start - value, range.End + value);
		}
		public static Range<double> InterpolateLinear(Range<double> range1, Range<double> range2, double fraction)
		{
			return new Range<double>(Scalars.InterpolateLinear(range1.Start, range2.Start, fraction), Scalars.InterpolateLinear(range1.End, range2.End, fraction));
		}
	}
}
