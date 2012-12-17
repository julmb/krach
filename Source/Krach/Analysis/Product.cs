using System;

namespace Krach.Analysis
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

		public override double Evaluate(Assignment assignment)
		{
			return term1.Evaluate(assignment) * term2.Evaluate(assignment);
		}
		public override Term GetDerivative(Variable variable)
		{
			return new Sum(new Product(term1, term2.GetDerivative(variable)), new Product(term1.GetDerivative(variable), term2));
		}
	}
}

