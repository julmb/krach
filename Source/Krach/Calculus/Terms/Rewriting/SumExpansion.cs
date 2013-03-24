using System;
using System.Linq;
using Krach.Calculus.Terms.Combination;
using Krach.Calculus.Terms;
using Krach.Calculus.Terms.Basic;
using Krach.Calculus;
using System.Collections.Generic;

namespace Krach.Terms.Rewriting
{
	public class SumExpansion : Rule
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
			
			if (!(application.Function is BasicFunction)) throw new InvalidOperationException();
			
			BasicFunction basicFunction = (BasicFunction)application.Function;
			
			if (!(basicFunction.Function is Sum)) throw new InvalidOperationException();
			
			Sum sum = (Sum)basicFunction.Function;
			
			if (sum.Dimension == 1) throw new InvalidOperationException();
			
			return (T)(BaseTerm)Term.Vector
			(
				from index in Enumerable.Range(0, sum.Dimension)
				let value1 = application.Parameter.Select(0 * sum.Dimension + index)
				let value2 = application.Parameter.Select(1 * sum.Dimension + index)
				select Term.Sum(value1, value2)
			);
		}
	}
}

