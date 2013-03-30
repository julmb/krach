using System;
using System.Collections.Generic;
using Krach.Calculus.Terms.Composite;
using System.Linq;

namespace Krach.Calculus.Terms.Basic
{
	public abstract class BasicValueTerm : ValueTerm, IEquatable<BasicValueTerm>
	{
		public override bool Equals(object obj)
		{
			throw new InvalidOperationException();
		}
		public override int GetHashCode()
		{
			throw new InvalidOperationException();
		}
		public bool Equals(BasicValueTerm other)
		{
			return object.Equals(this, other);
		}
		
		public override int GetSize()
		{
			return 1;
		}
		
		public override IEnumerable<Variable> GetFreeVariables()
		{
			yield break;
		}
		public override ValueTerm Substitute(Variable variable, ValueTerm substitute)
		{
			return this;
		}

		public override IEnumerable<ValueTerm> GetDerivatives(Variable variable)
		{
			return
			(
				from variableIndex in Enumerable.Range(0, variable.Dimension)
				select Term.Vector(Enumerable.Repeat(Term.Constant(0), Dimension))
			)
			.ToArray();
		}

		public static bool operator ==(BasicValueTerm term1, BasicValueTerm term2)
		{
			return object.Equals(term1, term2);
		}
		public static bool operator !=(BasicValueTerm term1, BasicValueTerm term2)
		{
			return !object.Equals(term1, term2);
		}
	}
}

