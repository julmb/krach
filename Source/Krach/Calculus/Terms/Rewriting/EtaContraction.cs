using System;
using System.Linq;
using Krach.Calculus.Terms.Combination;
using Krach.Calculus.Terms;

namespace Krach.Terms.Rewriting
{
	public class EtaContraction : Rule
	{
		public override bool Matches<T>(T term)
		{
			try { Rewrite(term); }
			catch (InvalidOperationException) { return false; }
			
			return true;
		}
		public override T Rewrite<T>(T term)
		{
			if (term is Abstraction) 
			{
				Abstraction abstraction = (Abstraction)(BaseTerm)term;
				
				if (abstraction.Term is Application)
				{
					Application application = (Application)abstraction.Term;
					
					if (abstraction.Variables.Count() == 1 && abstraction.Variables.Single() == application.Parameter)
						return (T)(BaseTerm)application.Function;
				}
			}
			
			throw new InvalidOperationException();
		}
	}
}

