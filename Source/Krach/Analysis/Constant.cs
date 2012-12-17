using System;

namespace Krach.Analysis
{
	public class Constant : Term
	{
		readonly double value;

		public Constant(double value)
		{
			this.value = value;
		}

		public override double Evaluate(Assignment assignment)
		{
			return value;
		}
		public override Term GetDerivative(Variable variable)
		{
			return new Constant(0);
		}
	}
}

