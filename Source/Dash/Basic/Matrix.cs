using System;
using System.Text;

namespace Matrices
{
	public struct Matrix : IEquatable<Matrix>
	{
		public readonly double[,] values;

		public double this[int row, int column]
		{
			get { return values[row, column]; }
			set { values[row, column] = value; }
		}
		public int Rows { get { return values.GetLength(0); } }
		public int Columns { get { return values.GetLength(1); } }
		public Matrix Transpose
		{
			get
			{
				Matrix matrix = new Matrix(Rows, Columns);

				for (int row = 0; row < Rows; row++)
					for (int column = 0; column < Columns; column++)
						matrix[row, column] = this[column, row];

				return matrix;
			}
		}

		public Matrix(int rows, int columns)
		{
			values = new double[rows, columns];
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

			for (int row = 0; row < Rows; row++)
				for (int column = 0; column < Columns; column++)
					result ^= this[row, column].GetHashCode();

			return result;
		}
		public override string ToString()
		{
			StringBuilder result = new StringBuilder();

			for (int row = 0; row < Rows; row++)
			{
				for (int column = 0; column < Columns; column++)
				{
					result.Append(this[row, column]);
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
			if (Rows != Columns) throw new InvalidOperationException();

			Matrix matrix = Identity((Rows + Columns) / 2);

			for (int i = 0; i < exponent; i++) matrix *= this;

			return matrix;
		}

		public static bool operator ==(Matrix matrix1, Matrix matrix2)
		{
			if (matrix1.Rows != matrix2.Rows) throw new ArgumentOutOfRangeException();
			if (matrix1.Columns != matrix2.Columns) throw new ArgumentOutOfRangeException();

			int rows = (matrix1.Rows + matrix2.Rows) / 2;
			int columns = (matrix1.Columns + matrix2.Columns) / 2;

			for (int row = 0; row < rows; row++)
				for (int column = 0; column < columns; column++)
					if (matrix1[row, column] != matrix2[row, column])
						return false;

			return true;
		}
		public static bool operator !=(Matrix matrix1, Matrix matrix2)
		{
			if (matrix1.Rows != matrix2.Rows) throw new ArgumentOutOfRangeException();
			if (matrix1.Columns != matrix2.Columns) throw new ArgumentOutOfRangeException();

			int rows = (matrix1.Rows + matrix2.Rows) / 2;
			int columns = (matrix1.Columns + matrix2.Columns) / 2;

			for (int row = 0; row < rows; row++)
				for (int column = 0; column < columns; column++)
					if (matrix1[row, column] != matrix2[row, column])
						return true;

			return false;
		}

		public static Matrix operator +(Matrix matrix1, Matrix matrix2)
		{
			if (matrix1.Rows != matrix2.Rows) throw new ArgumentOutOfRangeException();
			if (matrix1.Columns != matrix2.Columns) throw new ArgumentOutOfRangeException();

			int rows = (matrix1.Rows + matrix2.Rows) / 2;
			int columns = (matrix1.Columns + matrix2.Columns) / 2;

			Matrix result = new Matrix(rows, columns);

			for (int row = 0; row < rows; row++)
				for (int column = 0; column < columns; column++)
					result[row, column] = matrix1[row, column] + matrix2[row, column];

			return result;
		}
		public static Matrix operator *(Matrix matrix1, Matrix matrix2)
		{
			if (matrix1.Columns != matrix2.Rows) throw new ArgumentOutOfRangeException();

			int size = (matrix1.Columns + matrix2.Rows) / 2;

			Matrix result = new Matrix(matrix1.Rows, matrix2.Columns);

			for (int row = 0; row < matrix1.Rows; row++)
				for (int column = 0; column < matrix2.Columns; column++)
					for (int i = 0; i < size; i++)
						result[row, column] += matrix1[row, i] * matrix2[i, column];

			return result;
		}
		public static Matrix operator *(double factor, Matrix matrix)
		{
			Matrix result = new Matrix(matrix.Rows, matrix.Columns);

			for (int row = 0; row < matrix.Rows; row++)
				for (int column = 0; column < matrix.Columns; column++)
					result[row, column] = matrix[row, column] * factor;

			return result;
		}
		public static Matrix operator *(Matrix matrix, double factor)
		{
			Matrix result = new Matrix(matrix.Rows, matrix.Columns);

			for (int row = 0; row < matrix.Rows; row++)
				for (int column = 0; column < matrix.Columns; column++)
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
