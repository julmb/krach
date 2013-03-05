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
		
		public FunctionTerm Function { get { return function; } }
		public IEnumerable<ValueTerm> Parameters { get { return parameters; } }
		
		public Application(FunctionTerm function, IEnumerable<ValueTerm> parameters)
		{
			if (function == null) throw new ArgumentNullException("function");
			if (parameters == null) throw new ArgumentNullException("parameters");
			
			this.function = function;
			this.parameters = parameters;
		}
		
		public override bool Equals(object obj)
		{
			return obj is Application && Equals(this, (Application)obj);
		}
		public override int GetHashCode()
		{
			return function.GetHashCode() ^ Enumerables.GetSequenceHashCode(parameters);
		}
		public bool Equals(Application other)
		{
			return object.Equals(this, other);
		}
		public override string GetText()
		{
			return function.GetText(parameters.Select(parameter => parameter.GetText()));
		}
		public override IEnumerable<Variable> GetFreeVariables()
		{
			return Enumerables.Concatenate
			(
				function.GetFreeVariables(), 
				Enumerables.Concatenate(parameters.Select(parameter => parameter.GetFreeVariables()))
			);
		}
		public override ValueTerm RenameVariable(Variable oldVariable, Variable newVariable)
		{
			return
				function.RenameVariable(oldVariable, newVariable)
				.Apply(parameters.Select(parameter => parameter.RenameVariable(oldVariable, newVariable)));
		}
		public override ValueTerm Substitute(Variable variable, ValueTerm substitute)
		{
			return function.Substitute(variable, substitute).Apply(parameters.Select(term => term.Substitute(variable, substitute)));
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
		
		public static bool operator ==(Application application1, Application application2)
		{
			return object.Equals(application1, application2);
		}
		public static bool operator !=(Application application1, Application application2)
		{
			return !object.Equals(application1, application2);
		}
		
		static bool Equals(Application application1, Application application2)
		{
			if (object.ReferenceEquals(application1, application2)) return true;
			if (object.ReferenceEquals(application1, null) || object.ReferenceEquals(application2, null)) return false;
			
			return 
				application1.function == application2.function && 
				Enumerable.Zip(application1.parameters, application2.parameters, Tuple.Create)
				.All(item => item.Item1 == item.Item2);
		}
	}
}

