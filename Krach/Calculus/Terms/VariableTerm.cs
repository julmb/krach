using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;
using Krach.Calculus.Terms.Composite;

namespace Krach.Calculus.Terms
{
	public abstract class VariableTerm<T> : BaseTerm, IEquatable<VariableTerm<T>> where T : VariableTerm<T>
	{
		public override bool Equals(object obj)
		{
			throw new InvalidOperationException();
		}
		public override int GetHashCode()
		{
			throw new InvalidOperationException();
		}
		public bool Equals(VariableTerm<T> other)
		{
			return object.Equals(this, other);
		}

		public abstract IEnumerable<Variable> GetFreeVariables();
	    public abstract T Substitute(Variable variable, ValueTerm substitute);
		public T Substitute(IEnumerable<Variable> variables, IEnumerable<ValueTerm> substitutes)
		{
			if (!variables.IsDistinct()) throw new ArgumentException("The given variables are not distinct.");
			
			IEnumerable<Variable> freeVariables =
			(
				from substitute in substitutes
				from variable in substitute.GetFreeVariables()
				select variable
			)
			.ToArray();
			
			IEnumerable<Variable> newVariables = FindUnusedVariables(variables, freeVariables).ToArray();
			
			T result = (T)this;
			
			result = Enumerable.Zip(variables, newVariables, Tuple.Create)
				.Aggregate(result, (term, item) => term.Substitute(item.Item1, item.Item2));
			
			result = Enumerable.Zip(newVariables, substitutes, Tuple.Create)
				.Aggregate(result, (term, item) => term.Substitute(item.Item1, item.Item2));
			
			return result;
		}
		
		public static Variable FindUnusedVariable(Variable variable, IEnumerable<Variable> usedVariables)
		{			
			while (usedVariables.Contains(variable))
				variable = new Variable(variable.Dimension, variable.Name + "'");
			
			return variable;
		}
		public static IEnumerable<Variable> FindUnusedVariables(IEnumerable<Variable> variables, IEnumerable<Variable> usedVariables)
		{
			foreach (Variable variable in variables)
			{
				Variable newVariable = FindUnusedVariable(variable, usedVariables);
				
				yield return newVariable;
				
				usedVariables = usedVariables.Append(newVariable);
			}
		}
		
		public static bool operator ==(VariableTerm<T> term1, VariableTerm<T> term2)
		{
			return object.Equals(term1, term2);
		}
		public static bool operator !=(VariableTerm<T> term1, VariableTerm<T> term2)
		{
			return !object.Equals(term1, term2);
		}
	}
}