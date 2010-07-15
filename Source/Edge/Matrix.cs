using System;
using System.Text;

namespace Matrices
{
	struct Matrix
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
				Matrix m = new Matrix(Rows, Columns);

				for (int row = 0; row < Rows; row++)
					for (int column = 0; column < Columns; column++)
						m[row, column] = this[column, row];

				return m;
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

		public Matrix Power(int n)
		{
			if (Rows != Columns) throw new InvalidOperationException();

			Matrix m = Identity((Rows + Columns) / 2);

			for (int i = 0; i < n; i++) m *= this;

			return m;
		}

		public static Matrix Identity(int size)
		{
			Matrix m = new Matrix(size, size);

			for (int i = 0; i < size; i++) m[i, i] = 1;

			return m;
		}

		public static Matrix operator +(Matrix a, Matrix b)
		{
			if (a.Rows != b.Rows) throw new ArgumentOutOfRangeException();
			if (a.Columns != b.Columns) throw new ArgumentOutOfRangeException();

			int rows = (a.Rows + b.Rows) / 2;
			int columns = (a.Columns + b.Columns) / 2;

			Matrix m = new Matrix(rows, columns);

			for (int row = 0; row < rows; row++)
				for (int column = 0; column < columns; column++)
					m[row, column] = a[row, column] + b[row, column];

			return m;
		}
		public static Matrix operator *(Matrix a, Matrix b)
		{
			if (a.Columns != b.Rows) throw new ArgumentOutOfRangeException();

			int n = (a.Columns + b.Rows) / 2;

			Matrix m = new Matrix(a.Rows, b.Columns);

			for (int row = 0; row < a.Rows; row++)
				for (int column = 0; column < b.Columns; column++)
					for (int i = 0; i < n; i++)
						m[row, column] += a[row, i] * b[i, column];

			return m;
		}
		public static Matrix operator *(double d, Matrix m)
		{
			Matrix result = new Matrix(m.Rows, m.Columns);

			for (int row = 0; row < m.Rows; row++)
				for (int column = 0; column < m.Columns; column++)
					result[row, column] = m[row, column] * d;

			return result;
		}
		public static Matrix operator *(Matrix m, double d)
		{
			Matrix result = new Matrix(m.Rows, m.Columns);

			for (int row = 0; row < m.Rows; row++)
				for (int column = 0; column < m.Columns; column++)
					result[row, column] = m[row, column] * d;

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
	}
}
