using System;
using System.Collections.Generic;
using Krach.Calculus.Abstract;
using Krach.Calculus.Terms.Combination;
using System.Linq;

namespace Krach.Calculus.Terms.Basic
{
	public class BasicValue : ValueTerm, IEquatable<BasicValue>
	{		
		readonly IValue value;
		
		public IValue Value { get { return value; } }
		public override int Dimension { get { return value.Dimension; } }
		
		public BasicValue(IValue value)
		{
			if (value == null) throw new ArgumentNullException("value");
			if (value is ValueTerm) throw new ArgumentException("Parameter 'value' already is a term.");
			
			this.value = value;
		}

		public override bool Equals(object obj)
		{
			return obj is BasicValue && Equals(this, (BasicValue)obj);
		}
		public override int GetHashCode()
		{
			return value.GetHashCode();
		}
		public bool Equals(BasicValue other)
		{
			return object.Equals(this, other);
		}
		
		public override IEnumerable<Variable> GetFreeVariables()
		{
			yield break;
		}
		public override ValueTerm RenameVariable(Variable oldVariable, Variable newVariable)
		{
			return this;
		}
		public override ValueTerm Substitute(Variable variable, ValueTerm substitute)
		{
			return this;
		}
		
		public override int GetSize()
		{
			return 1;
		}
		public override string GetText()
		{
			return value.GetText();
		}
		public override IEnumerable<double> Evaluate()
		{
			return value.Evaluate();
		}
		public override IEnumerable<ValueTerm> GetDerivatives(Variable variable)
		{			
			return
			(
				from variableIndex in Enumerable.Range(0, variable.Dimension)
				select Term.Vector(Enumerable.Repeat(Term.Constant(0), value.Dimension))
			)
			.ToArray();
		}
		
		public static bool operator ==(BasicValue term1, BasicValue term2)
		{
			return object.Equals(term1, term2);
		}
		public static bool operator !=(BasicValue term1, BasicValue term2)
		{
			return !object.Equals(term1, term2);
		}
		
		static bool Equals(BasicValue value1, BasicValue value2)
		{
			if (object.ReferenceEquals(value1, value2)) return true;
			if (object.ReferenceEquals(value1, null) || object.ReferenceEquals(value2, null)) return false;
			
			return object.Equals(value1.value, value2.value);
		}
	}
}

