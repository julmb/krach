using System;
using Krach.Calculus.Terms.Composite;
using Krach.Calculus.Terms;
using System.Linq;
using System.Collections.Generic;
using Krach.Extensions;
using Krach.Calculus.Terms.Basic;
using Krach.Calculus;
using Krach.Calculus.Terms.Basic.Atoms;

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

			if (valueTerm.GetFreeVariables().Any()) return null;

			IEnumerable<double> values = Rewriting.Expansion.Rewrite(valueTerm).Evaluate();

			ValueTerm result = values.Count() == 1 ? (ValueTerm)new Constant(values.Single()) : (ValueTerm)new Vector(values.Select(value => new Constant(value)));

			if (result == valueTerm) return null;

			return (T)(BaseTerm)result;
		}
	}
}

