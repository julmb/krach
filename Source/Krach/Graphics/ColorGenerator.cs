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

using System;
using System.Collections.Generic;
using System.Linq;
using Krach;

namespace Krach.Graphics
{
	public class ColorGenerator
	{
		readonly RandomNumberGenerator generator = new RandomNumberGenerator();
		readonly List<Color> colors = new List<Color>();

		public Color NextColor()
		{
			Color color = GetColor();

			colors.Add(color);

			return color;
		}

		Color GetColor()
		{
			if (!colors.Any()) return GetRandomColors(generator, 1).Single();

			return
			(
				from randomColor in GetRandomColors(generator, 5000)
				orderby colors.Min(color => Color.DistanceRgb(randomColor, color)) descending
				select randomColor
			)
			.First();
		}

		static IEnumerable<Color> GetRandomColors(RandomNumberGenerator generator, int count)
		{
			for (int i = 0; i < count; i++) yield return Color.FromHsv(generator.NextDouble(0, 6), generator.NextDouble(0.5, 1), generator.NextDouble(0.5, 1));
		}
	}
}
