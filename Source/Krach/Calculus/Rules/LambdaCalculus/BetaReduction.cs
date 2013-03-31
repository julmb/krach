using System;
using Krach.Calculus.Terms.Composite;
using Krach.Calculus.Terms;
using System.Linq;
using System.Collections.Generic;
using Krach.Extensions;

namespace Krach.Calculus.Rules.LambdaCalculus
{
	public class BetaReduction : Rule
	{
		public override string ToString()
		{
			return "β";
		}
		public override T Rewrite<T>(T term)
		{
			if (!(term is Application)) return null;
			
			Application application = (Application)(BaseTerm)term;
			
			if (!(application.Function is Abstraction)) return null;

			Abstraction abstraction = (Abstraction)application.Function;
			
			return (T)(BaseTerm)Reduce(abstraction.Variables, abstraction.Term, application.Parameter);
		}

		static ValueTerm Reduce(IEnumerable<Variable> variables, ValueTerm term, ValueTerm parameter)
		{
			IEnumerable<ValueTerm> substitutes = Enumerable.Zip
			(
				variables.Select(variable => variable.Dimension).GetPartialSums(),
				variables.Select(variable => variable.Dimension),
				(start, length) => new Vector
				(
					from index in Enumerable.Range(start, length)
					select new Selection(parameter, index)
				)
			)
			.ToArray();

			return term.Substitute(variables, substitutes);
		}
	}
}

