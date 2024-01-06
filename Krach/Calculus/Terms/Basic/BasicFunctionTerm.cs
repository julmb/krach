using System;
using System.Collections.Generic;
using Krach.Calculus.Terms.Composite;
using System.Linq;
using Krach.Extensions;

namespace Krach.Calculus.Terms.Basic
{
	public abstract class BasicFunctionTerm : FunctionTerm, IEquatable<BasicFunctionTerm>
	{
		public override bool Equals(object obj)
		{
			throw new InvalidOperationException();
		}
		public override int GetHashCode()
		{
			throw new InvalidOperationException();
		}
		public bool Equals(BasicFunctionTerm other)
		{
			return object.Equals(this, other);
		}

		public override IEnumerable<Variable> GetFreeVariables()
		{
			yield break;
		}
		public override FunctionTerm Substitute(Variable variable, ValueTerm substitute)
		{
			return this;
		}

		public static bool operator ==(BasicFunctionTerm term1, BasicFunctionTerm term2)
		{
			return object.Equals(term1, term2);
		}
		public static bool operator !=(BasicFunctionTerm term1, BasicFunctionTerm term2)
		{
			return !object.Equals(term1, term2);
		}
	}
}

