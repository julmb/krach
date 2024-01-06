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
	public class ExpandAppliedFunctionDefinition : Rule
	{
		readonly string pattern;

		public ExpandAppliedFunctionDefinition(string pattern)
		{
			if (pattern == null) throw new ArgumentNullException("pattern");

			this.pattern = pattern;
		}
		public ExpandAppliedFunctionDefinition() : this(".*") { }

		public override string ToString()
		{
			return "expand_function!";
		}
		public override T Rewrite<T>(T term)
        {
            if (!(term is Application)) return null;

            Application application = (Application)(BaseTerm)term;

            if (!(application.Function is FunctionDefinition)) return null;

            FunctionDefinition functionDefinition = (FunctionDefinition)application.Function;

			if (!Regex.IsMatch(functionDefinition.Name, pattern)) return null;

			return (T)(BaseTerm)new Application(functionDefinition.Function, application.Parameter);
		}
	}
}

