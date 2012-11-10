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
using System.Text;
using Krach.Extensions;

namespace Krach.Basics
{
	public struct Matrix : IEquatable<Matrix>
	{
		public readonly double[,] values;

		public double this[int row, int column]
		{
			get { return values[row, column]; }
			set { values[row, column] = value; }
		}
		public int RowCount { get { return values.GetLength(0); } }
		public int ColumnCount { get { return values.GetLength(1); } }
		public Matrix Transpose
		{
			get
			{
				Matrix matrix = new Matrix(RowCount, ColumnCount);

				for (int row = 0; row < RowCount; row++)
					for (int column = 0; column < ColumnCount; column++)
						matrix[row, column] = values[column, row];

				return matrix;
			}
		}

		public Matrix(int rows, int columns)
		{
			this.values = new double[rows, columns];
		}
		public Matrix(double[,] values)
		{
			this.values = values;
		}

		public override bool Equals(object obj)
		{
			return obj is Matrix && this == (Matrix)obj;
		}
		public override int GetHashCode()
		{
			int result = 0;

			for (int row = 0; row < RowCount; row++)
				for (int column = 0; column < ColumnCount; column++)
					result ^= values[row, column].GetHashCode();

			return result;
		}
		public override string ToString()
		{
			StringBuilder result = new StringBuilder();

			for (int row = 0; row < RowCount; row++)
			{
				for (int column = 0; column < ColumnCount; column++)
				{
					result.Append(values[row, column]);
					result.Append(", ");
				}
				result.Remove(result.Length - 2, 2);
				result.AppendLine();
			}

			return result.ToString();
		}
		public bool Equals(Matrix other)
		{
			return this == other;
		}

		public Matrix Power(int exponent)
		{
			if (RowCount != ColumnCount) throw new InvalidOperationException();

			Matrix matrix = Identity(Items.Equal(RowCount, ColumnCount));

			for (int i = 0; i < exponent; i++) matrix *= this;

			return matrix;
		}

		public static bool operator ==(Matrix matrix1, Matrix matrix2)
		{
			if (matrix1.RowCount != matrix2.RowCount) throw new ArgumentException("The row counts do not match.");
			if (matrix1.ColumnCount != matrix2.ColumnCount) throw new ArgumentException("The column counts do not match.");

			int rows = Items.Equal(matrix1.RowCount, matrix2.RowCount);
			int columns = Items.Equal(matrix1.ColumnCount, matrix2.ColumnCount);

			for (int row = 0; row < rows; row++)
				for (int column = 0; column < columns; column++)
					if (matrix1[row, column] != matrix2[row, column])
						return false;

			return true;
		}
		public static bool operator !=(Matrix matrix1, Matrix matrix2)
		{
			if (matrix1.RowCount != matrix2.RowCount) throw new ArgumentException("The row counts do not match.");
			if (matrix1.ColumnCount != matrix2.ColumnCount) throw new ArgumentException("The column counts do not match.");

			int rows = Items.Equal(matrix1.RowCount, matrix2.RowCount);
			int columns = Items.Equal(matrix1.ColumnCount, matrix2.ColumnCount);

			for (int row = 0; row < rows; row++)
				for (int column = 0; column < columns; column++)
					if (matrix1[row, column] != matrix2[row, column])
						return true;

			return false;
		}

		public static Matrix operator +(Matrix matrix1, Matrix matrix2)
		{
			if (matrix1.RowCount != matrix2.RowCount) throw new ArgumentException("The row counts do not match.");
			if (matrix1.ColumnCount != matrix2.ColumnCount) throw new ArgumentException("The column counts do not match.");

			int rows = Items.Equal(matrix1.RowCount, matrix2.RowCount);
			int columns = Items.Equal(matrix1.ColumnCount, matrix2.ColumnCount);

			Matrix result = new Matrix(rows, columns);

			for (int row = 0; row < rows; row++)
				for (int column = 0; column < columns; column++)
					result[row, column] = matrix1[row, column] + matrix2[row, column];

			return result;
		}
		public static Matrix operator -(Matrix matrix1, Matrix matrix2)
		{
			if (matrix1.RowCount != matrix2.RowCount) throw new ArgumentException("The row counts do not match.");
			if (matrix1.ColumnCount != matrix2.ColumnCount) throw new ArgumentException("The column counts do not match.");

			int rows = Items.Equal(matrix1.RowCount, matrix2.RowCount);
			int columns = Items.Equal(matrix1.ColumnCount, matrix2.ColumnCount);

			Matrix result = new Matrix(rows, columns);

			for (int row = 0; row < rows; row++)
				for (int column = 0; column < columns; column++)
					result[row, column] = matrix1[row, column] - matrix2[row, column];

			return result;
		}
		public static Matrix operator *(Matrix matrix1, Matrix matrix2)
		{
			if (matrix1.ColumnCount != matrix2.RowCount) throw new ArgumentOutOfRangeException();

			int size = Items.Equal(matrix1.ColumnCount, matrix2.RowCount);

			Matrix result = new Matrix(matrix1.RowCount, matrix2.ColumnCount);

			for (int row = 0; row < matrix1.RowCount; row++)
				for (int column = 0; column < matrix2.ColumnCount; column++)
					for (int i = 0; i < size; i++)
						result[row, column] += matrix1[row, i] * matrix2[i, column];

			return result;
		}
		public static Matrix operator *(double factor, Matrix matrix)
		{
			Matrix result = new Matrix(matrix.RowCount, matrix.ColumnCount);

			for (int row = 0; row < matrix.RowCount; row++)
				for (int column = 0; column < matrix.ColumnCount; column++)
					result[row, column] = matrix[row, column] * factor;

			return result;
		}
		public static Matrix operator *(Matrix matrix, double factor)
		{
			Matrix result = new Matrix(matrix.RowCount, matrix.ColumnCount);

			for (int row = 0; row < matrix.RowCount; row++)
				for (int column = 0; column < matrix.ColumnCount; column++)
					result[row, column] = matrix[row, column] * factor;

			return result;
		}

		public static Matrix Identity(int size)
		{
			Matrix matrix = new Matrix(size, size);

			for (int i = 0; i < size; i++) matrix[i, i] = 1;

			return matrix;
		}
	}
}
