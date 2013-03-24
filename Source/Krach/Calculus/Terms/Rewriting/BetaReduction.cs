using System;
using Krach.Calculus.Terms.Combination;
using Krach.Calculus.Terms;
using System.Linq;
using System.Collections.Generic;
using Krach.Extensions;

namespace Krach.Terms.Rewriting
{
	public class BetaReduction : Rule
	{
		public override bool Matches<T>(T term)
		{
			try { Rewrite(term); }
			catch (InvalidOperationException) { return false; }
			
			return true;
		}
		public override T Rewrite<T>(T term)
		{
			if (term is Application) 
			{
				Application application = (Application)(BaseTerm)term;
				
				if (application.Function is Abstraction)
				{
					Abstraction abstraction = (Abstraction)application.Function;

					IEnumerable<ValueTerm> substitutes = Enumerable.Zip
					(
						abstraction.Variables.Select(variable => variable.Dimension).GetPartialSums(),
						abstraction.Variables.Select(variable => variable.Dimension),
						(start, length) => Term.Vector
						(
							from index in Enumerable.Range(start, length)
							select application.Parameter.Select(index)
						)
					)
					.ToArray();
					
					return (T)(BaseTerm)abstraction.Term.Substitute(abstraction.Variables, substitutes);
				}
			}
			
			throw new InvalidOperationException();
		}
	}
}

