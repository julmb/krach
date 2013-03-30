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
		public override bool Matches<T>(T term)
		{
			if (!(term is Application)) return false;
			
			Application application = (Application)(BaseTerm)term;
			
			if (!(application.Function is Abstraction)) return false;

			return true;
		}
		public override T Rewrite<T>(T term)
		{
			if (!(term is Application)) throw new InvalidOperationException();
			
			Application application = (Application)(BaseTerm)term;
			
			if (!(application.Function is Abstraction)) throw new InvalidOperationException();

			Abstraction abstraction = (Abstraction)application.Function;
			
			return (T)(BaseTerm)Reduce(abstraction.Variables, abstraction.Term, application.Parameter);
		}

		static ValueTerm Reduce(IEnumerable<Variable> variables, ValueTerm term, ValueTerm parameter)
		{
			IEnumerable<ValueTerm> substitutes = Enumerable.Zip
			(
				variables.Select(variable => variable.Dimension).GetPartialSums(),
				variables.Select(variable => variable.Dimension),
				(start, length) => Term.Vector
				(
					from index in Enumerable.Range(start, length)
					select parameter.Select(index)
				)
			)
			.ToArray();

			return term.Substitute(variables, substitutes);
		}
	}
}

