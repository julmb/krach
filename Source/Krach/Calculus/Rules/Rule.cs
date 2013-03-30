using System;
using Krach.Calculus.Terms;

namespace Krach.Calculus.Rules
{
	public abstract class Rule 
	{
		public abstract T Rewrite<T>(T term) where T : VariableTerm<T>;
	}
}
