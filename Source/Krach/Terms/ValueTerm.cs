using System;
using Krach.Terms.LambdaTerms;

namespace Krach.Terms
{
	public abstract class ValueTerm : Term<ValueTerm>, IEquatable<ValueTerm>
	{
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
		
		public abstract double Evaluate();
		public abstract ValueTerm GetDerivative(Variable variable);
	
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

