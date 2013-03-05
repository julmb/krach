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
		
		public IEnumerable<Variable> Variables { get { return variables; } }
		public ValueTerm Term { get { return term; } }
		
		public Abstraction(IEnumerable<Variable> variables, ValueTerm term)
		{
			if (variables == null) throw new ArgumentNullException("variables");
			if (term == null) throw new ArgumentNullException("term");
			
			this.variables = variables;
			this.term = term;
		}
		
		public override bool Equals(object obj)
		{
			return obj is Abstraction && Equals(this, (Abstraction)obj);
		}
		public override int GetHashCode()
		{
			IEnumerable<Variable> newVariables = 
				from index in Enumerable.Range(0, variables.Count())
				select new Variable(index.ToString());
				
			return term.RenameVariables(variables, newVariables).GetHashCode();
		}
		public bool Equals(Abstraction other)
		{
			return object.Equals(this, other);
		}
		public override string GetText() 
		{
			return string.Format
			(
				"(λ {0}. {1})", 
				variables.Select(variable => variable.GetText()).Separate(" ").AggregateString(), 
				term.GetText()
			); 
		}
		public override IEnumerable<Variable> GetFreeVariables()
		{
			return term.GetFreeVariables().Except(variables);
		}
		public override FunctionTerm RenameVariable(Variable oldVariable, Variable newVariable)
		{
			if (variables.Contains(oldVariable)) return this;
			
			return new Abstraction(variables, term.RenameVariable(oldVariable, newVariable));
		}		
		public override FunctionTerm Substitute(Variable variable, ValueTerm substitute) 
		{
			IEnumerable<Variable> newVariables =
				from boundVariable in variables
				select boundVariable.FindUnusedVariable
				(
					Enumerables.Concatenate
					(
						GetFreeVariables(), 
						substitute.GetFreeVariables(), 
						Enumerables.Create(variable)
					)
				);
					
			return new Abstraction(newVariables, term.RenameVariables(variables, newVariables).Substitute(variable, substitute)); 
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
			
			if (abstraction1.variables.Count() != abstraction2.variables.Count()) return false;
			
			IEnumerable<Variable> variables = 
				from item in Enumerable.Zip(abstraction1.variables, abstraction2.variables, Tuple.Create)
				let combinedVariable = new Variable(item.Item1.Name + item.Item2.Name)
				select combinedVariable.FindUnusedVariable(Enumerables.Concatenate(abstraction1.term.GetFreeVariables(), abstraction2.term.GetFreeVariables()));
				
			return 
				abstraction1.term.RenameVariables(abstraction1.variables, variables) ==
				abstraction2.term.RenameVariables(abstraction2.variables, variables);
		}
	}
}

