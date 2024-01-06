using System;
using System.Collections.Generic;
using Krach.Calculus.Terms.Composite;
using Krach.Extensions;
using System.Linq;

namespace Krach.Calculus.Terms.Notation.Custom
{
	public class ExponentiationSyntax : BinaryOperatorSyntax
	{
        public override string GetText()
        {
            return "^";
        }
		public override Syntax GetApplicationSyntax(Application application)
		{
            IEnumerable<ValueTerm> parameters = application.Parameter is Vector ? ((Vector)application.Parameter).Terms : Enumerables.Create(application.Parameter);

			string parameter0Text = parameters.ElementAt(0).ToString();
			string parameter1Text = parameters.ElementAt(1).ToString();

            if (!parameter1Text.IsSubSuperScriptCompatible()) return base.GetApplicationSyntax(application);

			return new BasicSyntax(string.Format("{0}{1}", parameter0Text, parameter1Text.ToSuperscript()));
		}
	}
}

