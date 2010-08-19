using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Krach.Extensions;
using Krach.Basics;

namespace Krach.Fourier
{
	public class DiscreteCosineTransformer
	{
		public static double[] Transform(double[] values)
		{
			return (GetTransformation(values.Length) * new Matrix(values)).ToValues();
		}

		static Matrix GetTransformation(int size)
		{
			Matrix transformation = new Matrix(size, size);

			for (int row = 0; row < transformation.Rows; row++)
				for (int column = 0; column < transformation.Columns; column++)
					transformation[row, column] = Scalars.Cosine((Math.PI / size) * (0.5 + column) * row);

			return transformation;
		}
	}
}
