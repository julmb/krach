using System;

namespace Krach.Analysis
{
	public abstract class Term
	{
		public abstract double Evaluate(Assignment assignment);
		public abstract Term GetDerivative(Variable variable);
	}
}

