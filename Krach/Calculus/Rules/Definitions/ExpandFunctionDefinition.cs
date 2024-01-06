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
	public class ExpandFunctionDefinition : Rule
	{
		readonly string pattern;

		public ExpandFunctionDefinition(string pattern)
		{
			if (pattern == null) throw new ArgumentNullException("pattern");

			this.pattern = pattern;
		}
		public ExpandFunctionDefinition() : this(".*") { }

		public override string ToString()
		{
			return "expand_function";
		}
		public override T Rewrite<T>(T term)
		{
			if (!(term is FunctionDefinition)) return null;

            FunctionDefinition functionDefinition = ((FunctionDefinition)(BaseTerm)term);

			if (!Regex.IsMatch(functionDefinition.Name, pattern)) return null;

            return (T)(BaseTerm)functionDefinition.Function;
		}
	}
}

