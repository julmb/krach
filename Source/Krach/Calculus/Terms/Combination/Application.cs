using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;
using Krach.Calculus.Terms.Basic;

namespace Krach.Calculus.Terms.Combination
{
	public class Application : ValueTerm, IEquatable<Application>
	{
		readonly FunctionTerm function;
		readonly ValueTerm parameter;
		
		public FunctionTerm Function { get { return function; } }
		public ValueTerm Parameter { get { return parameter; } }
		public override int Dimension { get { return function.CodomainDimension; } }
		
		public Application(FunctionTerm function, ValueTerm parameter)
		{
			if (function == null) throw new ArgumentNullException("function");
			if (parameter == null) throw new ArgumentNullException("parameter");
			if (parameter.Dimension != function.DomainDimension)
				throw new ArgumentException
				(
					string.Format
					(
						"Dimension mismatch applying function '{0}' to parameter '{1}'.",
						function.GetText(),
						parameter.GetText()
					)
				);
			
			this.function = function;
			this.parameter = parameter;
		}
		
		public override bool Equals(object obj)
		{
			return obj is Application && Equals(this, (Application)obj);
		}
		public override int GetHashCode()
		{
			return function.GetHashCode() ^ parameter.GetHashCode();
		}
		public bool Equals(Application other)
		{
			return object.Equals(this, other);
		}
		
		public override IEnumerable<Variable> GetFreeVariables()
		{
			return Enumerables.Concatenate
			(
				from variable in function.GetFreeVariables()
				select variable,
				from variable in parameter.GetFreeVariables()
				select variable
			)
			.ToArray();
		}
		public override ValueTerm RenameVariable(Variable oldVariable, Variable newVariable)
		{
			return new Application
			(
				function.RenameVariable(oldVariable, newVariable),
				parameter.RenameVariable(oldVariable, newVariable)
			);
		}
		public override ValueTerm Substitute(Variable variable, ValueTerm substitute)
		{
			return new Application
			(
				function.Substitute(variable, substitute),
				parameter.Substitute(variable, substitute)
			);
		}
		
		public override int GetSize ()
		{
			return 1 + function.GetSize() + parameter.GetSize();
		}
		public override string GetText()
		{			
			if (function.HasCustomApplicationText(parameter)) return function.GetCustomApplicationText(parameter);
			
			return string.Format("({0} {1})", function.GetText(), parameter.GetText());
		}
		public override IEnumerable<double> Evaluate()
		{
			return function.Evaluate(parameter.Evaluate());
		}
		public override IEnumerable<ValueTerm> GetDerivatives(Variable variable)
		{
			IEnumerable<ValueTerm> functionDerivatives =
			(
				from derivative in function.GetDerivatives()
				select derivative.Apply(parameter)
			)
			.ToArray();
			IEnumerable<ValueTerm> flippedFunctionDerivatives =
			(
				from index in Enumerable.Range(0, function.CodomainDimension)
				select Term.Vector
				(
					from derivative in functionDerivatives
					select derivative.Select(index)
				)
			)
			.ToArray();
			IEnumerable<ValueTerm> parameterDerivatives = parameter.GetDerivatives(variable);

			return
			(
				from parameterDerivative in parameterDerivatives
				select Term.Vector
				(
					from functionDerivative in flippedFunctionDerivatives
					select Term.Product(functionDerivative, parameterDerivative)
				)
			)
			.ToArray();
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
			
			return application1.function == application2.function && application1.parameter == application2.parameter;
		}
	}
}

