using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;
using Krach.Calculus.Terms.Combination;
using Krach.Calculus.Abstract;
using Krach.Calculus.Terms.Basic;

namespace Krach.Calculus.Terms
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
			if (terms.Count() == 1) return terms.Single();
			
			return new Vector(terms);
		}
		public static ValueTerm Vector(params ValueTerm[] terms)
		{
			return Vector((IEnumerable<ValueTerm>)terms);
		}
		public static ValueTerm Select(this ValueTerm term, int index)
		{
			if (term.Dimension == 1 && index == 0) return term;
			
			return new Selection(term, index);
		}
		
		public static ValueTerm ToTerm(this IValue value)
		{
			if (value is ValueTerm) return (ValueTerm)value;
			
			return new BasicValue(value);
		}
		public static FunctionTerm ToTerm(this IFunction function)
		{
			if (function is FunctionTerm) return (FunctionTerm)function;
			
			return new BasicFunction(function);
		}
		
		public static ValueTerm Constant(IEnumerable<double> values)
		{			
			return Vector(values.Select(value => new Constant(value).ToTerm()));
		}
		public static ValueTerm Constant(params double[] values)
		{
			return Constant((IEnumerable<double>)values);
		}
		public static ValueTerm Sum(ValueTerm value1, ValueTerm value2)
		{
			return new Sum(Items.Equal(value1.Dimension, value2.Dimension)).ToTerm().Apply(value1, value2);
		}
		public static ValueTerm Sum(IEnumerable<ValueTerm> terms)
		{
			return terms.Aggregate(Sum);
		}
		public static ValueTerm Negate(this ValueTerm value)
		{
			return Scale(Constant(-1), value);
		}
		public static ValueTerm Difference(ValueTerm value1, ValueTerm value2)
		{
			return Sum(value1, value2.Negate());
		}
		public static ValueTerm Product(ValueTerm value1, ValueTerm value2)
		{
			return new Product(Items.Equal(value1.Dimension, value2.Dimension)).ToTerm().Apply(value1, value2);
		}
		public static ValueTerm Product(IEnumerable<ValueTerm> terms)
		{
			return terms.Aggregate(Product);
		}
		public static ValueTerm Scale(ValueTerm factor, ValueTerm vector)
		{
			return new Scaling(vector.Dimension).ToTerm().Apply(factor, vector);
		}
		public static ValueTerm Invert(this ValueTerm value)
		{
			return value.Exponentiate(-1);
		}
		public static ValueTerm Quotient(ValueTerm value1, ValueTerm value2)
		{
			return Term.Product(value1, value2.Invert());
		}
		public static ValueTerm Exponentiate(this ValueTerm value, double exponent)
		{
			return new Exponentiation(exponent).ToTerm().Apply(value);
		}
		public static ValueTerm Square(this ValueTerm term)
		{
			return term.Exponentiate(2);
		}
	}
}