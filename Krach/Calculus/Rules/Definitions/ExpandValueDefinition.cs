using System;
using System.Linq;
using Krach.Calculus.Terms.Composite;
using Krach.Calculus.Terms;
using System.Collections.Generic;
using Krach.Extensions;
using Krach.Calculus.Terms.Basic.Definitions;
using System.Text.RegularExpressions;

namespace Krach.Calculus.Rules.Definitions
{
	public class ExpandValueDefinition : Rule
	{
		readonly string pattern;

		public ExpandValueDefinition(string pattern)
		{
			if (pattern == null) throw new ArgumentNullException("pattern");

			this.pattern = pattern;
		}
		public ExpandValueDefinition() : this(".*") { }

		public override string ToString()
		{
			return "expand_value";
		}
		public override T Rewrite<T>(T term)
		{
			if (!(term is ValueDefinition)) return null;

            ValueDefinition valueDefinition = ((ValueDefinition)(BaseTerm)term);

			if (!Regex.IsMatch(valueDefinition.Name, pattern)) return null;

            return (T)(BaseTerm)valueDefinition.Value;
		}
	}
}

