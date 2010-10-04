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
// A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with
// Krach. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Basics;
using Krach.Extensions;

namespace Krach.SignalProcessing
{
	// TODO: Questions:
	// - Is there a better way to handle waves at frequencies like 0.5 or 1.5?
	// - Does the DFT try to use minimal summed amplitude?
	// - Why does the DFT always try to put as much amplitude into the low frequencies?
	// - What about time-frequency analysis?
	// - How should one choose the intervals to which DFT is applied?
	public static class DiscreteFourierTransform
	{
		// TODO: This shouldn't be static (make the transformer an object)
		static readonly Dictionary<int, MatrixComplex> forwardTransformations = new Dictionary<int, MatrixComplex>();
		static readonly Dictionary<int, MatrixComplex> reverseTransformations = new Dictionary<int, MatrixComplex>();

		public static IEnumerable<Complex> TransformForward(IEnumerable<Complex> values)
		{
			if (!forwardTransformations.ContainsKey(values.Count())) forwardTransformations[values.Count()] = GetForwardTransformation(values.Count());

			MatrixComplex transformation = forwardTransformations[values.Count()];
			MatrixComplex vector = Matrices.ValuesToMatrix(values);

			return Matrices.MatrixToValues(transformation * vector);
		}
		public static IEnumerable<Complex> TransformReverse(IEnumerable<Complex> values)
		{
			if (!reverseTransformations.ContainsKey(values.Count())) reverseTransformations[values.Count()] = GetReverseTransformation(values.Count());

			MatrixComplex transformation = reverseTransformations[values.Count()];
			MatrixComplex vector = Matrices.ValuesToMatrix(values);

			return Matrices.MatrixToValues(transformation * vector);
		}

		// Intended for analysis
		// Input \ Output | Real | Imaginary
		// ---------------|------|----------
		// Real           |  +   |     +
		// Imaginary      |  -   |     +
		static MatrixComplex GetForwardTransformation(int size)
		{
			MatrixComplex transformation = new MatrixComplex(size, size);

			Complex factor = (2 * Math.PI / size) * Complex.ImaginaryOne;

			for (int row = 0; row < transformation.Rows; row++)
				for (int column = 0; column < transformation.Columns; column++)
					transformation[row, column] = Scalars.Exponentiate(+factor * row * column);

			return (1 / Scalars.SquareRoot(size)) * transformation;
		}
		// Intended for synthesis
		// Input \ Output | Real | Imaginary
		// ---------------|------|----------
		// Real           |  +   |     -
		// Imaginary      |  +   |     +
		static MatrixComplex GetReverseTransformation(int size)
		{
			MatrixComplex transformation = new MatrixComplex(size, size);

			Complex factor = (2 * Math.PI / size) * Complex.ImaginaryOne;

			for (int row = 0; row < transformation.Rows; row++)
				for (int column = 0; column < transformation.Columns; column++)
					transformation[row, column] = Scalars.Exponentiate(-factor * row * column);

			return (1 / Scalars.SquareRoot(size)) * transformation;
		}
	}
}
