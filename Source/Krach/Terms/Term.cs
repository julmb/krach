using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Terms.LambdaTerms;
using Krach.Terms.Functions;

namespace Krach.Terms
{
	public abstract class Term<T> : IEquatable<Term<T>> where T : Term<T>
	{
		public override bool Equals(object obj)
		{
			throw new InvalidOperationException();
		}
		public override int GetHashCode()
		{
			throw new InvalidOperationException();
		}
		public bool Equals(Term<T> other)
		{
			return object.Equals(this, other);
		}
		
		public abstract string GetText();
		public abstract IEnumerable<Variable> GetFreeVariables();
		public abstract T RenameVariable(Variable oldVariable, Variable newVariable);
		public T RenameVariables(IEnumerable<Variable> oldVariables, IEnumerable<Variable> newVariables)
		{
			return
				Enumerable.Zip(oldVariables, newVariables, Tuple.Create)
				.Aggregate((T)this, (result, item) => result.RenameVariable(item.Item1, item.Item2));
		}
		public abstract T Substitute(Variable variable, Value substitute);
		public T Substitute(IEnumerable<Variable> variables, IEnumerable<Value> substitutes)
		{
			return
				Enumerable.Zip(variables, substitutes, Tuple.Create)
				.Aggregate((T)this, (result, item) => result.Substitute(item.Item1, item.Item2));
		}
		
		public static bool operator ==(Term<T> term1, Term<T> term2)
		{
			return object.Equals(term1, term2);
		}
		public static bool operator !=(Term<T> term1, Term<T> term2)
		{
			return !object.Equals(term1, term2);
		}
	}
	public static class Term
	{		
		public static Value Constant(double value)
		{
			return new Constant(value);
		}
		public static Value Variable(string name)
		{
			return new Variable(name);
		}
		public static Function Abstract(this Value term, IEnumerable<Variable> variables)
		{
			return new Abstraction(variables, term);
		}
		public static Function Abstract(this Value term, params Variable[] variables)
		{
			return term.Abstract((IEnumerable<Variable>)variables);
		}
		public static Value Apply(this Function function, IEnumerable<Value> parameters) 
		{
			return new Application(function, parameters);
		}
		public static Value Apply(this Function function, params Value[] parameters) 
		{
			return function.Apply((IEnumerable<Value>)parameters);
		}
		
		public static Value Sum(Value term1, Value term2)
		{
			return new Sum().Apply(term1, term2);
		}
		public static Value Sum(IEnumerable<Value> terms)
		{
			return terms.Aggregate(Term.Constant(0), Sum);
		}
		public static Value Negate(this Value term) 
		{
			return Term.Product(Term.Constant(-1), term);
		}
		public static Value Difference(Value term1, Value term2)
		{
			return Term.Sum(term1, term2.Negate());
		}
		public static Value Product(Value term1, Value term2)
		{
			return new Product().Apply(term1, term2);
		}
		public static Value Product(IEnumerable<Value> terms)
		{
			return terms.Aggregate(Term.Constant(1), Product);
		}
		public static Value Invert(this Value term)
		{
			return term.Exponentiate(-1);
		}
		public static Value Quotient(Value term1, Value term2)
		{
			return Term.Product(term1, term2.Invert());
		}
		public static Value Exponentiate(this Value term, double exponent)
		{
			return new Exponentiation(exponent).Apply(term);
		}
		public static Value Square(this Value term)
		{
			return term.Exponentiate(2);
		}
	}
}

