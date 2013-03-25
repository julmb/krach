using System;
using Krach.Calculus.Terms;

namespace Krach.Calculus.Terms.Rewriting
{
	public abstract class Rule 
	{
		public abstract bool Matches<T>(T term) where T : VariableTerm<T>;
		public abstract T Rewrite<T>(T term) where T : VariableTerm<T>;
	}
}
