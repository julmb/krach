using System;
using Krach.Calculus.Terms;

namespace Krach.Calculus.Rules
{
	public abstract class Rule 
	{
		public abstract bool Matches<T>(T term) where T : VariableTerm<T>;
		public abstract T Rewrite<T>(T term) where T : VariableTerm<T>;
		public T TryRewrite<T>(T term) where T : VariableTerm<T>
		{
			if (Matches(term)) return Rewrite(term);

			return term;
		}
	}
}
