using System;
using System.Collections.Generic;
using Krach.Calculus.Terms.Composite;
using Krach.Extensions;
using System.Linq;

namespace Krach.Calculus.Terms.Notation.Custom
{
	public class NormSyntax : Syntax
	{
        public override string GetText()
        {
            return "norm";
        }
		public override Syntax GetApplicationSyntax(Application application)
		{
            return new BasicSyntax(string.Format("|{0}|", application.Parameter));
		}
	}
}

