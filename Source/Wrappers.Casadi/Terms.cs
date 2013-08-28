using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;
using System.Runtime.InteropServices;
using Krach.Basics;

namespace Wrappers.Casadi
{
	public static class Terms
	{
		// lambda terms
		public static ValueTerm Variable(string name, int dimension)
		{
			return TermsWrapped.Variable(name, dimension);
		}
		public static ValueTerm Variable(string name)
		{
			return Variable(name, 1);
		}
		public static FunctionTerm Abstraction(ValueTerm variable, ValueTerm value)
		{
			return TermsWrapped.Abstraction(variable, value);
		}
		public static FunctionTerm Abstract(this ValueTerm value, IEnumerable<ValueTerm> variables)
		{
			return Abstraction(Vector(variables), value);
		}
		public static FunctionTerm Abstract(this ValueTerm value, params ValueTerm[] variables)
		{
			return value.Abstract((IEnumerable<ValueTerm>)variables);
		}
		public static ValueTerm Application(FunctionTerm function, ValueTerm value)
		{
			return TermsWrapped.Application(function, value);
		}
		public static ValueTerm Apply(this FunctionTerm function, IEnumerable<ValueTerm> parameters)
		{
			return Application(function, Vector(parameters));
		}
		public static ValueTerm Apply(this FunctionTerm function, params ValueTerm[] parameters)
		{
			return function.Apply((IEnumerable<ValueTerm>)parameters);
		}

		// vectors
		public static ValueTerm Vector(IEnumerable<ValueTerm> values)
		{
			return TermsWrapped.Vector(values);
		}
		public static ValueTerm Vector(params ValueTerm[] values)
		{
			return Vector((IEnumerable<ValueTerm>)values);
		}
		public static ValueTerm Selection(ValueTerm value, int index)
		{
			return TermsWrapped.Selection(value, index);
		}
		public static ValueTerm Select(this ValueTerm value, int index)
		{
			return Selection(value, index);
		}

		// constants
		public static ValueTerm Constant(double value)
		{
			return TermsWrapped.Constant(value);
		}
		public static ValueTerm Constant(IEnumerable<double> values)
		{
			return Vector(values.Select(Constant).ToArray());
		}
		public static ValueTerm Constant(params double[] values)
		{
			return Constant((IEnumerable<double>)values);
		}

		// common operations
		public static ValueTerm Sum(ValueTerm value1, ValueTerm value2)
		{
			if (value1.Dimension != value2.Dimension) throw new ArgumentException("Dimensions of 'value1' and 'value2' do not match.");

			return TermsWrapped.Sum(value1, value2);
		}
		public static ValueTerm Sum(IEnumerable<ValueTerm> values)
		{
			if (!values.Any()) throw new ArgumentException("Parameter 'values' does not contain any items.");

			return values.Aggregate(Sum);
		}
		public static ValueTerm Sum(params ValueTerm[] values)
		{
			return Sum((IEnumerable<ValueTerm>)values);
		}
		public static ValueTerm Negate(ValueTerm value)
		{
			return Scaling(Constant(-1), value);
		}
		public static ValueTerm Difference(ValueTerm value1, ValueTerm value2)
		{
			return Sum(value1, Negate(value2));
		}
		public static ValueTerm Product(ValueTerm value1, ValueTerm value2)
		{
			if (value1.Dimension != 1) throw new ArgumentException("Dimension of 'value1' is not 1.");
			if (value2.Dimension != 1) throw new ArgumentException("Dimension of 'value2' is not 1.");

			return TermsWrapped.Product(value1, value2);
		}
		public static ValueTerm Product(IEnumerable<ValueTerm> values)
		{
			if (!values.Any()) throw new ArgumentException("Parameter 'values' does not contain any items.");

			return values.Aggregate(Product);
		}
		public static ValueTerm Product(params ValueTerm[] values)
		{
			return Product((IEnumerable<ValueTerm>)values);
		}
		public static ValueTerm DotProduct(ValueTerm value1, ValueTerm value2)
		{
			if (value1.Dimension != value2.Dimension) throw new ArgumentException("Dimensions of 'value1' and 'value2' do not match.");

			return TermsWrapped.MatrixProduct(TermsWrapped.Transpose(value1), value2);
		}
		public static ValueTerm Scaling(ValueTerm factor, ValueTerm value)
		{
			if (factor.Dimension != 1) throw new ArgumentException("Dimension of 'factor' is not 1.");

			return TermsWrapped.Product(factor, value);
		}
		public static ValueTerm Invert(ValueTerm value)
		{
			return Exponentiation(value, Constant(-1));
		}
		public static ValueTerm Quotient(ValueTerm value1, ValueTerm value2)
		{
			return Product(value1, Invert(value2));
		}
		public static ValueTerm Exponentiation(ValueTerm @base, ValueTerm exponent)
		{
			if (@base.Dimension != 1) throw new ArgumentException("Dimension of 'base' is not 1.");
			if (exponent.Dimension != 1) throw new ArgumentException("Dimension of 'exponent' is not 1.");

			return TermsWrapped.Exponentiation(@base, exponent);
		}
		public static ValueTerm Square(ValueTerm value)
		{
			return Exponentiation(value, Constant(2));
		}
		public static ValueTerm SquareRoot(ValueTerm value)
		{
			return Exponentiation(value, Constant(0.5));
		}

