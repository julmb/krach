using System;
using System.Linq;
using System.Collections.Generic;
using Krach.Extensions;

namespace Krach.Terms.LambdaTerms
{
	public class Abstraction : Function
	{
		readonly Variable variable;
		readonly Value term;
		
		public override int DomainDimension { get { return variable.Dimension; } }
		public override int CodomainDimension { get { return term.Dimension; } }
		public Variable Variable { get { return variable; } }
		public Value Term { get { return term; } }
		
		public Abstraction(Variable variable, Value term)
		{
			if (variable == null) throw new ArgumentNullException("variable");
			if (term == null) throw new ArgumentNullException("term");
			
			this.variable = variable;
			this.term = term;
		}
		
		public override string ToString() 
		{
			return string.Format("(λ {0}. {1})", variable, term); 
		}
		public override bool Equals(object obj)
		{
			return obj is Abstraction && Equals(this, (Abstraction)obj);
		}
		public override int GetHashCode()
		{
			return 0;
		}
		public bool Equals(Abstraction other)
		{
			return object.Equals(this, other);
		}
		public override IEnumerable<Variable> GetFreeVariables()
		{
			return term.GetFreeVariables().Except(Enumerables.Create(variable));
		}
		public override Function RenameVariable(Variable oldVariable, Variable newVariable)
		{
			if (variable == oldVariable) return this;
			
			return term.RenameVariable(oldVariable, newVariable).Abstract(variable);
		}		
		public override Function Substitute(Variable variable, Value substitute) 
		{
			Variable newVariable = variable.FindUnusedVariable
			(
				Enumerables.Concatenate
				(
					term.GetFreeVariables(), 
					substitute.GetFreeVariables()
				)
			);
					
			return term.RenameVariable(variable, newVariable).Substitute(variable, substitute).Abstract(newVariable); 
		}
		public override IEnumerable<double> Evaluate(IEnumerable<double> values)
		{
			return term.Substitute(variable, Terms.Term.Constant(values)).Evaluate();
		}
		public override IEnumerable<Function> GetPartialDerivatives() 
		{
			return 
			(
				from partialDerivative in term.GetPartialDerivatives(variable)
				select partialDerivative.Abstract(variable)
			)
			.ToArray();
		}
	
		public static bool operator ==(Abstraction abstraction1, Abstraction abstraction2)
		{
			return object.Equals(abstraction1, abstraction2);
		}
		public static bool operator !=(Abstraction abstraction1, Abstraction abstraction2)
		{
			return !object.Equals(abstraction1, abstraction2);
		}
		
		static bool Equals(Abstraction abstraction1, Abstraction abstraction2)
		{
			if (object.ReferenceEquals(abstraction1, abstraction2)) return true;
			if (object.ReferenceEquals(abstraction1, null) || object.ReferenceEquals(abstraction2, null)) return false;
			
			if (abstraction1.variable.Dimension != abstraction2.variable.Dimension) return false;
			
			int dimension = Items.Equal(abstraction1.variable.Dimension, abstraction2.variable.Dimension);
			Variable combinedVariable = new Variable(dimension, abstraction1.variable.Name + abstraction2.variable.Name);
			Variable unusedVariable = combinedVariable.FindUnusedVariable(Enumerables.Concatenate(abstraction1.term.GetFreeVariables(), abstraction2.term.GetFreeVariables()));
			
			return 
				abstraction1.term.RenameVariable(abstraction1.variable, unusedVariable) ==
				abstraction2.term.RenameVariable(abstraction2.variable, unusedVariable);
		}
	}
}

