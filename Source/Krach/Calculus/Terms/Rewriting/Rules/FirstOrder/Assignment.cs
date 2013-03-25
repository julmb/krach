using System;
using Krach.Calculus.Terms;
using Krach.Calculus.Terms.Combination;

namespace Krach.Calculus.Terms.Rewriting.Rules.FirstOrder
{
	public class Assignment
	{
		readonly Variable variable;
		readonly ValueTerm term;
		
		public Variable Variable { get { return variable; } }
		public ValueTerm Term { get { return term; } }
		
		public Assignment(Variable variable, ValueTerm term)
		{
			if (variable == null) throw new ArgumentNullException("variable");
			if (term == null) throw new ArgumentNullException("term");
			
			this.variable = variable;
			this.term = term;
		}
	}
}

