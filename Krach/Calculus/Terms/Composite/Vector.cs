using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;
using Krach.Calculus.Terms.Notation;

namespace Krach.Calculus.Terms.Composite
{
	public class Vector : ValueTerm, IEquatable<Vector>
	{
		readonly IEnumerable<ValueTerm> terms;
		
		public IEnumerable<ValueTerm> Terms { get { return terms; } }
        public override Syntax Syntax { get { return Syntax.Vector(this); } }
		public override int Dimension { get { return terms.Sum(term => term.Dimension); } }
		
		public Vector(IEnumerable<ValueTerm> terms)
		{
			if (terms == null) throw new ArgumentNullException("terms");
			
			this.terms = terms.ToArray();
		}
		
		public override bool Equals(object obj)
		{
			return obj is Vector && Equals(this, (Vector)obj);
		}
		public override int GetHashCode()
		{
			return Enumerables.GetSequenceHashCode(terms);
		}
		public bool Equals(Vector other)
		{
			return object.Equals(this, other);
		}

		public override IEnumerable<Variable> GetFreeVariables()
		{
			return
			(
				from term in terms
				from variable in term.GetFreeVariables()
				select variable
			);
		}
		public override ValueTerm Substitute(Variable variable, ValueTerm substitute)
		{
			return new Vector
			(
				from term in terms
				select term.Substitute(variable, substitute)
			);
		}

		public override IEnumerable<double> Evaluate()
		{
			return
			(
				from term in terms
				from value in term.Evaluate()
				select value
			)
			.ToArray();
		}
		
		public static bool operator ==(Vector vector1, Vector vector2)
		{
			return object.Equals(vector1, vector2);
		}
		public static bool operator !=(Vector vector1, Vector vector2)
		{
			return !object.Equals(vector1, vector2);
		}
		
		static bool Equals(Vector vector1, Vector vector2)
		{
			if (object.ReferenceEquals(vector1, vector2)) return true;
			if (object.ReferenceEquals(vector1, null) || object.ReferenceEquals(vector2, null)) return false;
			
			return Enumerable.SequenceEqual(vector1.terms, vector2.terms);
		}
	}
}

