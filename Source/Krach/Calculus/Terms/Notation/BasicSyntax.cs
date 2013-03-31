using System;
using System.Collections.Generic;

namespace Krach.Calculus.Terms.Notation
{
    public class BasicSyntax : Syntax
	{
		readonly string text;

		public BasicSyntax(string text)
		{
			if (text == null) throw new ArgumentNullException("text");

			this.text = text;
		}

		public override string GetText()
		{
			return text;
		}
	}
}

