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
        readonly string name;
		readonly FunctionTerm function;
        readonly Syntax syntax;

        public string Name { get { return name; } }
		public FunctionTerm Function { get { return function; } }
        public override Syntax Syntax { get { return syntax; } }
		public override int DomainDimension { get { return function.DomainDimension; } }
		public override int CodomainDimension { get { return function.CodomainDimension; } }

        public FunctionDefinition(string name, FunctionTerm function, Syntax syntax)
        {
            if (name == null) throw new ArgumentNullException("name");
			if (function == null) throw new ArgumentNullException("function");
            if (syntax == null) throw new ArgumentNullException("syntax");

            this.name = name;
			this.function = function;
            this.syntax = syntax;
		}

		public override bool Equals(object obj)
		{
			return obj is FunctionDefinition && Equals(this, (FunctionDefinition)obj);
		}
		public override int GetHashCode()
		{
            return name.GetHashCode();
		}
		public bool Equals(FunctionDefinition other)
		{
			return object.Equals(this, other);
		}

		public override IEnumerable<double> Evaluate(IEnumerable<double> values)
		{
            return function.Evaluate(values);
		}
		public override IEnumerable<FunctionTerm> GetDerivatives()
		{
            return function.GetDerivatives();
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

            return function1.name == function2.name;
		}
	}
}

