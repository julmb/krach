using System;

namespace Krach.Terms.LambdaTerms
{
	public class Constant : ValueTerm
	{
		readonly double value;
		
		public Constant(double value)
		{
			this.value = value;
		}
		
		public override ValueTerm Substitute(Variable variable, ValueTerm substitute) 
		{
			return this;
		}
		public override string GetText()
		{
			return value.ToString();
		}
		public override double Evaluate()
		{
			return value;
		}
		public override ValueTerm GetDerivative(Variable variable)
		{
			return Term.Constant(0);
		}
	}
}

