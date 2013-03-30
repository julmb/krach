using System;
using System.Collections.Generic;
using Krach.Calculus.Terms.Composite;
using Krach.Extensions;
using System.Linq;

namespace Krach.Calculus.Terms.Notation.Basic
{
	public class ExponentiationSyntax : BinaryOperatorSyntax
	{
		public ExponentiationSyntax(string name) : base(name) { }

		public override ValueSyntax GetApplicationSyntax(ValueTerm parameter)
		{
			IEnumerable<ValueTerm> parameters = parameter is Vector ? ((Vector)parameter).Terms : Enumerables.Create(parameter);

			string parameter0Text = parameters.ElementAt(0).ToString();
			string parameter1Text = parameters.ElementAt(1).ToString();

			if (!parameter1Text.IsSubSuperScriptCompatible()) return base.GetApplicationSyntax(parameter);

			return new BasicValueSyntax(string.Format("{0}{1}", parameter0Text, parameter1Text.ToSuperscript()));
		}
	}
}

