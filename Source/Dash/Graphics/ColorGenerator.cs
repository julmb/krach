using System;
using System.Collections.Generic;
using System.Linq;
using Utility.Extensions;
using Dash;

namespace Edge.Graphics
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
