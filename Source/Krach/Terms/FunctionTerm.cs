using System;
using System.Collections.Generic;

namespace Krach.Terms
{
	public abstract class FunctionTerm : Term<FunctionTerm>
	{	
		public abstract string GetText(IEnumerable<string> parameterTexts);
		public abstract double Evaluate(IEnumerable<double> values);
		public abstract IEnumerable<FunctionTerm> GetJacobian();
	}
}
