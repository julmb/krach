using System;
using System.Linq;
using System.Collections.Generic;
using Krach.Extensions;

namespace Krach.Terms
{
	public class Abstraction : Function
	{
		readonly IEnumerable<Variable> variables;
		readonly Term term;
		
		public override IEnumerable<Function> Jacobian 
		{
			get 
			{
				return 
					from variable in variables
					select new Abstraction(variables, term.GetDerivative(variable));
			}
		}
		
		public Abstraction(IEnumerable<Variable> variables, Term term)
		{
			if (variables == null) throw new ArgumentNullException("variables");
			if (term == null) throw new ArgumentNullException("term");
			
			this.variables = variables;
			this.term = term;
		}

		public override string ToString()
		{
			return string.Format("λ {0}. {1}", variables.ToStrings().Separate(" ").AggregateString(), term);
		}
		public override double Evaluate(IEnumerable<double> values)
		{
			return term.Substitute(variables, values.Select(value => new Constant(value))).Evaluate();
		}
		public override Function Substitute(Variable variable, Term substitute) 
		{
			if (variables.Contains(variable)) return this;
			
			return new Abstraction(variables, term.Substitute(variable, substitute)); 
		}
	}
}

