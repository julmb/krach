using System;

namespace Krach.Calculus.Terms
{
	public class Product : Term
	{
		readonly Term term1;
		readonly Term term2;

		public Product(Term term1, Term term2)
		{
			if (term1 == null) throw new ArgumentNullException("term1");
			if (term2 == null) throw new ArgumentNullException("term2");

			this.term1 = term1;
			this.term2 = term2;
		}
	
		public override string ToString()
		{
			return string.Format("({0}) * ({1})", term1, term2);
		}
		public override double Evaluate()
		{
			return term1.Evaluate() * term2.Evaluate();
		}
		public override Term Substitute(Variable variable, Term substitute)
		{
			return new Product(term1.Substitute(variable, substitute), term2.Substitute(variable, substitute));
		}
		public override Term GetDerivative(Variable variable)
		{
			return new Sum(new Product(term1, term2.GetDerivative(variable)), new Product(term1.GetDerivative(variable), term2));
		}
	}
}

