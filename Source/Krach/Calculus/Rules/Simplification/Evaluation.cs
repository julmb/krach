using System;
using Krach.Calculus.Terms.Composite;
using Krach.Calculus.Terms;
using System.Linq;
using System.Collections.Generic;
using Krach.Extensions;
using Krach.Calculus.Terms.Basic;
using Krach.Calculus;
using Krach.Calculus.Terms.Basic.Atoms;
using Krach.Calculus.Terms.Basic.Definitions;

namespace Krach.Calculus.Rules.Simplification
{
	public class Evaluation : Rule
	{
		public override string ToString()
		{
			return "evaluation";
		}
		public override T Rewrite<T>(T term)
		{
			if (!(term is ValueTerm)) return null;
			
			ValueTerm valueTerm = (ValueTerm)(BaseTerm)term;

			if (valueTerm is ValueDefinition) return null;
            if (valueTerm is Constant) return null;
            if (valueTerm is Vector && ((Vector)valueTerm).Terms.Count() != 1 && ((Vector)valueTerm).Terms.All(subTerm => subTerm is Constant)) return null;

			if (valueTerm.GetFreeVariables().Any()) return null;

			IEnumerable<double> values = valueTerm.Evaluate();

			ValueTerm result = values.Count() == 1 ? (ValueTerm)new Constant(values.Single()) : (ValueTerm)new Vector(values.Select(value => new Constant(value)));

			return (T)(BaseTerm)result;
		}
	}
}

