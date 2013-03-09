using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;
using Krach.Terms.LambdaTerms;

namespace Krach.Terms.Functions
{
	public class Exponentiation : BasicFunction
	{
		readonly double exponent;
			
		public override int DomainDimension { get { return 1; } }
		public override int CodomainDimension { get { return 1; } }

		public Exponentiation(double exponent) 
		{
			this.exponent = exponent;
		}
		
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
		public override string ToString()
		{
			if (exponent.Floor() == exponent) return ((int)exponent).ToSuperscriptString();
			
			return string.Format("^{0}", exponent);
		}
		public override IEnumerable<double> Evaluate(IEnumerable<double> values)
		{
			yield return values.ElementAt(0).Exponentiate(exponent);
		}
		public override IEnumerable<Function> GetPartialDerivatives()
		{	
			Variable x = new Variable(1, "x");
			
			yield return Term.Product(Term.Constant(exponent), x.Exponentiate(exponent - 1)).Abstract(x);
		}
		
		public static bool operator ==(Exponentiation exponentiation1, Exponentiation exponentiation2)
		{
			return object.Equals(exponentiation1, exponentiation2);
		}
		public static bool operator !=(Exponentiation exponentiation1, Exponentiation exponentiation2)
		{
			return !object.Equals(exponentiation1, exponentiation2);
		}
		
		static bool Equals(Exponentiation exponentiation1, Exponentiation exponentiation2)
		{
			if (object.ReferenceEquals(exponentiation1, exponentiation2)) return true;
			if (object.ReferenceEquals(exponentiation1, null) || object.ReferenceEquals(exponentiation2, null)) return false;
			
			return exponentiation1.exponent == exponentiation2.exponent;
		}
	}
}

