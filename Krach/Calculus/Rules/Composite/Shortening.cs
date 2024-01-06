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
	public class Shortening : Rule
	{
		readonly Rule rule;

		public Rule Rule { get { return rule; } }

        public Shortening(Rule rule)
		{
			if (rule == null) throw new ArgumentException("rule");

			this.rule = rule;
		}

		public override string ToString()
		{
			return string.Format(">{0}<", rule);
		}
		public override T Rewrite<T>(T term)
		{
            T rewrittenTerm = rule.Rewrite(term);

            if (rewrittenTerm == null) return null;

			if (rewrittenTerm.ToString().Length >= term.ToString().Length) return null;

            return rewrittenTerm;
		}
	}
}

