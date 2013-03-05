using System;
using System.Collections.Generic;
using Krach.Extensions;
using System.Linq;
using Krach.Terms.LambdaTerms;

namespace Krach.Terms.Functions
{
	public class Product : Function
	{
		public override string GetText(IEnumerable<string> parameterTexts)
		{
			return string.Format("({0} * {1})", parameterTexts.ElementAt(0), parameterTexts.ElementAt(1));
		}
		public override double Evaluate(IEnumerable<double> values)
		{
			return values.ElementAt(0) * values.ElementAt(1);
		}	
		public override IEnumerable<FunctionTerm> GetJacobian()
		{
			Variable x = new Variable("x");
			Variable y = new Variable("y");
			
			yield return y.Abstract(x, y);
			yield return x.Abstract(x, y);
		}
	}
}
