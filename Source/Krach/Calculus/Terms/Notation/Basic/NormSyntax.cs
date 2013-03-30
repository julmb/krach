using System;
using System.Collections.Generic;
using Krach.Calculus.Terms.Composite;
using Krach.Extensions;
using System.Linq;

namespace Krach.Calculus.Terms.Notation.Basic
{
	public class NormSyntax : BasicFunctionSyntax
	{
		public NormSyntax(string name) : base(name) { }

		public override ValueSyntax GetApplicationSyntax(ValueTerm parameter)
		{
			return new BasicValueSyntax(string.Format("|{0}|", parameter));
		}
	}
}

