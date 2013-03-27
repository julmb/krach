using System;
using System.Linq;
using Krach.Calculus.Terms.Combination;
using Krach.Calculus.Terms;
using Krach.Calculus.Terms.Basic;
using Krach.Calculus;
using System.Collections.Generic;

namespace Krach.Calculus.Terms.Rewriting.Rules
{
	public class Evaluation : Rule
	{
		public override bool Matches<T>(T term)
		{
			try { Rewrite(term); }
			catch (InvalidOperationException) { return false; }
			
			return true;
		}
		public override T Rewrite<T>(T term)
		{
			if (!(term is ValueTerm)) throw new InvalidOperationException();
			
			ValueTerm valueTerm = (ValueTerm)(BaseTerm)term;
			
			if (valueTerm is Constant) throw new InvalidOperationException();
			
			if (valueTerm.GetFreeVariables().Any()) throw new InvalidOperationException();
			
			return (T)(BaseTerm)Term.Constant(valueTerm.Evaluate());
		}
	}
}

