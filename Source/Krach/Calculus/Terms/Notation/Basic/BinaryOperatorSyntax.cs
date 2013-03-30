using System;
using System.Collections.Generic;
using Krach.Calculus.Terms.Composite;
using Krach.Extensions;
using System.Linq;

namespace Krach.Calculus.Terms.Notation.Basic
{
	public class BinaryOperatorSyntax : BasicFunctionSyntax
	{
		public BinaryOperatorSyntax(string name) : base(name) { }

		public override ValueSyntax GetApplicationSyntax(ValueTerm parameter)
		{
			IEnumerable<ValueTerm> parameters = parameter is Vector ? ((Vector)parameter).Terms : Enumerables.Create(parameter);

			if (parameters.Count() != 2) return base.GetApplicationSyntax(parameter);
			
			return new BasicValueSyntax(string.Format("({0} {1} {2})", parameters.ElementAt(0), GetText(), parameters.ElementAt(1)));
		}
	}
}

