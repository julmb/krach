using System;

namespace Krach.Terms
{
	public class Constant : Term
	{
		readonly double value;
		
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
		public override Term GetDerivative(Variable variable)
		{
			return Term.Constant(0);
		}
		public override Term Substitute(Variable variable, Term substitute) 
		{
			return this;
		}
	}
}

