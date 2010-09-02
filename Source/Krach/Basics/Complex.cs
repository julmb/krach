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
using Krach.Extensions;

namespace Krach.Basics
{
	public struct Complex : IEquatable<Complex>
	{
		readonly double real;
		readonly double imaginary;

		public static Complex Zero { get { return new Complex(0, 0); } }
		public static Complex RealOne { get { return new Complex(1, 0); } }
		public static Complex ImaginaryOne { get { return new Complex(0, 1); } }

		public double Real { get { return real; } }
		public double Imaginary { get { return imaginary; } }
		public double Absolute { get { return Scalars.SquareRoot(real.Square() + imaginary.Square()); } }
		public Complex Conjugate { get { return new Complex(real, -imaginary); } }

		public Complex(double real, double imaginary)
		{
			this.real = real;
			this.imaginary = imaginary;
		}

		public override bool Equals(object obj)
		{
			return obj is Complex && this == (Complex)obj;
		}
		public override int GetHashCode()
		{
			return real.GetHashCode() ^ imaginary.GetHashCode();
		}
		public override string ToString()
		{
			double real = this.real.Round(0.00000001);
			double imaginary = this.imaginary.Round(0.00000001);

			if (real == 0 && imaginary == 0) return string.Format("0");
			if (real != 0 && imaginary == 0) return string.Format("{0}r", real);
			if (real == 0 && imaginary != 0) return string.Format("{0}i", imaginary);
			if (real != 0 && imaginary != 0) return string.Format("{0}r|{1}i", real, imaginary);

			throw new InvalidOperationException();
		}
		public bool Equals(Complex other)
		{
			return this == other;
		}

		public static bool operator ==(Complex complex1, Complex complex2)
		{
			return complex1.real == complex2.real && complex1.imaginary == complex2.imaginary;
		}
		public static bool operator !=(Complex complex1, Complex complex2)
		{
			return complex1.real != complex2.real || complex1.imaginary != complex2.imaginary;
		}

		public static Complex operator +(Complex complex1, Complex complex2)
		{
			return new Complex(complex1.real + complex2.real, complex1.imaginary + complex2.imaginary);
		}
		public static Complex operator -(Complex complex1, Complex complex2)
		{
			return new Complex(complex1.real - complex2.real, complex1.imaginary - complex2.imaginary);
		}
		public static Complex operator *(Complex complex1, Complex complex2)
		{
			return new Complex
			(
				complex1.real * complex2.real - complex1.imaginary * complex2.imaginary,
				complex1.real * complex2.imaginary + complex1.imaginary * complex2.real
			);
		}
		public static Complex operator /(Complex complex1, Complex complex2)
		{
			double divisor = complex2.real.Square() + complex2.imaginary.Square();

			if (divisor == 0) throw new DivideByZeroException();

			return new Complex
			(
				(complex1.real * complex2.real + complex1.imaginary * complex2.imaginary) / divisor,
				(complex1.real * complex2.imaginary - complex1.imaginary * complex2.real) / divisor
			);
		}

		public static Complex operator +(Complex complex)
		{
			return Zero + complex;
		}
		public static Complex operator -(Complex complex)
		{
			return Zero - complex;
		}

		public static implicit operator Complex(double value)
		{
			return new Complex(value, 0);
		}
	}
}
