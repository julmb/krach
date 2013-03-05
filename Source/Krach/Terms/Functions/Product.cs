using System;
using System.Collections.Generic;
using Krach.Extensions;
using System.Linq;
using Krach.Terms.LambdaTerms;

namespace Krach.Terms.Functions
{
	public class Product : Function
	{
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
		public override string GetText()
		{
			return "(*)";
		}
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
		
		public static bool operator ==(Product product1, Product product2)
		{
			return object.Equals(product1, product2);
		}
		public static bool operator !=(Product product1, Product product2)
		{
			return !object.Equals(product1, product2);
		}
		
		static bool Equals(Product product1, Product product2)
		{
			if (object.ReferenceEquals(product1, product2)) return true;
			if (object.ReferenceEquals(product1, null) || object.ReferenceEquals(product2, null)) return false;
			
			return true;
		}
	}
}
