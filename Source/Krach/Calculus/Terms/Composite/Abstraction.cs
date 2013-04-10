using System;
using System.Linq;
using System.Collections.Generic;
using Krach.Extensions;
using Krach.Calculus.Terms.Basic;
using Krach.Calculus.Terms.Notation;

namespace Krach.Calculus.Terms.Composite
{
	public class Abstraction : FunctionTerm, IEquatable<Abstraction>
	{
		readonly IEnumerable<Variable> variables;
		readonly ValueTerm term;
		
		public IEnumerable<Variable> Variables { get { return variables; } }
		public ValueTerm Term { get { return term; } }
        public override Syntax Syntax { get { return Syntax.Abstraction(this); } }
		public override int DomainDimension { get { return variables.Sum(variable => variable.Dimension); } }
		public override int CodomainDimension { get { return term.Dimension; } }
		
		public Abstraction(IEnumerable<Variable> variables, ValueTerm term)
		{
			if (variables == null) throw new ArgumentNullException("variables");
			if (term == null) throw new ArgumentNullException("term");
			if (!variables.IsDistinct()) throw new ArgumentException("The given variables are not distinct.");
			
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
		public override FunctionTerm Substitute(Variable variable, ValueTerm substitute) 
		{
			if (variables.Contains(variable)) return this;
			
			IEnumerable<Variable> newVariables = FindUnusedVariables
			(
				variables,
				Enumerables.Concatenate
				(
					GetFreeVariables(),
					Enumerables.Create(variable),
					substitute.GetFreeVariables()
				)
			);
			ValueTerm newTerm = term.Substitute(variables, newVariables);
			
			return new Abstraction(newVariables, newTerm.Substitute(variable, substitute)); 
		}

		public override IEnumerable<double> Evaluate(IEnumerable<double> values)
		{
			IEnumerable<ValueTerm> substitutes = Enumerable.Zip
			(
				variables.Select(variable => variable.Dimension).GetPartialSums(),
				variables.Select(variable => variable.Dimension),
				(start, length) => Calculus.Term.Constant(values.Skip(start).Take(length))
			)
			.ToArray();
			
			return term.Substitute(variables, substitutes).Evaluate();
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

