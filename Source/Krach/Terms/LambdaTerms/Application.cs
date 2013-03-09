using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;

namespace Krach.Terms.LambdaTerms
{
	public class Application : Value
	{
		readonly Function function;
		readonly Value parameter;
		
		public override int Dimension { get { return function.CodomainDimension; } }
		public Function Function { get { return function; } }
		public Value Parameter { get { return parameter; } }
		
		public Application(Function function, Value parameter)
		{
			if (function == null) throw new ArgumentNullException("function");
			if (parameter == null) throw new ArgumentNullException("parameter");
			
			this.function = function;
			this.parameter = parameter;
		}
		
		public override string ToString()
		{
			return string.Format("({0} ! {1})", function, parameter);
		}
		public override bool Equals(object obj)
		{
			return obj is Application && Equals(this, (Application)obj);
		}
		public override int GetHashCode()
		{
			return 0;
		}
		public bool Equals(Application other)
		{
			return object.Equals(this, other);
		}
		public override IEnumerable<Variable> GetFreeVariables()
		{
			return Enumerables.Concatenate(function.GetFreeVariables(), parameter.GetFreeVariables());
		}
		public override Value RenameVariable(Variable oldVariable, Variable newVariable)
		{
			return
				function.RenameVariable(oldVariable, newVariable)
				.Apply(parameter.RenameVariable(oldVariable, newVariable));
		}
		public override Value Substitute(Variable variable, Value substitute)
		{
			return function.Substitute(variable, substitute).Apply(parameter.Substitute(variable, substitute));
		}
		public override IEnumerable<double> Evaluate()
		{
			return function.Evaluate(parameter.Evaluate());
		}
		public override IEnumerable<Value> GetPartialDerivatives(Variable variable)
		{
			return 
			(
				from parameterPartialDerivative in parameter.GetPartialDerivatives(variable)
				select Term.Vector
				(
					from functionPartialDerivative in function.GetPartialDerivatives()
					select Term.Product(functionPartialDerivative.Apply(parameter), parameterPartialDerivative)
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
			
			return 
				application1.function == application2.function && 
				application1.parameter == application2.parameter;
		}
	}
}

