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
	public class SelectVector : Rule
	{
		public override bool Matches<T>(T term)
		{
			if (!(term is Selection)) return false;

			Selection selection = (Selection)(BaseTerm)term;
			
			if (!(selection.Term is Vector)) return false;

			return true;
		}
		public override T Rewrite<T>(T term)
		{
			if (!(term is Selection)) throw new InvalidOperationException();

			Selection selection = (Selection)(BaseTerm)term;
			
			if (!(selection.Term is Vector)) throw new InvalidOperationException();

			Vector vector = (Vector)selection.Term;

			return (T)(BaseTerm)Select(vector.Terms, selection.Index);
		}

		static ValueTerm Select(IEnumerable<ValueTerm> terms, int index)
		{
			return
			(
				from subTerm in terms
				from subIndex in Enumerable.Range(0, subTerm.Dimension)
				select subTerm.Select(subIndex)
			)
			.ElementAt(index);
		}
	}
}

