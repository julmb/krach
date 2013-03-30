using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;
using Krach.Calculus.Terms.Composite;
using Krach.Calculus.Terms;
using Krach.Calculus.Terms.Basic;
using Krach.Calculus.Terms.Basic.Atoms;
using Krach.Calculus.Terms.Basic.Definitions;
using Krach.Calculus.Terms.Notation;
using Krach.Calculus.Terms.Notation.Basic;

namespace Krach.Calculus
{
	public static class Term
	{
		public static ValueTerm Variable(int dimension, string name) 
		{
			return new Variable(dimension, name);
		}
		public static ValueTerm Variable(string name)
		{
			return Variable(1, name);
		}
		public static FunctionTerm Abstract(this ValueTerm term, IEnumerable<Variable> variables) 
		{
			return new Abstraction(variables, term);
		}
		public static FunctionTerm Abstract(this ValueTerm term, params Variable[] variables) 
		{
			return term.Abstract((IEnumerable<Variable>)variables);
		}
		public static ValueTerm Apply(this FunctionTerm function, IEnumerable<ValueTerm> parameters)
		{
			return new Application(function, Vector(parameters));
		}
		public static ValueTerm Apply(this FunctionTerm function, params ValueTerm[] parameters)
		{
			return function.Apply((IEnumerable<ValueTerm>)parameters);
		}
		
		public static ValueTerm Vector(IEnumerable<ValueTerm> terms)
		{
			return new Vector(terms);
		}
		public static ValueTerm Vector(params ValueTerm[] terms)
		{
			return Vector((IEnumerable<ValueTerm>)terms);
		}
		public static ValueTerm Select(this ValueTerm term, int index)
		{
			return new Selection(term, index);
		}

		public static ValueTerm Constant(IEnumerable<double> values)
		{
			return Vector(values.Select(value => new Constant(value)));
		}
		public static ValueTerm Constant(params double[] values)
		{
			return Constant((IEnumerable<double>)values);
		}

		static ValueTerm ApplySum(ValueTerm value1, ValueTerm value2)
		{
			return new Sum().Apply(value1, value2);
		}
		static ValueTerm ApplyVectorSum(ValueTerm value1, ValueTerm value2)
		{
			int dimension = Items.Equal(value1.Dimension, value2.Dimension);
			
			Variable x = new Variable(dimension, "x");
			Variable y = new Variable(dimension, "y");

			FunctionTerm vectorSum = new FunctionDefinition
			(
				Vector
				(
					from index in Enumerable.Range(0, dimension)
					select ApplySum(x.Select(index), y.Select(index))
				)
				.Abstract(x, y),
				new BinaryOperatorSyntax("⊕")
			);

			return vectorSum.Apply(value1, value2);
		}
		static ValueTerm ApplyBigSum(IEnumerable<ValueTerm> values)
		{
			if (values.Count() == 0) throw new ArgumentException("Parameter 'values' did not contain any items.");
			if (values.Count() == 1) return values.Single();
			if (values.Count() == 2) return ApplyVectorSum(values.ElementAt(0), values.ElementAt(1));

			int dimension = values.Select(value => value.Dimension).Distinct().Single();
			int length = values.Count();

			IEnumerable<Variable> variables =
			(
				from index in Enumerable.Range(0, length)
				select new Variable(dimension, string.Format("x{0}", index))
			)
			.ToArray();

			FunctionTerm bigSum = new FunctionDefinition
			(
				variables.Aggregate<ValueTerm>(ApplyVectorSum).Abstract(variables),
				new BasicFunctionSyntax("Σ")
			);

			return bigSum.Apply(values);
		}
		static ValueTerm ApplyProduct(ValueTerm value1, ValueTerm value2)
		{
			return new Product().Apply(value1, value2);
		}
		static ValueTerm ApplyVectorProduct(ValueTerm value1, ValueTerm value2)
		{
			int dimension = Items.Equal(value1.Dimension, value2.Dimension);
			
			Variable x = new Variable(dimension, "x");
			Variable y = new Variable(dimension, "y");

			FunctionTerm vectorProduct = new FunctionDefinition
			(
				Sum
				(
					from index in Enumerable.Range(0, dimension)
					select ApplyProduct(x.Select(index), y.Select(index))
				)
				.Abstract(x, y),
				new BinaryOperatorSyntax("⊙")
			);

			return vectorProduct.Apply(value1, value2);
		}
		static ValueTerm ApplyBigProduct(IEnumerable<ValueTerm> values)
		{
			if (values.Count() == 0) throw new ArgumentException("Parameter 'values' did not contain any items.");
			if (values.Count() == 1) return values.Single();
			if (values.Count() == 2) return ApplyVectorProduct(values.ElementAt(0), values.ElementAt(1));

			int dimension = values.Select(value => value.Dimension).Distinct().Single();
			int length = values.Count();

			IEnumerable<Variable> variables =
			(
				from index in Enumerable.Range(0, length)
				select new Variable(dimension, string.Format("x{0}", index))
			)
			.ToArray();

			FunctionTerm bigProduct = new FunctionDefinition
			(
				variables.Aggregate<ValueTerm>(ApplyVectorProduct).Abstract(variables),
				new BasicFunctionSyntax("π")
			);

			return bigProduct.Apply(values);
		}
		static ValueTerm ApplyVectorScaling(ValueTerm factor, ValueTerm value)
		{
			int dimension = value.Dimension;
			
			Variable c = new Variable(1, "c");
			Variable x = new Variable(dimension, "x");

			FunctionTerm vectorScaling = new FunctionDefinition
			(
				Vector
				(
					from index in Enumerable.Range(0, dimension)
					select ApplyProduct(c, x.Select(index))
				)
				.Abstract(c, x),
				new BinaryOperatorSyntax("⊛")
			);

			return vectorScaling.Apply(factor, value);
		}
		static ValueTerm ApplyExponentiation(ValueTerm @base, ValueTerm exponent)
		{
			return new Exponentiation().Apply(@base, exponent);
		}
		static ValueTerm ApplyLogarithm(ValueTerm value)
		{
			return new Logarithm().Apply(value);
		}
		static ValueTerm ApplyVectorNorm(ValueTerm value)
		{
			int dimension = value.Dimension;

			Variable x = new Variable(dimension, "x");

			return new FunctionDefinition
			(
				Exponentiation(Product(x, x), Constant(0.5)).Abstract(x),
				new NormSyntax("norm")
			)
			.Apply(value);
		}

