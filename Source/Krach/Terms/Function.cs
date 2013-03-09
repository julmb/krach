using System;
using System.Collections.Generic;
using Krach.Extensions;

namespace Krach.Terms
{
	public abstract class Function : Term<Function>
	{
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
		public bool Equals(Function other)
		{
			return object.Equals(this, other);
		}
		
		public abstract IEnumerable<double> Evaluate(IEnumerable<double> values);
		public abstract IEnumerable<Function> GetPartialDerivatives();
		
		public static bool operator ==(Function term1, Function term2)
		{
			return object.Equals(term1, term2);
		}
		public static bool operator !=(Function term1, Function term2)
		{
			return !object.Equals(term1, term2);
		}
	}
}
