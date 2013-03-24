using System;
using System.Linq;
using System.Collections.Generic;
using Krach.Extensions;
using Krach.Calculus.Abstract;
using Krach.Calculus.Terms.Basic;

namespace Krach.Calculus.Terms.Combination
{
	public class Abstraction : FunctionTerm, IEquatable<Abstraction>
	{
		readonly IEnumerable<Variable> variables;
		readonly ValueTerm term;
		
		public IEnumerable<Variable> Variables { get { return variables; } }
		public ValueTerm Term { get { return term; } }
		public override int DomainDimension { get { return variables.Sum(variable => variable.Dimension); } }
		public override int CodomainDimension { get { return term.Dimension; } }
		
		public Abstraction(IEnumerable<Variable> variables, ValueTerm term)
		{
			if (variables == null) throw new ArgumentNullException("variables");
			if (term == null) throw new ArgumentNullException("term");
			
			this.variables = variables.ToArray();
			this.term = term;
		}
		
		public override bool Equals(object obj)
		{
			return obj is Abstraction && Equals(this, (Abstraction)obj);
		}
		public override int GetHashCode()
		{
			return Enumerables.GetSequenceHashCode(variables) ^ term.GetHashCode();
		}
		public bool Equals(Abstraction other)
		{
			return object.Equals(this, other);
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
			if (variables.Contains(variable)) return this;
			
			// TODO: this may rename the bound variables in such a way that they collide with each other
			// TODO: also, think this through once more
			IEnumerable<Variable> newVariables =
			(
				from boundVariable in variables
				select boundVariable.FindUnusedVariable
				(
					Enumerables.Concatenate
					(
						GetFreeVariables(), 
						substitute.GetFreeVariables(), 
						Enumerables.Create(variable)
					)
				)
			)
			.ToArray();
			
			return new Abstraction(newVariables, term.RenameVariables(variables, newVariables).Substitute(variable, substitute)); 
		}
		
		public override int GetSize ()
		{
			return 1 + variables.Sum(variable => variable.GetSize()) + term.GetSize();
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
		public override bool HasCustomApplicationText(IValue parameter)
		{
			return false;
		}
		public override string GetCustomApplicationText(IValue parameter)
		{
			throw new InvalidOperationException();
		}
		public override IEnumerable<double> Evaluate(IEnumerable<double> values)
		{
			IEnumerable<ValueTerm> substitutes = Enumerable.Zip
			(
				variables.Select(variable => variable.Dimension).GetPartialSums(),
				variables.Select(variable => variable.Dimension),
				(start, length) => Terms.Term.Constant(values.Skip(start).Take(length))
			)
			.ToArray();
			
			return term.Substitute(variables, substitutes).Evaluate();
		}
		public override IEnumerable<IFunction> GetDerivatives() 
		{
			return
			(
				from variable in variables
				from derivative in term.GetDerivatives(variable)
				select derivative.Abstract(variables)
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
			
			return
				Enumerable.SequenceEqual(abstraction1.variables, abstraction2.variables) &&
				abstraction1.term == abstraction2.term;
		}
	}
}

