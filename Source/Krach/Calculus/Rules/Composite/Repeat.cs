using System;
using Krach.Calculus.Terms.Composite;
using Krach.Calculus.Terms;
using System.Linq;
using System.Collections.Generic;
using Krach.Extensions;
using Krach.Calculus.Terms.Basic;
using Krach.Calculus;

namespace Krach.Calculus.Rules.Composite
{
	public class Repeat : Rule
	{
		readonly Rule rule;

		public Repeat(Rule rule)
		{
			if (rule == null) throw new ArgumentException("rule");

			this.rule = rule;
		}

		public override bool Matches<T>(T term)
		{
			return true;
		}
		public override T Rewrite<T>(T term)
		{
			while (rule.Matches(term))
			{
//				Terminal.Write(term.ToString(), ConsoleColor.Yellow);
//				Terminal.WriteLine();

				term = rule.Rewrite(term);
			}

			return term;
		}
	}
}

