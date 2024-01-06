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
		public override string ToString()
		{
			return "singleton_vector";
		}
		public override T Rewrite<T>(T term)
		{
			if (!(term is Vector)) return null;

			Vector vector = (Vector)(BaseTerm)term;
			
			if (vector.Terms.Count() != 1) return null;

			return (T)(BaseTerm)vector.Terms.Single();
		}
	}
}

