using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Terms.LambdaTerms;
using Krach.Terms.Functions;

namespace Krach.Terms
{
	public abstract class Term<T> where T : Term<T>
	{
		public abstract T Substitute(Variable variable, ValueTerm substitute);
		public T Substitute(IEnumerable<Variable> variables, IEnumerable<ValueTerm> substitutes)
		{
			return
				Enumerable.Zip(variables, substitutes, Tuple.Create)
				.Aggregate((T)this, (result, item) => result.Substitute(item.Item1, item.Item2));
		}
	}
	public static class Term
	{		
		public static ValueTerm Constant(double value)
		{
			return new Constant(value);
		}
		public static ValueTerm Variable(string name)
		{
			return new Variable(name);
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
			return new Application(function, parameters);
		}
		public static ValueTerm Apply(this FunctionTerm function, params ValueTerm[] parameters) 
		{
			return function.Apply((IEnumerable<ValueTerm>)parameters);
		}
		
		public static ValueTerm Sum(ValueTerm term1, ValueTerm term2)
		{
			return new Sum().Apply(term1, term2);
		}
		public static ValueTerm Sum(IEnumerable<ValueTerm> terms)
		{
			return terms.Aggregate(Term.Constant(0), Sum);
		}
		public static ValueTerm Negate(this ValueTerm term) 
		{
			return Term.Product(Term.Constant(-1), term);
		}
		public static ValueTerm Difference(ValueTerm term1, ValueTerm term2)
		{
			return Term.Sum(term1, term2.Negate());
		}
		public static ValueTerm Product(ValueTerm term1, ValueTerm term2)
		{
			return new Product().Apply(term1, term2);
		}
		public static ValueTerm Product(IEnumerable<ValueTerm> terms)
		{
			return terms.Aggregate(Term.Constant(1), Product);
		}
		public static ValueTerm Invert(this ValueTerm term)
		{
			return term.Exponentiate(-1);
		}
		public static ValueTerm Quotient(ValueTerm term1, ValueTerm term2)
		{
			return Term.Product(term1, term2.Invert());
		}
		public static ValueTerm Exponentiate(this ValueTerm term, double exponent)
		{
			return new Exponentiation(exponent).Apply(term);
		}
		public static ValueTerm Square(this ValueTerm term)
		{
			return term.Exponentiate(2);
		}
	}
}

