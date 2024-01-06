using System;
using System.Collections.Generic;
using Krach.Extensions;
using System.Linq;
using Krach.Calculus.Terms.Composite;
using Krach.Calculus.Terms;
using Krach.Calculus.Terms.Notation;
using Krach.Calculus.Terms.Notation.Custom;

namespace Krach.Calculus.Terms.Basic.Atoms
{
	public class Product : BasicFunctionTerm, IEquatable<Product>
	{
        public override Syntax Syntax { get { return new BasicBinaryOperatorSyntax("∙"); } }
		public override int DomainDimension { get { return 2; } }
		public override int CodomainDimension { get { return 1; } }

		public override bool Equals(object obj)
		{
			return obj is Product && Equals(this, (Product)obj);
		}
		public override int GetHashCode()
		{
			return 0;
		}
		public bool Equals(Product other)
		{
			return object.Equals(this, other);
		}

		public override IEnumerable<double> Evaluate(IEnumerable<double> values)
		{
			yield return values.ElementAt(0) * values.ElementAt(1);
		}
		
		public static bool operator ==(Product function1, Product function2)
		{
			return object.Equals(function1, function2);
		}
		public static bool operator !=(Product function1, Product function2)
		{
			return !object.Equals(function1, function2);
		}
		
		static bool Equals(Product function1, Product function2) 
		{
			if (object.ReferenceEquals(function1, function2)) return true;
			if (object.ReferenceEquals(function1, null) || object.ReferenceEquals(function2, null)) return false;
			
			return true;
		}
	}
}
