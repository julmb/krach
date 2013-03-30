using System;
using Krach.Calculus.Terms;
using System.Linq;
using System.Collections.Generic;
using Krach.Extensions;
using Krach.Calculus.Terms.Basic;
using Krach.Calculus;

namespace Krach.Calculus.Rules.Composite
{
	public class Any : Rule
	{
		readonly IEnumerable<Rule> rules;

		public Any(IEnumerable<Rule> rules)
		{
			if (rules == null) throw new ArgumentException("rules");

			this.rules = rules;
		}
		public Any(params Rule[] rules) : this((IEnumerable<Rule>)rules) { }

		public override bool Matches<T>(T term)
		{
			return rules.Any(rule => rule.Matches(term));
		}
		public override T Rewrite<T>(T term)
		{
			Rule matchingRule = rules.First(rule => rule.Matches(term));

//			Terminal.Write(string.Format("{0}: {1}", matchingRule.GetType().Name, term.GetText()), ConsoleColor.Cyan);
//			Terminal.WriteLine();

			return matchingRule.Rewrite(term);
		}
	}
}

