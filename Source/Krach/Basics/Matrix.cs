// Copyright © Julian Brunner 2010 - 2012

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
using System.Linq;
using Krach.Extensions;
using System.Collections.Generic;

namespace Krach.Basics
{
	public struct Matrix : IEquatable<Matrix>
	{
		public readonly double[,] values;

		public double this[int rowIndex, int columnIndex]
		{
			get { return values[rowIndex, columnIndex]; }
			set { values[rowIndex, columnIndex] = value; }
		}
		public int RowCount { get { return values.GetLength(0); } }
		public int ColumnCount { get { return values.GetLength(1); } }
		public Vector2Integer Size { get { return new Vector2Integer(RowCount, ColumnCount); } }
		public Matrix Transpose
		{
			get
			{
				Matrix result = new Matrix(ColumnCount, RowCount);

				for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
					for (int columnIndex = 0; columnIndex < ColumnCount; columnIndex++)
						result[columnIndex, rowIndex] = values[rowIndex, columnIndex];

				return result;
			}
		}
		public IEnumerable<IEnumerable<double>> Rows { get { for (int rowIndex = 0; rowIndex < RowCount; rowIndex++) yield return GetRow(rowIndex); } }
		public IEnumerable<IEnumerable<double>> Columns { get { for (int columnIndex = 0; columnIndex < ColumnCount; columnIndex++) yield return GetColumn(columnIndex); } }

		public Matrix(int rowCount, int columnCount)
		{
			if (rowCount <= 0) throw new ArgumentOutOfRangeException("rowCount");
			if (columnCount <= 0) throw new ArgumentOutOfRangeException("columnCount");

			this.values = new double[rowCount, columnCount];
		}
		public Matrix(double[,] values)
		{
			if (values.GetLength(0) == 0) throw new ArgumentOutOfRangeException("values");
			if (values.GetLength(1) == 0) throw new ArgumentOutOfRangeException("values");

			this.values = values;
		}

		public override bool Equals(object obj)
		{
			return obj is Matrix && this == (Matrix)obj;
		}
		public override int GetHashCode()
		{
			int result = 0;

			for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
				for (int columnIndex = 0; columnIndex < ColumnCount; columnIndex++)
					result ^= values[rowIndex, columnIndex].GetHashCode();

			return result;
		}
		public override string ToString()
		{
			StringBuilder result = new StringBuilder();

			for (int rowIndex = 0; rowIndex < RowCount; rowIndex++)
			{
				for (int columnIndex = 0; columnIndex < ColumnCount; columnIndex++)
				{
					result.Append(values[rowIndex, columnIndex]);
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

			Matrix matrix = CreateIdentity(Items.Equal(RowCount, ColumnCount));

			for (int i = 0; i < exponent; i++) matrix *= this;

			return matrix;
		
		}
		public IEnumerable<double> GetRow(int rowIndex)
		{
			for (int columnIndex = 0; columnIndex < ColumnCount; columnIndex++) yield return values[rowIndex, columnIndex];
		}
		public IEnumerable<double> GetColumn(int columnIndex)
		{
			for (int rowIndex = 0; rowIndex < RowCount; rowIndex++) yield return values[rowIndex, columnIndex];
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

		public static Matrix CreateIdentity(int size)
		{
			if (size <= 0) throw new ArgumentOutOfRangeException("size");

			Matrix matrix = new Matrix(size, size);

			for (int i = 0; i < size; i++) matrix [i, i] = 1;

			return matrix;
		}
		public static Matrix CreateSingleton(double value)
		{
			return new Matrix(new double[,] { { value } });
		}
		public static Matrix FromColumnVectors(IEnumerable<Matrix> vectors)
		{
			if (vectors == null) throw new ArgumentNullException("vectors");

			vectors = vectors.ToArray();

			IEnumerable<Vector2Integer> vectorSizes = vectors.Select(vector => vector.Size);
			if (vectorSizes.Distinct().Count() != 1) throw new ArgumentException("Vectors in parameter 'vectors' are not all the same size.");
			Vector2Integer vectorSize = vectorSizes.Distinct().Single();
			if (vectorSize.Y != 1) throw new ArgumentException("Vectors in parameter 'vectors' are not column vectors.");

			Matrix result = new Matrix(vectorSize.X, vectors.Count());
			for (int rowIndex = 0; rowIndex < result.RowCount; rowIndex++)
				for (int columnIndex = 0; columnIndex < result.ColumnCount; columnIndex++)
					result[rowIndex, columnIndex] = vectors.ElementAt(columnIndex)[rowIndex, 0];

			return result;
		}
		public static Matrix FromRowVectors(IEnumerable<Matrix> vectors)
		{
			if (vectors == null) throw new ArgumentNullException("vectors");

			vectors = vectors.ToArray();

			IEnumerable<Vector2Integer> vectorSizes = vectors.Select(vector => vector.Size);
			if (vectorSizes.Distinct().Count() != 1) throw new ArgumentException("Vectors in parameter 'vectors' are not all the same size.");
			Vector2Integer vectorSize = vectorSizes.Distinct().Single();
			if (vectorSize.X != 1) throw new ArgumentException("Vectors in parameter 'vectors' are not row vectors.");

			Matrix result = new Matrix(vectors.Count(), vectorSize.Y);
			for (int rowIndex = 0; rowIndex < result.RowCount; rowIndex++)
				for (int columnIndex = 0; columnIndex < result.ColumnCount; columnIndex++)
					result[rowIndex, columnIndex] = vectors.ElementAt(rowIndex)[0, columnIndex];

			return result;
		}
	}
}
