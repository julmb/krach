using System;
using Krach.Extensions;

namespace Krach.Calculus
{
	public class Exponentiation : Term
	{
		readonly Term term;
		readonly Constant exponent;

		public Exponentiation(Term term, Constant exponent)
		{
			if (term == null) throw new ArgumentNullException("term");
			if (exponent == null) throw new ArgumentNullException("exponent");

			this.term = term;
			this.exponent = exponent;
		}

		public override double Evaluate()
		{
			return Scalars.Exponentiate(term.Evaluate(), exponent.Evaluate());
		}
		public override Term Substitute(Variable variable, Term substitute)
		{
			return new Exponentiation(term.Substitute(variable, substitute), exponent);
		}
		public override Term GetDerivative(Variable variable)
		{
			return new Product(term.GetDerivative(variable), new Product(exponent, new Exponentiation(term, new Constant(exponent.Value - 1))));
		}
	}
}

