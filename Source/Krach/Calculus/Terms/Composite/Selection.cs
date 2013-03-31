using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;
using Krach.Calculus.Terms.Notation;

namespace Krach.Calculus.Terms.Composite
{
	public class Selection : ValueTerm, IEquatable<Selection>
	{
		readonly ValueTerm term;
		readonly int index;
		
		public ValueTerm Term { get { return term; } }
		public int Index { get { return index; } }
        public override Syntax Syntax { get { return Syntax.Selection(this); } }
		public override int Dimension { get { return 1; } }
		
		public Selection(ValueTerm term, int index)
		{
			if (term == null) throw new ArgumentNullException("term");
			if (index < 0 || index >= term.Dimension) throw new ArgumentOutOfRangeException("index");
			
			this.term = term;
			this.index = index;
		}
		
		public override bool Equals(object obj)
		{
			return obj is Selection && Equals(this, (Selection)obj);
		}
		public override int GetHashCode()
		{
			return term.GetHashCode() ^ index.GetHashCode();
		}
		public bool Equals(Selection other)
		{
			return object.Equals(this, other);
		}

		public override IEnumerable<Variable> GetFreeVariables()
		{
			return term.GetFreeVariables();
		}
		public override ValueTerm Substitute(Variable variable, ValueTerm substitute)
		{
			return new Selection(term.Substitute(variable, substitute), index);
		}

		public override IEnumerable<double> Evaluate()
		{
			yield return term.Evaluate().ElementAt(index);
		}
		public override IEnumerable<ValueTerm> GetDerivatives(Variable variable)
		{
			return
			(
				from derivative in term.GetDerivatives(variable)
				select derivative.Select(index)
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
			
			return selection1.term == selection2.term && selection1.index == selection2.index;
		}
	}
}

