using System;
using System.Collections.Generic;
using Krach.Calculus.Terms.Composite;
using Krach.Extensions;
using System.Linq;

namespace Krach.Calculus.Terms.Notation.Default
{
    public class AbstractionSyntax : Syntax
	{
		readonly IEnumerable<Variable> variables;
		readonly ValueTerm term;

		public AbstractionSyntax(IEnumerable<Variable> variables, ValueTerm term)
		{
			if (variables == null) throw new ArgumentNullException("variables");
			if (term == null) throw new ArgumentNullException("term");

			this.variables = variables;
			this.term = term;
		}

		public override string GetText()
		{
            if (!variables.Any()) return string.Format("(λ. {0})", term);

			return string.Format("(λ {0}. {1})", variables.ToStrings().Separate(" ").AggregateString(), term);
		}
	}
}

