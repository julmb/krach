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
