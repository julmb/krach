using System;
using System.Linq;
using Krach.Calculus.Terms.Combination;
using Krach.Calculus.Terms;
using Krach.Calculus.Terms.Basic;
using Krach.Calculus;
using System.Collections.Generic;

namespace Krach.Calculus.Terms.Rewriting.Rules
{
	public class ProductExpansion : Rule
	{
		public override bool Matches<T>(T term)
		{
			try { Rewrite(term); }
			catch (InvalidOperationException) { return false; }
			
			return true;
		}
		public override T Rewrite<T>(T term)
		{
			if (!(term is Application)) throw new InvalidOperationException();
			
			Application application = (Application)(BaseTerm)term;
			
			if (!(application.Function is Product)) throw new InvalidOperationException();
			
			Product product = (Product)application.Function;
			
			if (product.Dimension == 1) throw new InvalidOperationException();
			
			return (T)(BaseTerm)Term.Sum
			(
				from index in Enumerable.Range(0, product.Dimension)
				let value1 = application.Parameter.Select(0 * product.Dimension + index)
				let value2 = application.Parameter.Select(1 * product.Dimension + index)
				select Term.Product(value1, value2)
			);
		}
	}
}

