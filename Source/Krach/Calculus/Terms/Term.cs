using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;
using Krach.Calculus.Terms.Combination;
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
			
			if (term is Vector)
			{
				Vector vector = (Vector)term;
				
				return
				(
					from subTerm in vector.Terms
					from subIndex in Enumerable.Range(0, subTerm.Dimension)
					select subTerm.Select(subIndex)
				)
				.ElementAt(index);
			}
			
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
		public static ValueTerm Sum(ValueTerm value1, ValueTerm value2)
		{
			return new Sum(Items.Equal(value1.Dimension, value2.Dimension)).Apply(value1, value2);
		}
		public static ValueTerm Sum(IEnumerable<ValueTerm> terms)
		{
			return terms.Aggregate(Sum);
		}
		public static ValueTerm Sum(params ValueTerm[] terms)
		{
			return Sum((IEnumerable<ValueTerm>)terms);
		}
		public static ValueTerm Negate(ValueTerm value)
		{
			return Scale(Constant(-1), value);
		}
		public static ValueTerm Difference(ValueTerm value1, ValueTerm value2)
		{
			return Sum(value1, Term.Negate(value2));
		}
		public static ValueTerm Product(ValueTerm value1, ValueTerm value2)
		{
			return new Product(Items.Equal(value1.Dimension, value2.Dimension)).Apply(value1, value2);
		}
		public static ValueTerm Product(IEnumerable<ValueTerm> terms)
		{
			return terms.Aggregate(Product);
		}
		public static ValueTerm Product(params ValueTerm[] terms)
		{
			return Product((IEnumerable<ValueTerm>)terms);
		}
		public static ValueTerm Scale(ValueTerm factor, ValueTerm vector)
		{
			if (vector.Dimension == 1) return Product(factor, vector);
			
			return new Scaling(vector.Dimension).Apply(factor, vector);
		}
		public static ValueTerm Invert(ValueTerm value)
		{
			return Exponentiate(value, Term.Constant(-1));
		}
		public static ValueTerm Quotient(ValueTerm value1, ValueTerm value2)
		{
			return Term.Product(value1, Term.Invert(value2));
		}
		public static ValueTerm Exponentiate(ValueTerm @base, ValueTerm exponent)
		{
			return new Exponentiation().Apply(@base, exponent);
		}
		public static ValueTerm Logarithm(ValueTerm value)
		{
			return new Logarithm().Apply(value);
		}
	}
}