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
	public class Sum : BasicFunctionTerm, IEquatable<Sum>
	{
        public override Syntax Syntax { get { return new BasicBinaryOperatorSyntax("+"); } }
		public override int DomainDimension { get { return 2; } }
		public override int CodomainDimension { get { return 1; } }
		
		public override bool Equals(object obj)
		{
			return obj is Sum && Equals(this, (Sum)obj);
		}
		public override int GetHashCode()
		{
			return 0;
		}
		public bool Equals(Sum other)
		{
			return object.Equals(this, other);
		}

		public override IEnumerable<double> Evaluate(IEnumerable<double> values)
		{
			yield return values.ElementAt(0) + values.ElementAt(1);
		}
		public override IEnumerable<FunctionTerm> GetDerivatives()
		{
			Variable x = new Variable(1, "x");
			Variable y = new Variable(1, "y");

			yield return Term.Constant(1).Abstract(x, y);
			yield return Term.Constant(1).Abstract(x, y);
		}
		
		public static bool operator ==(Sum function1, Sum function2)
		{
			return object.Equals(function1, function2);
		}
		public static bool operator !=(Sum function1, Sum function2)
		{
			return !object.Equals(function1, function2);
		}
		
		static bool Equals(Sum function1, Sum function2) 
		{
			if (object.ReferenceEquals(function1, function2)) return true;
			if (object.ReferenceEquals(function1, null) || object.ReferenceEquals(function2, null)) return false;
			
			return true;
		}
	}
}

