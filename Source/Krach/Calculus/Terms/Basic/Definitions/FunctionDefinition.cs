using System;
using System.Collections.Generic;
using Krach.Calculus.Terms.Composite;
using System.Linq;
using Krach.Extensions;
using Krach.Calculus.Terms.Notation;

namespace Krach.Calculus.Terms.Basic.Definitions
{
	public class FunctionDefinition : BasicFunctionTerm, IEquatable<FunctionDefinition>
	{
		readonly FunctionTerm function;
		readonly FunctionSyntax functionSyntax;

		public FunctionTerm Function { get { return function; } }
		public override FunctionSyntax FunctionSyntax { get { return functionSyntax; } }
		public override int DomainDimension { get { return function.DomainDimension; } }
		public override int CodomainDimension { get { return function.CodomainDimension; } }

		public FunctionDefinition(FunctionTerm function, FunctionSyntax functionSyntax)
		{
			if (function == null) throw new ArgumentNullException("function");
			if (functionSyntax == null) throw new ArgumentNullException("functionSyntax");

			this.function = function;
			this.functionSyntax = functionSyntax;
		}

		public override bool Equals(object obj)
		{
			return obj is FunctionDefinition && Equals(this, (FunctionDefinition)obj);
		}
		public override int GetHashCode()
		{
			return function.GetHashCode();
		}
		public bool Equals(FunctionDefinition other)
		{
			return object.Equals(this, other);
		}

		public override IEnumerable<double> Evaluate(IEnumerable<double> values)
		{
			throw new InvalidOperationException("Cannot evaluate definition.");
		}
		public override IEnumerable<FunctionTerm> GetDerivatives()
		{
			throw new InvalidOperationException("Cannot derive definition.");
		}

		public static bool operator ==(FunctionDefinition function1, FunctionDefinition function2)
		{
			return object.Equals(function1, function2);
		}
		public static bool operator !=(FunctionDefinition function1, FunctionDefinition function2)
		{
			return !object.Equals(function1, function2);
		}

		static bool Equals(FunctionDefinition function1, FunctionDefinition function2) 
		{
			if (object.ReferenceEquals(function1, function2)) return true;
			if (object.ReferenceEquals(function1, null) || object.ReferenceEquals(function2, null)) return false;
			
			return function1.function == function2.function;
		}
	}
}

