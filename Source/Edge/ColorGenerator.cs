// Copyright © Julian Brunner 2009 - 2010

// This file is part of Stream Visualizer (streamvis).
// 
// Stream Visualizer is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Stream Visualizer is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Stream Visualizer.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Utility.Extensions;

namespace Utility
{
	public class ColorGenerator
	{
		readonly Random random = new Random();
		readonly List<ColorPlus> colors = new List<ColorPlus>();

		public Color NextColor()
		{
			ColorPlus color = GetColor();

			colors.Add(color);

			return color.ToColor();
		}

		ColorPlus GetColor()
		{
			if (!colors.Any()) return GetRandomColors(1).Single();

			return
			(
				from randomColor in GetRandomColors(5000)
				orderby colors.Min((Func<ColorPlus, double>)(color => ColorPlus.Distance(randomColor, color))) descending
				select randomColor
			)
			.First();
		}
		IEnumerable<ColorPlus> GetRandomColors(int count)
		{
			for (int i = 0; i < count; i++)
				yield return ColorPlus.FromHsv(random.NextDouble(0, 6), random.NextDouble(0.5, 1), random.NextDouble(0.5, 1));
		}
	}
}
