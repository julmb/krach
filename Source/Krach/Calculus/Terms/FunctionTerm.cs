using System;
using System.Collections.Generic;
using Krach.Calculus.Terms.Notation;

namespace Krach.Calculus.Terms
{
	public abstract class FunctionTerm : VariableTerm<FunctionTerm>, IFunction, IEquatable<FunctionTerm>
	{
		public override Syntax Syntax { get { return FunctionSyntax; } }
		public abstract FunctionSyntax FunctionSyntax { get; }
		public abstract int DomainDimension { get; }
		public abstract int CodomainDimension { get; }
		
		public override bool Equals(object obj)
		{
			throw new InvalidOperationException();
		}
		public override int GetHashCode()
		{
			throw new InvalidOperationException();
		}
		public bool Equals(FunctionTerm other)
		{
			return object.Equals(this, other);
		}

		public abstract IEnumerable<double> Evaluate(IEnumerable<double> parameters);
		public abstract IEnumerable<FunctionTerm> GetDerivatives();

		public static bool operator ==(FunctionTerm term1, FunctionTerm term2)
		{
			return object.Equals(term1, term2);
		}
		public static bool operator !=(FunctionTerm term1, FunctionTerm term2)
		{
			return !object.Equals(term1, term2);
		}

		IEnumerable<IFunction> IFunction.GetDerivatives()
		{
			return GetDerivatives();
		}
	}
}

