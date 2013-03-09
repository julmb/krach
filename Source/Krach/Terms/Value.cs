using System;
using Krach.Terms.LambdaTerms;
using System.Collections.Generic;

namespace Krach.Terms
{
	public abstract class Value : Term<Value>, IEquatable<Value>
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
		public bool Equals(Value other)
		{
			return object.Equals(this, other);
		}
		
		public abstract IEnumerable<double> Evaluate();
		public abstract IEnumerable<Value> GetPartialDerivatives(Variable variable);
	
		public static bool operator ==(Value term1, Value term2)
		{
			return object.Equals(term1, term2);
		}
		public static bool operator !=(Value term1, Value term2)
		{
			return !object.Equals(term1, term2);
		}
	}
}

