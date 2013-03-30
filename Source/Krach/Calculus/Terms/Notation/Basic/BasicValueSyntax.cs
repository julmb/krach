using System;
using System.Collections.Generic;

namespace Krach.Calculus.Terms.Notation.Basic
{
	public class BasicValueSyntax : ValueSyntax
	{
		readonly string text;

		public BasicValueSyntax(string text)
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

