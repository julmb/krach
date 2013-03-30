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
		public override bool Matches<T>(T term)
		{
			return term is FunctionDefinition;
		}
		public override T Rewrite<T>(T term)
		{
			return (T)(BaseTerm)((FunctionDefinition)(BaseTerm)term).Function;
		}
	}
}

