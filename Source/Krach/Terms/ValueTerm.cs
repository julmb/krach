using System;
using Krach.Terms.LambdaTerms;

namespace Krach.Terms
{
	public abstract class ValueTerm : Term<ValueTerm>
	{
		public abstract string GetText();
		public abstract double Evaluate();
		public abstract ValueTerm GetDerivative(Variable variable);
	}
}

