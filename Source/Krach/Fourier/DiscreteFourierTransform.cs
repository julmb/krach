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
using Krach.Basics;
using Krach.Extensions;

namespace Krach.Fourier
{
	public static class DiscreteFourierTransform
	{
		public static Complex[] TransformForward(Complex[] values)
		{
			return (GetForwardTransformation(values.Length) * new MatrixComplex(values)).ToValues();
		}
		public static Complex[] TransformReverse(Complex[] values)
		{
			return (GetReverseTransformation(values.Length) * new MatrixComplex(values)).ToValues();
		}

		static MatrixComplex GetForwardTransformation(int size)
		{
			MatrixComplex transformation = new MatrixComplex(size, size);

			Complex factor = (2 * Math.PI / size) * Complex.ImaginaryOne;

			for (int row = 0; row < transformation.Rows; row++)
				for (int column = 0; column < transformation.Columns; column++)
					transformation[row, column] = Scalars.Exponentiate(-factor * row * column);

			return transformation;
		}
		static MatrixComplex GetReverseTransformation(int size)
		{
			MatrixComplex transformation = new MatrixComplex(size, size);

			Complex factor = (2 * Math.PI / size) * Complex.ImaginaryOne;

			for (int row = 0; row < transformation.Rows; row++)
				for (int column = 0; column < transformation.Columns; column++)
					transformation[row, column] = Scalars.Exponentiate(factor * row * column);

			return (1.0 / size) * transformation;
		}
	}
}
