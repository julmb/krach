using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;
using Krach.Calculus.Terms.Composite;
using Krach.Calculus.Terms;
using Krach.Calculus.Terms.Notation;
using Krach.Calculus.Terms.Notation.Basic;

namespace Krach.Calculus.Terms.Basic.Atoms
{
	public class Logarithm : BasicFunctionTerm, IEquatable<Logarithm>
	{
		public override FunctionSyntax FunctionSyntax { get { return new BasicFunctionSyntax("ln"); } }
		public override int DomainDimension { get { return 1; } }
		public override int CodomainDimension { get { return 1; } }

		public override bool Equals(object obj)
		{
			return obj is Logarithm && Equals(this, (Logarithm)obj);
		}
		public override int GetHashCode()
		{
			return 0;
		}
		public bool Equals(Logarithm other)
		{
			return object.Equals(this, other);
		}

		public override IEnumerable<double> Evaluate(IEnumerable<double> values)
		{
			yield return Scalars.Logarithm(values.ElementAt(0));
		}
		public override IEnumerable<FunctionTerm> GetDerivatives()
		{	
			Variable x = new Variable(1, "x");

			yield return Term.Invert(x).Abstract(x);
		}
		
		public static bool operator ==(Logarithm function1, Logarithm function2)
		{
			return object.Equals(function1, function2);
		}
		public static bool operator !=(Logarithm function1, Logarithm function2)
		{
			return !object.Equals(function1, function2);
		}
		
		static bool Equals(Logarithm function1, Logarithm function2) 
		{
			if (object.ReferenceEquals(function1, function2)) return true;
			if (object.ReferenceEquals(function1, null) || object.ReferenceEquals(function2, null)) return false;
			
			return true;
		}
	}
}

