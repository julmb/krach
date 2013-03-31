using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;
using Krach.Calculus.Terms.Composite;
using Krach.Calculus.Terms;
using Krach.Calculus.Terms.Notation;
using Krach.Calculus.Terms.Notation.Custom;

namespace Krach.Calculus.Terms.Basic.Atoms
{
	public class Exponentiation : BasicFunctionTerm, IEquatable<Exponentiation>
	{
        public override Syntax Syntax { get { return new ExponentiationSyntax(); } }
		public override int DomainDimension { get { return 2; } }
		public override int CodomainDimension { get { return 1; } }

		public override bool Equals(object obj)
		{
			return obj is Exponentiation && Equals(this, (Exponentiation)obj);
		}
		public override int GetHashCode()
		{
			return 0;
		}
		public bool Equals(Exponentiation other)
		{
			return object.Equals(this, other);
		}

		public override IEnumerable<double> Evaluate(IEnumerable<double> values)
		{
			yield return values.ElementAt(0).Exponentiate(values.ElementAt(1));
		}
		public override IEnumerable<FunctionTerm> GetDerivatives()
		{	
			Variable x = new Variable(1, "x");
			Variable y = new Variable(1, "y");
			
			yield return Term.Product(y, Term.Exponentiation(x, Term.Difference(y, Term.Constant(1)))).Abstract(x, y);
			yield return Term.Product(Term.Logarithm(x), Term.Exponentiation(x, y)).Abstract(x, y);
		}
		
		public static bool operator ==(Exponentiation function1, Exponentiation function2)
		{
			return object.Equals(function1, function2);
		}
		public static bool operator !=(Exponentiation function1, Exponentiation function2)
		{
			return !object.Equals(function1, function2);
		}
		
		static bool Equals(Exponentiation function1, Exponentiation function2) 
		{
			if (object.ReferenceEquals(function1, function2)) return true;
			if (object.ReferenceEquals(function1, null) || object.ReferenceEquals(function2, null)) return false;
			
			return true;
		}
	}
}

