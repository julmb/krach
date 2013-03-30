using System;
using System.Collections.Generic;

namespace Krach.Calculus.Terms.Notation.Basic
{
	public class BasicFunctionSyntax : FunctionSyntax
	{
		readonly string name;

		public BasicFunctionSyntax(string name)
		{
			if (name == null) throw new ArgumentNullException("name");

			this.name = name;
		}

		public override string GetText()
		{
			return name;
		}
		public override ValueSyntax GetApplicationSyntax(ValueTerm parameter)
		{
			return null;
		}
	}
}

