using System;

namespace Krach.Calculus.Terms
{
	public class Constant : Term
	{
		readonly double value;

		public double Value { get { return value; } }

		public Constant(double value)
		{
			this.value = value;
		}
		
		public override string ToString()
		{
			return value.ToString();
		}
		public override double Evaluate()
		{
			return value;
		}
		public override Term Substitute(Variable variable, Term term)
		{
			return this;
		}
		public override Term GetDerivative(Variable variable)
		{
			return new Constant(0);
		}
	}
}

