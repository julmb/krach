using System;
using System.Linq;
using Krach.Terms.LambdaTerms;

namespace Krach.Terms.Rewriting
{
	public class EtaContraction : Rule
	{
		public override bool Matches<T>(T term)
		{
			if (term is Abstraction) 
			{
				Abstraction abstraction = (Abstraction)(object)term;
				
				if (abstraction.Term is Application)
				{
					Application application = (Application)abstraction.Term;
					
					if (Enumerable.SequenceEqual(abstraction.Variables, application.Parameters)) 
						return true;
				}
			}
			
			return false;
		}
		public override T Rewrite<T>(T term)
		{
			if (term is Abstraction) 
			{
				Abstraction abstraction = (Abstraction)(object)term;
				
				if (abstraction.Term is Application)
				{
					Application application = (Application)abstraction.Term;
					
					if (Enumerable.SequenceEqual(abstraction.Variables, application.Parameters)) 
						return (T)(object)application.Function;
				}
			}
			
			throw new InvalidOperationException();
		}
	}
}

