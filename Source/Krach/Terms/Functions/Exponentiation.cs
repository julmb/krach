using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;
using Krach.Terms.LambdaTerms;

namespace Krach.Terms.Functions
{
	public class Exponentiation : Function
	{
		readonly double exponent;
		
		public Exponentiation(double exponent) 
		{
			this.exponent = exponent;
		}

		public override string GetText(IEnumerable<string> parameterTexts)
		{
			return string.Format("({0} ^ {1})", parameterTexts.ElementAt(0), exponent);
		}
		public override double Evaluate(IEnumerable<double> values)
		{
			return values.ElementAt(0).Exponentiate(exponent);
		}
		public override IEnumerable<FunctionTerm> GetJacobian()
		{	
			Variable x = new Variable("x");
			
			yield return Term.Product(Term.Constant(exponent), x.Exponentiate(exponent - 1)).Abstract(x);
		}
	}
}

