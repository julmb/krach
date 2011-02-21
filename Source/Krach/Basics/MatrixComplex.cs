// Copyright © Julian Brunner 2009 - 2011

// This file is part of Stream Visualizer (streamvis).
// 
// Stream Visualizer is free software: you can redistribute it and/or modify it
// under the terms of the GNU General Public License as published by the Free
// Software Foundation, either version 3 of the License, or (at your option) any
// later version.
// 
// Stream Visualizer is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more
// details.
// 
// You should have received a copy of the GNU General Public License along with
// Stream Visualizer. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Text;
using Krach.Extensions;

namespace Krach.Basics
{
	public struct MatrixComplex : IEquatable<MatrixComplex>
	{
		public readonly Complex[,] values;

		public Complex this[int row, int column]
		{
			get { return values[row, column]; }
			set { values[row, column] = value; }
		}
		public int Rows { get { return values.GetLength(0); } }
		public int Columns { get { return values.GetLength(1); } }
		public MatrixComplex Transpose
		{
			get
			{
				MatrixComplex matrix = new MatrixComplex(Rows, Columns);

				for (int row = 0; row < Rows; row++)
					for (int column = 0; column < Columns; column++)
						matrix[row, column] = values[column, row];

				return matrix;
			}
		}

		public MatrixComplex(int rows, int columns)
		{
			this.values = new Complex[rows, columns];
		}
		public MatrixComplex(Complex[,] values)
		{
			this.values = values;
		}

		public override bool Equals(object obj)
		{
			return obj is MatrixComplex && this == (MatrixComplex)obj;
		}
		public override int GetHashCode()
		{
			int result = 0;

			for (int row = 0; row < Rows; row++)
				for (int column = 0; column < Columns; column++)
					result ^= values[row, column].GetHashCode();

			return result;
		}
		public override string ToString()
		{
			StringBuilder result = new StringBuilder();

			for (int row = 0; row < Rows; row++)
			{
				for (int column = 0; column < Columns; column++)
				{
					result.Append(values[row, column]);
					result.Append(", ");
				}
				result.Remove(result.Length - 2, 2);
				result.AppendLine();
			}

			return result.ToString();
		}
		public bool Equals(MatrixComplex other)
		{
			return this == other;
		}

		public MatrixComplex Power(int exponent)
		{
			if (Rows != Columns) throw new InvalidOperationException();

			MatrixComplex matrix = Identity(Items.Equal(Rows, Columns));

			for (int i = 0; i < exponent; i++) matrix *= this;

			return matrix;
		}

		public static bool operator ==(MatrixComplex matrix1, MatrixComplex matrix2)
		{
			if (matrix1.Rows != matrix2.Rows) throw new ArgumentException("The row counts do not match.");
			if (matrix1.Columns != matrix2.Columns) throw new ArgumentException("The column counts do not match.");

			int rows = Items.Equal(matrix1.Rows, matrix2.Rows);
			int columns = Items.Equal(matrix1.Columns, matrix2.Columns);

			for (int row = 0; row < rows; row++)
				for (int column = 0; column < columns; column++)
					if (matrix1[row, column] != matrix2[row, column])
						return false;

			return true;
		}
		public static bool operator !=(MatrixComplex matrix1, MatrixComplex matrix2)
		{
			if (matrix1.Rows != matrix2.Rows) throw new ArgumentException("The row counts do not match.");
			if (matrix1.Columns != matrix2.Columns) throw new ArgumentException("The column counts do not match.");

			int rows = Items.Equal(matrix1.Rows, matrix2.Rows);
			int columns = Items.Equal(matrix1.Columns, matrix2.Columns);

			for (int row = 0; row < rows; row++)
				for (int column = 0; column < columns; column++)
					if (matrix1[row, column] != matrix2[row, column])
						return true;

			return false;
		}

		public static MatrixComplex operator +(MatrixComplex matrix1, MatrixComplex matrix2)
		{
			if (matrix1.Rows != matrix2.Rows) throw new ArgumentException("The row counts do not match.");
			if (matrix1.Columns != matrix2.Columns) throw new ArgumentException("The column counts do not match.");

			int rows = Items.Equal(matrix1.Rows, matrix2.Rows);
			int columns = Items.Equal(matrix1.Columns, matrix2.Columns);

			MatrixComplex result = new MatrixComplex(rows, columns);

			for (int row = 0; row < rows; row++)
				for (int column = 0; column < columns; column++)
					result[row, column] = matrix1[row, column] + matrix2[row, column];

			return result;
		}
		public static MatrixComplex operator -(MatrixComplex matrix1, MatrixComplex matrix2)
		{
			if (matrix1.Rows != matrix2.Rows) throw new ArgumentException("The row counts do not match.");
			if (matrix1.Columns != matrix2.Columns) throw new ArgumentException("The column counts do not match.");

			int rows = Items.Equal(matrix1.Rows, matrix2.Rows);
			int columns = Items.Equal(matrix1.Columns, matrix2.Columns);

			MatrixComplex result = new MatrixComplex(rows, columns);

			for (int row = 0; row < rows; row++)
				for (int column = 0; column < columns; column++)
					result[row, column] = matrix1[row, column] - matrix2[row, column];

			return result;
		}
		public static MatrixComplex operator *(MatrixComplex matrix1, MatrixComplex matrix2)
		{
			if (matrix1.Columns != matrix2.Rows) throw new ArgumentOutOfRangeException();

			int size = Items.Equal(matrix1.Columns, matrix2.Rows);

			MatrixComplex result = new MatrixComplex(matrix1.Rows, matrix2.Columns);

			for (int row = 0; row < matrix1.Rows; row++)
				for (int column = 0; column < matrix2.Columns; column++)
					for (int i = 0; i < size; i++)
						result[row, column] += matrix1[row, i] * matrix2[i, column];

			return result;
		}
		public static MatrixComplex operator *(Complex factor, MatrixComplex matrix)
		{
			MatrixComplex result = new MatrixComplex(matrix.Rows, matrix.Columns);

			for (int row = 0; row < matrix.Rows; row++)
				for (int column = 0; column < matrix.Columns; column++)
					result[row, column] = matrix[row, column] * factor;

			return result;
		}
		public static MatrixComplex operator *(MatrixComplex matrix, Complex factor)
		{
			MatrixComplex result = new MatrixComplex(matrix.Rows, matrix.Columns);

			for (int row = 0; row < matrix.Rows; row++)
				for (int column = 0; column < matrix.Columns; column++)
					result[row, column] = matrix[row, column] * factor;

			return result;
		}

		public static MatrixComplex Identity(int size)
		{
			MatrixComplex matrix = new MatrixComplex(size, size);

			for (int i = 0; i < size; i++) matrix[i, i] = 1;

			return matrix;
		}
	}
}
