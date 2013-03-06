using System;
using Krach.Terms.LambdaTerms;

namespace Krach.Terms
{
	public abstract class Value : Term<Value>, IEquatable<Value>
	{
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
		
		public abstract double Evaluate();
		public abstract Value GetDerivative(Variable variable);
	
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

