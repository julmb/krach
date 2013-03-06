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
			
		public override int ParameterCount { get { return 1; } }
		
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
			return exponent.GetHashCode();
		}
		public bool Equals(Exponentiation other)
		{
			return object.Equals(this, other);
		}
		public override string GetText()
		{
			return string.Format("(^{0})", exponent);
		}
		public override string GetText(IEnumerable<string> parameterTexts)
		{
			return string.Format("({0} ^ {1})", parameterTexts.ElementAt(0), exponent);
		}
		public override double Evaluate(IEnumerable<double> values)
		{
			return values.ElementAt(0).Exponentiate(exponent);
		}
		public override IEnumerable<Function> GetJacobian()
		{	
			Variable x = new Variable("x");
			
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

