using System;
using Krach.Terms.LambdaTerms;
using System.Collections.Generic;
using Krach.Extensions;

namespace Krach.Terms.Functions
{
	public abstract class Function : FunctionTerm
	{
		public override FunctionTerm Substitute(Variable variable, ValueTerm substitute)
		{
			return this;
		}
		public override string GetText(IEnumerable<string> parameterTexts)
		{
			return string.Format("({0} {1})", GetType().Name, parameterTexts.Separate(" ").AggregateString());
		}
	}
}

