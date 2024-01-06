using System;
using System.Collections.Generic;
using Krach.Calculus.Terms.Composite;
using Krach.Extensions;
using System.Linq;

namespace Krach.Calculus.Terms.Notation.Custom
{
	public abstract class BinaryOperatorSyntax : Syntax
	{
		public override Syntax GetApplicationSyntax(Application application)
		{
            IEnumerable<ValueTerm> parameters = application.Parameter is Vector ? ((Vector)application.Parameter).Terms : Enumerables.Create(application.Parameter);

            if (parameters.Count() != 2) return base.GetApplicationSyntax(application);
			
			return new BasicSyntax(string.Format("({0} {1} {2})", parameters.ElementAt(0), GetText(), parameters.ElementAt(1)));
		}
	}
}

