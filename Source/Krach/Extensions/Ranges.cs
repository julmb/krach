// Copyright © Julian Brunner 2010 - 2011

// This file is part of Krach.
//
// Krach is free software: you can redistribute it and/or modify it under the
// terms of the GNU Lesser General Public License as published by the Free
// Software Foundation, either version 3 of the License, or (at your option) any
// later version.
//
// Krach is distributed in the hope that it will be useful, but WITHOUT ANY
// WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
// A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with
// Krach. If not, see <http://www.gnu.org/licenses/>.

using Krach.Basics;

namespace Krach.Extensions
{
	public static class Ranges
	{
		public static int Length(this OrderedRange<int> range)
		{
			return range.End - range.Start;
		}
		public static OrderedRange<int> Inflate(this OrderedRange<int> range, int value)
		{
			return new OrderedRange<int>(range.Start - value, range.End + value);
		}
		public static double Length(this OrderedRange<double> range)
		{
			return range.End - range.Start;
		}
		public static OrderedRange<double> Inflate(this OrderedRange<double> range, double value)
		{
			return new OrderedRange<double>(range.Start - value, range.End + value);
		}
		public static OrderedRange<double> Interpolate(OrderedRange<double> range1, OrderedRange<double> range2, Interpolation<double> interpolate, double fraction)
		{
			return new OrderedRange<double>(interpolate(range1.Start, range2.Start, fraction), interpolate(range1.End, range2.End, fraction));
		}
	}
}
