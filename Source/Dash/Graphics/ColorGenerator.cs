using System;
using System.Collections.Generic;
using System.Linq;
using Utility.Extensions;

namespace Edge.Graphics
{
	public class ColorGenerator
	{
		readonly Random random = new Random();
		readonly List<Color> colors = new List<Color>();

		public Color NextColor()
		{
			Color color = GetColor();

			colors.Add(color);

			return color;
		}

		Color GetColor()
		{
			if (!colors.Any()) return GetRandomColors(random, 1).Single();

			return
			(
				from randomColor in GetRandomColors(random, 5000)
				orderby colors.Min(color => Color.DistanceRgb(randomColor, color)) descending
				select randomColor
			)
			.First();
		}

		static IEnumerable<Color> GetRandomColors(Random random, int count)
		{
			for (int i = 0; i < count; i++) yield return Color.FromHsv(random.NextDouble(0, 6), random.NextDouble(0.5, 1), random.NextDouble(0.5, 1));
		}
	}
}
