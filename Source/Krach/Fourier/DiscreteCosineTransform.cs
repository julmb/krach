// Copyright © Julian Brunner 2010

// This file is part of Krach.
//
// Krach is free software: you can redistribute it and/or modify it under the
// terms of the GNU Lesser General Public License as published by the Free
// Software Foundation, either version 3 of the License, or (at your option) any
// later version.
//
// Krach is distributed in the hope that it will be useful, but WITHOUT ANY
// WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
// A PARTICULAR PURPOSE.  See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with
// Krach. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Krach.Extensions;
using Krach.Basics;

namespace Krach.Fourier
{
	public static class DiscreteCosineTransform
	{
		public static double[] TransformForward(double[] values)
		{
			return (GetForwardTransformation(values.Length) * new Matrix(values)).ToValues();
		}
		public static double[] TransformReverse(double[] values)
		{
			return (GetReverseTransformation(values.Length) * new Matrix(values)).ToValues();
		}

		static Matrix GetForwardTransformation(int size)
		{
			Matrix transformation = new Matrix(size, size);

			for (int row = 0; row < transformation.Rows; row++)
				for (int column = 0; column < transformation.Columns; column++)
					transformation[row, column] = Scalars.Cosine((Math.PI / size) * (0.5 + column) * row);

			return transformation;
		}
		static Matrix GetReverseTransformation(int size)
		{
			Matrix transformation = new Matrix(size, size);

			for (int row = 0; row < transformation.Rows; row++)
			{
				transformation[row, 0] = 0.5;
				for (int column = 1; column < transformation.Columns; column++)
					transformation[row, column] = Scalars.Cosine((Math.PI / size) * column * (0.5 + row));
			}

			return transformation;
		}
	}
}