		// trigonometry
		public static ValueTerm Sine(ValueTerm value)
		{
			if (value.Dimension != 1) throw new ArgumentException("Dimension of 'value' is not 1.");

			return TermsWrapped.Sine(value);
		}
		public static ValueTerm ArcSine(ValueTerm value)
		{
			if (value.Dimension != 1) throw new ArgumentException("Dimension of 'value' is not 1.");

			return TermsWrapped.ArcSine(value);
		}
		public static ValueTerm Cosine(ValueTerm value)
		{
			if (value.Dimension != 1) throw new ArgumentException("Dimension of 'value' is not 1.");

			return TermsWrapped.Cosine(value);
		}
		public static ValueTerm ArcCosine(ValueTerm value)
		{
			if (value.Dimension != 1) throw new ArgumentException("Dimension of 'value' is not 1.");

			return TermsWrapped.ArcCosine(value);
		}
		public static ValueTerm Tangent(ValueTerm value)
		{
			if (value.Dimension != 1) throw new ArgumentException("Dimension of 'value' is not 1.");

			return TermsWrapped.Tangent(value);
		}
		public static ValueTerm ArcTangent(ValueTerm value)
		{
			if (value.Dimension != 1) throw new ArgumentException("Dimension of 'value' is not 1.");

			return TermsWrapped.ArcTangent(value);
		}

		// polar coordinates
		public static ValueTerm CartesianToPolar(ValueTerm value)
		{
			if (value.Dimension != 2) throw new ArgumentException("Dimension of 'value' is not 2.");

			return Vector(Norm(value), Angle(value));
		}
		public static ValueTerm PolarToCartesian(ValueTerm value)
		{
			if (value.Dimension != 2) throw new ArgumentException("Dimension of 'value' is not 2.");

			return Scaling(value.Select(0), Direction(value.Select(1)));
		}

		public static ValueTerm Norm(ValueTerm value)
		{
			return SquareRoot(DotProduct(value, value));
		}
		public static ValueTerm NormSquared(ValueTerm value)
		{
			return DotProduct(value, value);
		}
		public static ValueTerm Angle(ValueTerm value)
		{
			if (value.Dimension != 2) throw new ArgumentException("Dimension of 'value' is not 2.");

			return TermsWrapped.ArcTangent2(value.Select(1), value.Select(0));
		}
		public static ValueTerm Direction(ValueTerm value)
		{
			if (value.Dimension != 1) throw new ArgumentException("Dimension of 'value' is not 1.");

			return Vector(Cosine(value), Sine(value));
		}
		public static ValueTerm Normalize(ValueTerm value)
		{
			return Scaling(Invert(Norm(value)), value);
		}
		public static ValueTerm Normal(ValueTerm value)
		{
			if (value.Dimension != 2) throw new ArgumentException("Dimension of 'value' is not 2.");

			return Vector(Negate(value.Select(1)), value.Select(0));
		}

		public static IEnumerable<FunctionTerm> StandardPolynomialBasis(int length)
		{
			if (length < 0) throw new ArgumentOutOfRangeException("length");

			ValueTerm variable = Variable("x");

			return
			(
				from index in Enumerable.Range(0, length)
				select Exponentiation(variable, Constant(index)).Abstract(variable)
			)
			.ToArray();
		}
		public static IEnumerable<FunctionTerm> BernsteinPolynomialBasis(int length)
		{
			if (length < 0) throw new ArgumentOutOfRangeException("length");

			ValueTerm variable = Variable("x");

			int degree = length - 1;

			return
			(
				from index in Enumerable.Range(0, length)
				select Product
				(
					Constant(Scalars.BinomialCoefficient(degree, index)),
					Exponentiation(variable, Constant(index)),
					Exponentiation(Difference(Constant(1), variable), Constant(degree - index))
				)
				.Abstract(variable)
			)
			.ToArray();
		}
		public static FunctionTerm Polynomial(IEnumerable<FunctionTerm> basis, int dimension)
		{
			if (basis == null) throw new ArgumentNullException("basis");
			if (dimension < 0) throw new ArgumentOutOfRangeException("dimension");

			ValueTerm variable = Variable("x");
			IEnumerable<ValueTerm> coefficients =
			(
				from index in Enumerable.Range(0, basis.Count())
				select Variable(string.Format("c_{0}", index), dimension)
			)
			.ToArray();
			IEnumerable<ValueTerm> parameters = Enumerables.Concatenate(Enumerables.Create(variable), coefficients);

			if (!basis.Any()) return Constant(Enumerable.Repeat(0.0, dimension)).Abstract(parameters);

			IEnumerable<ValueTerm> basisValues = basis.Select(basisFunction => basisFunction.Apply(variable));

			return Sum(Enumerable.Zip(basisValues, coefficients, Scaling)).Abstract(parameters);
		}
		public static ValueTerm IntegrateTrapezoid(FunctionTerm function, OrderedRange<double> bounds, int segmentCount)
		{
			if (segmentCount < 1) throw new ArgumentOutOfRangeException("segmentCount");

			ValueTerm segmentWidth = Terms.Constant(bounds.Length() / segmentCount);

			IEnumerable<ValueTerm> values =
			(
				from segmentPosition in Scalars.GetIntermediateValuesSymmetric(bounds.Start, bounds.End, segmentCount)
				select function.Apply(Terms.Constant(segmentPosition))
			)
			.ToArray();

			return Terms.Product
			(
				segmentWidth,
				Terms.Sum
				(
					Enumerables.Concatenate
					(
						Enumerables.Create(Terms.Product(Terms.Constant(0.5), values.First())),
						values.Skip(1).SkipLast(1),
						Enumerables.Create(Terms.Product(Terms.Constant(0.5), values.Last()))
					)
				)
			);
		}
	}
}

