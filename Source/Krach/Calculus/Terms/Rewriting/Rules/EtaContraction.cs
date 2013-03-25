using System;
using System.Linq;
using Krach.Calculus.Terms.Combination;
using Krach.Calculus.Terms;

namespace Krach.Calculus.Terms.Rewriting.Rules
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
			if (!(term is Abstraction)) throw new InvalidOperationException();
			
			Abstraction abstraction = (Abstraction)(BaseTerm)term;
			
			if (!(abstraction.Term is Application)) throw new InvalidOperationException();
		
			Application application = (Application)abstraction.Term;
			
			if (abstraction.Variables.Count() != 1) throw new InvalidOperationException();
			if (abstraction.Variables.Single() != application.Parameter) throw new InvalidOperationException();
				
			return (T)(BaseTerm)application.Function;
		}
	}
}

