using System;
using System.Linq;
using System.Collections.Generic;
using Krach.Extensions;

namespace Krach.Terms.LambdaTerms
{
	public class Abstraction : FunctionTerm
	{
		readonly IEnumerable<Variable> variables;
		readonly ValueTerm term;
		
		public Abstraction(IEnumerable<Variable> variables, ValueTerm term)
		{
			if (variables == null) throw new ArgumentNullException("variables");
			if (term == null) throw new ArgumentNullException("term");
			
			this.variables = variables;
			this.term = term;
		}

		public override FunctionTerm Substitute(Variable variable, ValueTerm substitute) 
		{
			if (variables.Contains(variable)) return this;
			
			return new Abstraction(variables, term.Substitute(variable, substitute)); 
		}
		public override string GetText(IEnumerable<string> parameterTexts)
		{
			return string.Format
			(
				"(λ {0}. {1}: {2})", 
				variables.Select(variable => variable.GetText()).Separate(" ").AggregateString(), 
				term.GetText(),
				parameterTexts.Separate(" ").AggregateString()
			);
		}
		public override double Evaluate(IEnumerable<double> values)
		{
			return term.Substitute(variables, values.Select(value => new Constant(value))).Evaluate();
		}
		public override IEnumerable<FunctionTerm> GetJacobian() 
		{
			return 
				from variable in variables
				select term.GetDerivative(variable).Abstract(variables);
		}
	}
}

