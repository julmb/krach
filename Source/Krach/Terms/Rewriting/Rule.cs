using System;

namespace Krach.Terms.Rewriting
{
	public abstract class Rule 
	{
		public abstract bool Matches<T>(T term) where T : Term<T>;
		public abstract T Rewrite<T>(T term) where T : Term<T>;
	}
}
