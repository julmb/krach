using System;
using System.Collections.Generic;
using Krach.Calculus.Terms.Composite;
using Krach.Extensions;

namespace Krach.Calculus.Terms.Notation.Composite
{
	public class AbstractionSyntax : FunctionSyntax
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
			return string.Format("(λ {0}. {1})", variables.ToStrings().Separate(" ").AggregateString(), term);
		}
		public override ValueSyntax GetApplicationSyntax(ValueTerm parameter)
		{
			return null;
		}
	}
}

