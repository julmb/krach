using System;
using System.Collections.Generic;
using Krach.Calculus.Terms.Combination;

namespace Krach.Calculus.Terms
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
		
		public override int GetSize()
		{
			return 1;
		}
		
		public override IEnumerable<Variable> GetFreeVariables()
		{
			yield break;
		}
		public override FunctionTerm Substitute(Variable variable, ValueTerm substitute)
		{
			return this;
		}
		
		public override bool HasCustomApplicationText(ValueTerm parameter)
		{
			return false;
		}
		public override string GetCustomApplicationText(ValueTerm parameter)
		{
			throw new InvalidOperationException();
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

