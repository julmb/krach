using System;
using System.Collections.Generic;
using Krach.Calculus.Terms.Composite;
using Krach.Extensions;

namespace Krach.Calculus.Terms.Notation.Composite
{
	public class VariableSyntax : ValueSyntax
	{
		readonly string name;

		public VariableSyntax(string name)
		{
			if (name == null) throw new ArgumentNullException("name");

			this.name = name;
		}

		public override string GetText()
		{
			return name;
		}
	}
}

