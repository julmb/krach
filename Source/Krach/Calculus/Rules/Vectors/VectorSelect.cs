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
	public class VectorSelect : Rule
	{
		public override string ToString()
		{
			return "vector_select";
		}
		public override T Rewrite<T>(T term)
		{
			if (!(term is Vector)) return null;

			Vector vector = (Vector)(BaseTerm)term;

			if (!vector.Terms.All(subTerm => subTerm is Selection)) return null;

			IEnumerable<Selection> selections = vector.Terms.Cast<Selection>();

			IEnumerable<ValueTerm> terms = selections.Select(selection => selection.Term).Distinct().ToArray();

			if (terms.Count() != 1) return null;

			if (vector != GetSelectionVector(terms.Single())) return null;

			return (T)(BaseTerm)terms.Single();
		}

		static Vector GetSelectionVector(ValueTerm term)
		{
			return new Vector
			(
				from index in Enumerable.Range(0, term.Dimension)
				select new Selection(term, index)
			);
		}
	}
}

