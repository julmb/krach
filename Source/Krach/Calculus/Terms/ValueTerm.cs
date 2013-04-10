using System;
using System.Collections.Generic;
using Krach.Calculus.Terms.Composite;
using Krach.Calculus.Terms.Notation;
using Krach.Calculus.Abstract;

namespace Krach.Calculus.Terms
{
	public abstract class ValueTerm : VariableTerm<ValueTerm>, IValue, IEquatable<ValueTerm>
	{
		public abstract int Dimension { get; }
		
		public override bool Equals(object obj)
		{
			throw new InvalidOperationException();
		}
		public override int GetHashCode()
		{
			throw new InvalidOperationException();
		}
		public bool Equals(ValueTerm other)
		{
			return object.Equals(this, other);
		}
		
		public abstract IEnumerable<double> Evaluate();
		
		public static bool operator ==(ValueTerm term1, ValueTerm term2)
		{
			return object.Equals(term1, term2);
		}
		public static bool operator !=(ValueTerm term1, ValueTerm term2)
		{
			return !object.Equals(term1, term2);
		}
	}
}

