using System;
using System.Collections.Generic;
using Krach.Calculus.Terms.Composite;
using Krach.Extensions;

namespace Krach.Calculus.Terms.Notation.Composite
{
	public class ApplicationSyntax : ValueSyntax
	{
		readonly FunctionTerm function;
		readonly ValueTerm parameter;

		public ApplicationSyntax(FunctionTerm function, ValueTerm parameter)
		{
			if (function == null) throw new ArgumentNullException("function");
			if (parameter == null) throw new ArgumentNullException("parameter");

			this.function = function;
			this.parameter = parameter;
		}

		public override string GetText()
		{
			return string.Format("({0} {1})", function, parameter);
		}
	}
}

