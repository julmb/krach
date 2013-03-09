using System;
using Krach.Terms.LambdaTerms;

namespace Krach.Terms.Rewriting
{
	public class BetaReduction : Rule
	{
		public override bool Matches<T>(T term)
		{
			if (term is Application) 
			{
				Application application = (Application)(object)term;
				
				if (application.Function is Abstraction) return true;
			}
			
			return false;
		}
		public override T Rewrite<T>(T term)
		{
			if (term is Application) 
			{
				Application application = (Application)(object)term;
				
				if (application.Function is Abstraction)
				{
					Abstraction abstraction = (Abstraction)application.Function;
					
					return (T)(object)abstraction.Term.Substitute(abstraction.Variable, application.Parameter);
				}
			}
			
			throw new InvalidOperationException();
		}
	}
}