		public static ValueTerm Sum(IEnumerable<ValueTerm> values)
		{
			return ApplyBigSum(values);
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
		public static ValueTerm Product(IEnumerable<ValueTerm> values)
		{
			return ApplyBigProduct(values);
		}
		public static ValueTerm Product(params ValueTerm[] values)
		{
			return Product((IEnumerable<ValueTerm>)values);
		}
		public static ValueTerm Invert(ValueTerm value)
		{
			return Exponentiation(value, Constant(-1));
		}
		public static ValueTerm Quotient(ValueTerm value1, ValueTerm value2)
		{
			return Product(value1, Invert(value2));
		}
		public static ValueTerm Scaling(ValueTerm factor, ValueTerm value)
		{
			return ApplyVectorScaling(factor, value);
		}
		public static ValueTerm Exponentiation(ValueTerm @base, ValueTerm exponent)
		{
			return ApplyExponentiation(@base, exponent);
		}
		public static ValueTerm Square(ValueTerm value)
		{
			return Exponentiation(value, Constant(2));
		}
		public static ValueTerm Logarithm(ValueTerm value)
		{
			return ApplyLogarithm(value);
		}
		public static ValueTerm Norm(ValueTerm value)
		{
			return ApplyVectorNorm(value);
		}
		public static ValueTerm NormSquared(ValueTerm value)
		{
			return Square(Norm(value));
		}

		public static FunctionTerm Polynomial(int dimension, int degree)
		{
			if (dimension < 0) throw new ArgumentOutOfRangeException("dimension");
			if (degree < 0) throw new ArgumentOutOfRangeException("degree");

			Variable variable = new Variable(1, "x");
			IEnumerable<Variable> coefficients =
			(
				from index in Enumerable.Range(0, degree)
				select new Variable(dimension, string.Format("c{0}", index))
			)
			.ToArray();

			return new FunctionDefinition
			(
				Sum
				(
					from index in Enumerable.Range(0, degree)
					let power = Exponentiation(variable, Term.Constant(index))
					let parameter = coefficients.ElementAt(index)
					select Scaling(power, parameter)
				)
				.Abstract(Enumerables.Concatenate(Enumerables.Create(variable), coefficients)),
				new BasicFunctionSyntax("ω")
			);
		}
	}
}