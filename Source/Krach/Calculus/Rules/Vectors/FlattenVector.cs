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
	public class FlattenVector : Rule
	{
		public override bool Matches<T>(T term)
		{
			if (!(term is Vector)) return false;

			Vector vector = (Vector)(BaseTerm)term;
			
			if (!vector.Terms.Any(subTerm => subTerm is Vector)) return false;

			return true;
		}
		public override T Rewrite<T>(T term)
		{
			if (!(term is Vector)) throw new InvalidOperationException();

			Vector vector = (Vector)(BaseTerm)term;
			
			if (!vector.Terms.Any(subTerm => subTerm is Vector)) throw new InvalidOperationException();

			return (T)(BaseTerm)Term.Vector
			(
				from subTerm in vector.Terms
				let subSubTerms = subTerm is Vector ? ((Vector)subTerm).Terms : Enumerables.Create(subTerm)
				from subSubTerm in subSubTerms
				select subSubTerm
			);
		}
	}
}

