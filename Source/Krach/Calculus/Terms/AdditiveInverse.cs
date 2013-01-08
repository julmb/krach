using System;

namespace Krach.Calculus.Terms
{
	public class AdditiveInverse : Term
	{
		readonly Term term;

		public AdditiveInverse(Term term)
		{
			if (term == null) throw new ArgumentNullException("term");

			this.term = term;
		}

		public override double Evaluate()
		{
			return - term.Evaluate();
		}
		public override Term Substitute(Variable variable, Term substitute)
		{
			return new AdditiveInverse(term.Substitute(variable, substitute));
		}
		public override Term GetDerivative(Variable variable)
		{
			return new AdditiveInverse(term.GetDerivative(variable));
		}
	}
}

