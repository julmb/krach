using System;
using System.Collections.Generic;
using Krach.Calculus.Terms.Composite;
using Krach.Extensions;

namespace Krach.Calculus.Terms.Notation.Composite
{
	public class VectorSyntax : ValueSyntax
	{
		readonly IEnumerable<ValueTerm> terms;

		public VectorSyntax(IEnumerable<ValueTerm> terms)
		{
			if (terms == null) throw new ArgumentNullException("terms");

			this.terms = terms;
		}

		public override string GetText()
		{
			return string.Format("[{0}]", terms.ToStrings().Separate(", ").AggregateString());
		}
	}
}

