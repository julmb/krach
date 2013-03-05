using System;
using System.Collections.Generic;

namespace Krach.Terms
{
	public abstract class Function : LambdaTerm<Function>
	{
		public abstract IEnumerable<Function> Jacobian { get; }
		
		public abstract double Evaluate(IEnumerable<double> values);
		public override Function Substitute(Variable variable, Term substitute)
		{
			return this;
		}
	}
}
