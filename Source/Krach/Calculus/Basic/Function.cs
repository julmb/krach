using System;
using System.Collections.Generic;
using Krach.Calculus.Abstract;
using Krach.Calculus.Terms.Combination;

namespace Krach.Calculus.Basic
{
	public abstract class Function : IFunction, IEquatable<Function>
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
		
		public abstract string GetText();
		public virtual bool HasCustomApplicationText(IValue parameter)
		{
			return false;
		}
		public virtual string GetCustomApplicationText(IValue parameter)
		{
			throw new InvalidOperationException();
		}
		public abstract IEnumerable<double> Evaluate(IEnumerable<double> parameters);
		public abstract IEnumerable<IFunction> GetDerivatives();
		
		public static bool operator ==(Function function1, Function function2)
		{
			return object.Equals(function1, function2);
		}
		public static bool operator !=(Function function1, Function function2)
		{
			return !object.Equals(function1, function2);
		}
	}
}

