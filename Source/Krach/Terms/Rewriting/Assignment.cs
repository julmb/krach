using System;
using Krach.Terms.LambdaTerms;

namespace Krach.Terms.Rewriting
{
	public class Assignment
	{
		readonly Variable variable;
		readonly Value term;
		
		public Variable Variable { get { return variable; } }
		public Value Term { get { return term; } }
		
		public Assignment(Variable variable, Value term)
		{
			if (variable == null) throw new ArgumentNullException("variable");
			if (term == null) throw new ArgumentNullException("term");
			
			this.variable = variable;
			this.term = term;
		}
	}
}

