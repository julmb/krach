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
				MatrixComplex MatrixComplex = new MatrixComplex(Rows, Columns);

				for (int row = 0; row < Rows; row++)
					for (int column = 0; column < Columns; column++)
						MatrixComplex[row, column] = this[column, row];

				return MatrixComplex;
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
		public MatrixComplex(Complex[] values)
			: this(values.Length, 1)
		{
			for (int row = 0; row < Rows; row++) this[row, 0] = values[row];
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
		public bool Equals(MatrixComplex other)
		{
			return this == other;
		}
		public Complex[] ToValues()
		{
			if (Columns != 1) throw new InvalidOperationException("Cannot convert non-Vector to values.");

			Complex[] values = new Complex[Rows];

			for (int row = 0; row < Rows; row++) values[row] = this[row, 0];

			return values;
		}

		public MatrixComplex Power(int exponent)
		{
			if (Rows != Columns) throw new InvalidOperationException();

			MatrixComplex MatrixComplex = Identity(Items.Equal(Rows, Columns));

			for (int i = 0; i < exponent; i++) MatrixComplex *= this;

			return MatrixComplex;
		}

		public static bool operator ==(MatrixComplex MatrixComplex1, MatrixComplex MatrixComplex2)
		{
			if (MatrixComplex1.Rows != MatrixComplex2.Rows) throw new ArgumentException("The row counts do not match.");
			if (MatrixComplex1.Columns != MatrixComplex2.Columns) throw new ArgumentException("The column counts do not match.");

			int rows = Items.Equal(MatrixComplex1.Rows, MatrixComplex2.Rows);
			int columns = Items.Equal(MatrixComplex1.Columns, MatrixComplex2.Columns);

			for (int row = 0; row < rows; row++)
				for (int column = 0; column < columns; column++)
					if (MatrixComplex1[row, column] != MatrixComplex2[row, column])
						return false;

			return true;
		}
		public static bool operator !=(MatrixComplex MatrixComplex1, MatrixComplex MatrixComplex2)
		{
			if (MatrixComplex1.Rows != MatrixComplex2.Rows) throw new ArgumentException("The row counts do not match.");
			if (MatrixComplex1.Columns != MatrixComplex2.Columns) throw new ArgumentException("The column counts do not match.");

			int rows = Items.Equal(MatrixComplex1.Rows, MatrixComplex2.Rows);
			int columns = Items.Equal(MatrixComplex1.Columns, MatrixComplex2.Columns);

			for (int row = 0; row < rows; row++)
				for (int column = 0; column < columns; column++)
					if (MatrixComplex1[row, column] != MatrixComplex2[row, column])
						return true;

			return false;
		}

		public static MatrixComplex operator +(MatrixComplex MatrixComplex1, MatrixComplex MatrixComplex2)
		{
			if (MatrixComplex1.Rows != MatrixComplex2.Rows) throw new ArgumentException("The row counts do not match.");
			if (MatrixComplex1.Columns != MatrixComplex2.Columns) throw new ArgumentException("The column counts do not match.");

			int rows = Items.Equal(MatrixComplex1.Rows, MatrixComplex2.Rows);
			int columns = Items.Equal(MatrixComplex1.Columns, MatrixComplex2.Columns);

			MatrixComplex result = new MatrixComplex(rows, columns);

			for (int row = 0; row < rows; row++)
				for (int column = 0; column < columns; column++)
					result[row, column] = MatrixComplex1[row, column] + MatrixComplex2[row, column];

			return result;
		}
		public static MatrixComplex operator -(MatrixComplex MatrixComplex1, MatrixComplex MatrixComplex2)
		{
			if (MatrixComplex1.Rows != MatrixComplex2.Rows) throw new ArgumentException("The row counts do not match.");
			if (MatrixComplex1.Columns != MatrixComplex2.Columns) throw new ArgumentException("The column counts do not match.");

			int rows = Items.Equal(MatrixComplex1.Rows, MatrixComplex2.Rows);
			int columns = Items.Equal(MatrixComplex1.Columns, MatrixComplex2.Columns);

			MatrixComplex result = new MatrixComplex(rows, columns);

			for (int row = 0; row < rows; row++)
				for (int column = 0; column < columns; column++)
					result[row, column] = MatrixComplex1[row, column] - MatrixComplex2[row, column];

			return result;
		}
		public static MatrixComplex operator *(MatrixComplex MatrixComplex1, MatrixComplex MatrixComplex2)
		{
			if (MatrixComplex1.Columns != MatrixComplex2.Rows) throw new ArgumentOutOfRangeException();

			int size = Items.Equal(MatrixComplex1.Columns, MatrixComplex2.Rows);

			MatrixComplex result = new MatrixComplex(MatrixComplex1.Rows, MatrixComplex2.Columns);

			for (int row = 0; row < MatrixComplex1.Rows; row++)
				for (int column = 0; column < MatrixComplex2.Columns; column++)
					for (int i = 0; i < size; i++)
						result[row, column] += MatrixComplex1[row, i] * MatrixComplex2[i, column];

			return result;
		}
		public static MatrixComplex operator *(Complex factor, MatrixComplex MatrixComplex)
		{
			MatrixComplex result = new MatrixComplex(MatrixComplex.Rows, MatrixComplex.Columns);

			for (int row = 0; row < MatrixComplex.Rows; row++)
				for (int column = 0; column < MatrixComplex.Columns; column++)
					result[row, column] = MatrixComplex[row, column] * factor;

			return result;
		}
		public static MatrixComplex operator *(MatrixComplex MatrixComplex, Complex factor)
		{
			MatrixComplex result = new MatrixComplex(MatrixComplex.Rows, MatrixComplex.Columns);

			for (int row = 0; row < MatrixComplex.Rows; row++)
				for (int column = 0; column < MatrixComplex.Columns; column++)
					result[row, column] = MatrixComplex[row, column] * factor;

			return result;
		}

		public static MatrixComplex Identity(int size)
		{
			MatrixComplex MatrixComplex = new MatrixComplex(size, size);

			for (int i = 0; i < size; i++) MatrixComplex[i, i] = 1;

			return MatrixComplex;
		}
	}
}
