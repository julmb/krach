using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Terms.LambdaTerms;
using Krach.Terms.Functions;
using Krach.Extensions;

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
		public static Value Variable(int dimension, string name) 
		{
			return new Variable(dimension, name);
		}
		public static Value Variable(string name)
		{
			return Variable(1, name);
		}
		public static Value Vector(IEnumerable<Value> values) 
		{
			return new Vector(values);	
		}
		public static Value Vector(params Value[] values) 
		{
			return Vector((IEnumerable<Value>)values);	
		}
		public static Value Select(this Value value, int index) 
		{
			return new Selection(value, index);
		}
		public static Function Abstract(this Value term, Variable variable)
		{
			return new Abstraction(variable, term);
		}
		public static Function Abstract(this Value term, IEnumerable<Variable> variables) 
		{
			Variable combinedVariable = new Variable(variables.Sum(variable => variable.Dimension), "x").FindUnusedVariable(term.GetFreeVariables());
			
			IEnumerable<int> baseIndices = variables.Select(variable => variable.Dimension).GetPartialSums(); 
				
			IEnumerable<Value> substitutes = 
			(
				from item in Enumerable.Zip(variables, baseIndices, Tuple.Create)
				select Vector
				(
					from index in Enumerable.Range(item.Item2, item.Item1.Dimension)
					select combinedVariable.Select(index)
				)
			)
			.ToArray();
			
			return term.Substitute(variables, substitutes).Abstract(combinedVariable);
		}
		public static Function Abstract(this Value term, params Variable[] variables) 
		{
			return term.Abstract((IEnumerable<Variable>)variables);
		}
		public static Value Apply(this Function function, Value parameter) 
		{
			return new Application(function, parameter);
		}
		public static Value Apply(this Function function, IEnumerable<Value> parameters) 
		{
			return function.Apply(Vector(parameters));
		}
		public static Value Apply(this Function function, params Value[] parameters) 
		{
			return function.Apply((IEnumerable<Value>)parameters);
		}
		
		public static Value Constant(IEnumerable<double> values) 
		{
			return Vector(values.Select(value => new Constant(value).Apply()));
		}
		public static Value Constant(params double[] values)
		{
			return Constant((IEnumerable<double>)values);
		}
		public static Function Sum(int dimension)
		{
			if (dimension < 0) throw new ArgumentOutOfRangeException("dimension");
			
			Variable vector1 = new Variable(dimension, "x");
			Variable vector2 = new Variable(dimension, "y");
			
			return Vector
			(
				from index in Enumerable.Range(0, dimension)
				select new Sum().Apply(vector1.Select(index), vector2.Select(index))
			)
			.Abstract(vector1, vector2);
		}
		public static Value Sum(Value value1, Value value2) 
		{
			return Sum(Items.Equal(value1.Dimension, value2.Dimension)).Apply(value1, value2);
		}
		public static Value Sum(IEnumerable<Value> terms)
		{
			if (!terms.Any()) throw new ArgumentException("Parameter terms did not contain any items.");
			
			return terms.Skip(1).Aggregate(terms.First(), Sum);
		}
		public static Value Negate(this Value value) 
		{
			return Term.Scale(Term.Constant(-1), value);
		}
		public static Value Difference(Value value1, Value value2)
		{
			return Term.Sum(value1, value2.Negate());
		}
		public static Function DotProduct(int dimension)
		{
			if (dimension < 0) throw new ArgumentOutOfRangeException("dimension");
			
			Variable vector1 = new Variable(dimension, "x");
			Variable vector2 = new Variable(dimension, "y");
			
			return Sum
			(
				from index in Enumerable.Range(0, dimension)
				select new Product().Apply(vector1.Select(index), vector2.Select(index))
			)
			.Abstract(vector1, vector2);
		}
		public static Value DotProduct(Value value1, Value value2) 
		{
			return DotProduct(Items.Equal(value1.Dimension, value2.Dimension)).Apply(value1, value2);
		}
		public static Function Scale(int dimension)
		{
			if (dimension < 0) throw new ArgumentOutOfRangeException("dimension");
			
			Variable factor = new Variable(1, "x");
			Variable vector = new Variable(dimension, "y");
			
			return Vector
			(
				from index in Enumerable.Range(0, dimension)
				select new Product().Apply(factor, vector.Select(index))
			)
			.Abstract(factor, vector);
		}
		public static Value Scale(Value factor, Value vector) 
		{
			if (factor.Dimension != 1) throw new ArgumentException("Parameter factor is not a scalar.");
			
			return Scale(vector.Dimension).Apply(factor, vector);
		}
		public static Value Product(Value value1, Value value2)
		{
			if (value1.Dimension != 1) throw new ArgumentException("Parameter value1 is not a scalar.");
			if (value2.Dimension != 1) throw new ArgumentException("Parameter value2 is not a scalar.");

			return new Product().Apply(value1, value2);
		}
		public static Value Product(IEnumerable<Value> terms)
		{
			return terms.Aggregate(Term.Constant(1), Product);
		}
		public static Value Invert(this Value value)
		{
			if (value.Dimension != 1) throw new ArgumentException("Parameter value is not a scalar.");
		
			return value.Exponentiate(-1);
		}
		public static Value Quotient(Value value1, Value value2)
		{
			if (value1.Dimension != 1) throw new ArgumentException("Parameter value1 is not a scalar.");
			if (value2.Dimension != 1) throw new ArgumentException("Parameter value2 is not a scalar.");
			
			return Term.Product(value1, value2.Invert());
		}
		public static Value Exponentiate(this Value value, double exponent)
		{
			if (value.Dimension != 1) throw new ArgumentException("Parameter value is not a scalar.");
		
			return new Exponentiation(exponent).Apply(value);
		}
		public static Value Square(this Value term)
		{
			return term.Exponentiate(2);
		}
	}
}

