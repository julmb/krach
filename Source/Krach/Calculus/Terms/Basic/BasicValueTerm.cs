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

		public override IEnumerable<Variable> GetFreeVariables()
		{
			yield break;
		}
		public override ValueTerm Substitute(Variable variable, ValueTerm substitute)
		{
			return this;
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

