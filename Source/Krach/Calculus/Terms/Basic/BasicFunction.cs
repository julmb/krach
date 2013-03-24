using System;
using System.Collections.Generic;
using Krach.Calculus.Abstract;
using Krach.Calculus.Terms.Combination;

namespace Krach.Calculus.Terms.Basic
{
	public class BasicFunction : FunctionTerm, IEquatable<BasicFunction>
	{
		readonly IFunction function;
		
		public IFunction Function { get { return function; } }
		public override int DomainDimension { get { return function.DomainDimension; } }
		public override int CodomainDimension { get { return function.CodomainDimension; } }
		
		public BasicFunction(IFunction function)
		{
			if (function == null) throw new ArgumentNullException("function");
			if (function is FunctionTerm) throw new ArgumentException("Parameter 'function' already is a term.");
			
			this.function = function;
		}
		
		public override bool Equals(object obj)
		{
			return obj is BasicFunction && Equals(this, (BasicFunction)obj);
		}
		public override int GetHashCode()
		{
			return function.GetHashCode();
		}
		public bool Equals(BasicFunction other)
		{
			return object.Equals(this, other);
		}
		
		public override IEnumerable<Variable> GetFreeVariables()
		{
			yield break;
		}
		public override FunctionTerm RenameVariable(Variable oldVariable, Variable newVariable)
		{
			return this;
		}
		public override FunctionTerm Substitute(Variable variable, ValueTerm substitute)
		{
			return this;
		}
		
		public override int GetSize()
		{
			return 1;
		}
		public override string GetText()
		{
			return function.GetText();
		}
		public override bool HasCustomApplicationText(IValue parameter)
		{
			return function.HasCustomApplicationText(parameter);
		}
		public override string GetCustomApplicationText(IValue parameter)
		{
			return function.GetCustomApplicationText(parameter);
		}
		public override IEnumerable<double> Evaluate(IEnumerable<double> parameters)
		{
			return function.Evaluate(parameters);
		}
		public override IEnumerable<IFunction> GetDerivatives()
		{
			return function.GetDerivatives();
		}
		
		public static bool operator ==(BasicFunction term1, BasicFunction term2)
		{
			return object.Equals(term1, term2);
		}
		public static bool operator !=(BasicFunction term1, BasicFunction term2)
		{
			return !object.Equals(term1, term2);
		}
		
		static bool Equals(BasicFunction function1, BasicFunction function2)
		{
			if (object.ReferenceEquals(function1, function2)) return true;
			if (object.ReferenceEquals(function1, null) || object.ReferenceEquals(function2, null)) return false;
			
			return object.Equals(function1.function, function2.function);
		}
	}
}

