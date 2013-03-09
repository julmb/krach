using System;
using Krach.Extensions;
using System.Linq;
using System.Collections.Generic;

namespace Krach.Terms.LambdaTerms
{
	public class Selection : Value
	{
		readonly Value value;
		readonly int index;
		
		public override int Dimension { get { return 1; } }
		public Value Value { get { return value; } }
		public int Index { get { return index; } }
		
		public Selection(Value value, int index)
		{
			if (value == null) throw new ArgumentNullException("value");
			if (index < 0 || index >= value.Dimension) throw new ArgumentOutOfRangeException("index");
			
			this.value = value;
			this.index = index;
		}
		
		public override string ToString()
		{
			return string.Format("{0}{1}", value, index.ToSubscriptString());
		}
		public override bool Equals(object obj)
		{
			return obj is Selection && Equals(this, (Selection)obj);
		}
		public override int GetHashCode()
		{
			return 0;
		}
		public bool Equals(Selection other)
		{
			return object.Equals(this, other);
		}
		public override IEnumerable<Variable> GetFreeVariables()
		{
			return value.GetFreeVariables();
		}
		public override Value RenameVariable(Variable oldVariable, Variable newVariable)
		{
			return value.RenameVariable(oldVariable, newVariable).Select(index);
		}
		public override Value Substitute(Variable variable, Value term)
		{
			return value.Substitute(variable, term).Select(index);
		}
		public override IEnumerable<double> Evaluate()
		{
			yield return value.Evaluate().ElementAt(index);
		}
		public override IEnumerable<Value> GetPartialDerivatives(Variable variable)
		{
			return 
			(
				from partialDerivative in value.GetPartialDerivatives(variable)
				select partialDerivative.Select(index)
			)
			.ToArray();
		}
		
		public static bool operator ==(Selection selection1, Selection selection2)
		{
			return object.Equals(selection1, selection2);
		}
		public static bool operator !=(Selection selection1, Selection selection2)
		{
			return !object.Equals(selection1, selection2);
		}
		
		static bool Equals(Selection selection1, Selection selection2) 
		{
			if (object.ReferenceEquals(selection1, selection2)) return true;
			if (object.ReferenceEquals(selection1, null) || object.ReferenceEquals(selection2, null)) return false;
			
			return selection1.value == selection2.value && selection1.index == selection2.index;
		}
	}
}

