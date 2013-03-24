using System;
using System.Collections.Generic;
using Krach.Calculus.Abstract;

namespace Krach.Calculus.Basic
{
	public abstract class Value : IValue, IEquatable<Value>
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
		
		public abstract string GetText();
		public abstract IEnumerable<double> Evaluate();
		
		public static bool operator ==(Value value1, Value value2)
		{
			return object.Equals(value1, value2);
		}
		public static bool operator !=(Value value1, Value value2)
		{
			return !object.Equals(value1, value2);
		}
	}
}

