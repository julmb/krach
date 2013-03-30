using System;
using System.Linq;
using Krach.Calculus.Terms.Composite;
using Krach.Calculus.Terms;
using System.Collections.Generic;
using Krach.Extensions;
using Krach.Calculus.Terms.Basic.Definitions;

namespace Krach.Calculus.Rules.Definitions
{
	public class ExpandValueDefinition : Rule
	{
		public override string ToString()
		{
			return "expand_value_definition";
		}
		public override T Rewrite<T>(T term)
		{
			if (!(term is ValueDefinition)) return null;

			return (T)(BaseTerm)((ValueDefinition)(BaseTerm)term).Value;
		}
	}
}

