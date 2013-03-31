using System;
using System.Collections.Generic;
using Krach.Calculus.Terms.Composite;
using Krach.Extensions;
using System.Linq;

namespace Krach.Calculus.Terms.Notation.Custom
{
	public class BasicBinaryOperatorSyntax : BinaryOperatorSyntax
	{
        readonly string text;

        public BasicBinaryOperatorSyntax(string text)
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

