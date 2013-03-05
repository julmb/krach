using System;
using Krach.Terms.LambdaTerms;
using System.Collections.Generic;
using Krach.Extensions;

namespace Krach.Terms.Functions
{
	public abstract class Function : FunctionTerm, IEquatable<Function>
	{
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
		public override IEnumerable<Variable> GetFreeVariables()
		{
			yield break;
		}
		public override FunctionTerm RenameVariable(Variable oldVariable, Variable newVariable)
		{
			return this;
		}
		public override FunctionTerm Substitute(Variable variable, ValueTerm substitute)
		{
			return this;
		}
		
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

