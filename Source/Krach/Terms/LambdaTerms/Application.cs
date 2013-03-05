using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;

namespace Krach.Terms.LambdaTerms
{
	public class Application : ValueTerm
	{
		readonly FunctionTerm function;
		readonly IEnumerable<ValueTerm> parameters;
		
		public Application(FunctionTerm function, IEnumerable<ValueTerm> parameters)
		{
			if (function == null) throw new ArgumentNullException("function");
			if (parameters == null) throw new ArgumentNullException("parameters");
			
			this.function = function;
			this.parameters = parameters;
		}
		
		public override ValueTerm Substitute(Variable variable, ValueTerm substitute)
		{
			return new Application(function.Substitute(variable, substitute), parameters.Select(term => term.Substitute(variable, substitute)));
		}
		public override string GetText()
		{
			return function.GetText(parameters.Select(parameter => parameter.GetText()));
		}
		public override double Evaluate()
		{
			return function.Evaluate(parameters.Select(term => term.Evaluate()));
		}
		public override ValueTerm GetDerivative(Variable variable)
		{
			return Term.Sum
			(
				from item in Enumerable.Zip(function.GetJacobian(), parameters, Tuple.Create)
				let partialDerivative = new Application(item.Item1, parameters) 
				let parameterDerivative = item.Item2.GetDerivative(variable)
				select Term.Product(partialDerivative, parameterDerivative)
			);
		}
	}
}

