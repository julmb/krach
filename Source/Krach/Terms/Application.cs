using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;

namespace Krach.Terms
{
	public class Application : Term
	{
		readonly Function function;
		readonly IEnumerable<Term> parameters;
		
		public Application(Function function, IEnumerable<Term> parameters)
		{
			if (function == null) throw new ArgumentNullException("function");
			if (parameters == null) throw new ArgumentNullException("parameters");
			
			this.function = function;
			this.parameters = parameters;
		}
		
		public override string ToString()
		{
			return string.Format("(({0}) ({1}))", function, parameters.ToStrings().Separate(" ").AggregateString());
		}
		public override double Evaluate()
		{
			return function.Evaluate(parameters.Select(term => term.Evaluate()));
		}
		public override Term GetDerivative(Variable variable)
		{
			return Term.Sum
			(
				from item in Enumerable.Zip(function.Jacobian, parameters, Tuple.Create)
				let functionDerivative = new Application(item.Item1, parameters) 
				let parameterDerivative = item.Item2.GetDerivative(variable)
				select Term.Product(functionDerivative, parameterDerivative)
			);
		}
		public override Term Substitute(Variable variable, Term substitute)
		{
			return new Application(function.Substitute(variable, substitute), parameters.Select(term => term.Substitute(variable, substitute)));
		}
	}
}

