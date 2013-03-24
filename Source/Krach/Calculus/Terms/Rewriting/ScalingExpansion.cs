using System;
using System.Linq;
using Krach.Calculus.Terms.Combination;
using Krach.Calculus.Terms;
using Krach.Calculus.Terms.Basic;
using Krach.Calculus;
using System.Collections.Generic;

namespace Krach.Terms.Rewriting
{
	public class ScalingExpansion : Rule
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
			
			if (!(basicFunction.Function is Scaling)) throw new InvalidOperationException();
			
			Scaling scaling = (Scaling)basicFunction.Function;
			
			if (scaling.Dimension == 1) throw new InvalidOperationException();
			
			ValueTerm factor = application.Parameter.Select(0);
			
			return (T)(BaseTerm)Term.Vector
			(
				from index in Enumerable.Range(0, scaling.Dimension)
				let value = application.Parameter.Select(1 + index)
				select Term.Product(factor, value)
			);
		}
	}
}

