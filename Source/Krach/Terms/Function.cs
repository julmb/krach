using System;
using System.Collections.Generic;
using Krach.Extensions;

namespace Krach.Terms
{
	public abstract class Function : Term<Function>
	{
		public abstract int ParameterCount { get; }
		
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
		
		public virtual string GetText(IEnumerable<string> parameterTexts)
		{
			return string.Format("({0} {1})", GetText(), parameterTexts.Separate(" ").AggregateString());
		}
		public abstract double Evaluate(IEnumerable<double> values);
		public abstract IEnumerable<Function> GetJacobian();
	
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
