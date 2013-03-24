using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;
using Krach.Calculus.Terms.Combination;

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
		public abstract T RenameVariable(Variable oldVariable, Variable newVariable);
		public T RenameVariables(IEnumerable<Variable> oldVariables, IEnumerable<Variable> newVariables)
		{
			return
				Enumerable.Zip(oldVariables, newVariables, Tuple.Create)
				.Aggregate((T)this, (result, item) => result.RenameVariable(item.Item1, item.Item2));
		}
	    public abstract T Substitute(Variable variable, ValueTerm substitute);
		public T Substitute(IEnumerable<Variable> variables, IEnumerable<ValueTerm> substitutes)
		{
			return
				Enumerable.Zip(variables, substitutes, Tuple.Create)
				.Aggregate((T)this, (result, item) => result.Substitute(item.Item1, item.Item2));
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