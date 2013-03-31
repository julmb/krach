using System;
using System.Linq;
using Krach.Calculus.Terms.Composite;
using Krach.Calculus.Terms;
using System.Collections.Generic;
using Krach.Extensions;
using Krach.Calculus.Terms.Basic.Definitions;

namespace Krach.Calculus.Rules.Definitions
{
	public class ExpandFunctionDefinition : Rule
	{
		public override string ToString()
		{
			return "expand_function";
		}
		public override T Rewrite<T>(T term)
		{
			if (!(term is FunctionDefinition)) return null;

            FunctionDefinition functionDefinition = ((FunctionDefinition)(BaseTerm)term);

            return (T)(BaseTerm)functionDefinition.Function;
		}
	}
}

