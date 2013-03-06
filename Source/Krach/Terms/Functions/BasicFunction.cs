using System;
using Krach.Terms.LambdaTerms;
using System.Collections.Generic;
using Krach.Extensions;

namespace Krach.Terms.Functions
{
	public abstract class BasicFunction : Function, IEquatable<BasicFunction>
	{
		public override bool Equals(object obj)
		{
			throw new InvalidOperationException();
		}
		public override int GetHashCode()
		{
			throw new InvalidOperationException();
		}
		public bool Equals(BasicFunction other)
		{
			return object.Equals(this, other);
		}
		public override IEnumerable<Variable> GetFreeVariables()
		{
			yield break;
		}
		public override Function RenameVariable(Variable oldVariable, Variable newVariable)
		{
			return this;
		}
		public override Function Substitute(Variable variable, Value substitute)
		{
			return this;
		}
		
		public static bool operator ==(BasicFunction function1, BasicFunction function2)
		{
			return object.Equals(function1, function2);
		}
		public static bool operator !=(BasicFunction function1, BasicFunction function2)
		{
			return !object.Equals(function1, function2);
		}
	}
}

