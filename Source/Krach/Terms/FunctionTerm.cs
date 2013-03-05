using System;
using System.Collections.Generic;
using Krach.Extensions;

namespace Krach.Terms
{
	public abstract class FunctionTerm : Term<FunctionTerm>
	{	
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
		
		public virtual string GetText(IEnumerable<string> parameterTexts)
		{
			return string.Format("({0} {1})", GetText(), parameterTexts.Separate(" ").AggregateString());
		}
		public abstract double Evaluate(IEnumerable<double> values);
		public abstract IEnumerable<FunctionTerm> GetJacobian();
	
		public static bool operator ==(FunctionTerm term1, FunctionTerm term2)
		{
			return object.Equals(term1, term2);
		}
		public static bool operator !=(FunctionTerm term1, FunctionTerm term2)
		{
			return !object.Equals(term1, term2);
		}
	}
}
