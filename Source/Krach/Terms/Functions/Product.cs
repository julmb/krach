using System;
using System.Collections.Generic;
using Krach.Extensions;
using System.Linq;
using Krach.Terms.LambdaTerms;

namespace Krach.Terms.Functions
{
	public class Product : BasicFunction
	{	
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
		
		public override string ToString()
		{
			return "*";
		}
		public override IEnumerable<double> Evaluate(IEnumerable<double> values)
		{
			yield return values.ElementAt(0) * values.ElementAt(1);
		}
		public override IEnumerable<Function> GetPartialDerivatives()
		{
			Variable x = new Variable(2, "x");
			
			yield return x.Select(1).Abstract(x);
			yield return x.Select(0).Abstract(x);
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
