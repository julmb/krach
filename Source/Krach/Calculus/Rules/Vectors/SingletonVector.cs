using System;
using Krach.Calculus.Terms.Composite;
using Krach.Calculus.Terms;
using System.Linq;
using System.Collections.Generic;
using Krach.Extensions;
using Krach.Calculus.Terms.Basic;
using Krach.Calculus;

namespace Krach.Calculus.Rules.Vectors
{
	public class SingletonVector : Rule
	{
		public override bool Matches<T>(T term)
		{
			if (!(term is Vector)) return false;

			Vector vector = (Vector)(BaseTerm)term;
			
			if (vector.Terms.Count() != 1) return false;

			return true;
		}
		public override T Rewrite<T>(T term)
		{
			if (!(term is Vector)) throw new InvalidOperationException();

			Vector vector = (Vector)(BaseTerm)term;
			
			if (vector.Terms.Count() != 1) throw new InvalidOperationException();

			return (T)(BaseTerm)vector.Terms.Single();
		}
	}
}

