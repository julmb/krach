using System;
using System.Collections.Generic;
using Krach.Extensions;
using Krach.Calculus.Terms.Notation;

namespace Krach.Calculus.Terms.Basic.Atoms
{
	public class Constant : BasicValueTerm, IEquatable<Constant>
	{
		readonly double value;
		
		public double Value { get { return value; } }
        public override Syntax Syntax { get { return new BasicSyntax(value.ToString()); } }
		public override int Dimension { get { return 1; } }
		
		public Constant(double value)
		{
			this.value = value;
		}

		public override bool Equals(object obj)
		{
			return obj is Constant && Equals(this, (Constant)obj);
		}
		public override int GetHashCode()
		{
			return value.GetHashCode();
		}
		public bool Equals(Constant other)
		{
			return object.Equals(this, other);
		}

		public override IEnumerable<double> Evaluate()
		{
			return Enumerables.Create(value);
		}
		
		public static bool operator ==(Constant value1, Constant value2)
		{
			return object.Equals(value1, value2);
		}
		public static bool operator !=(Constant value1, Constant value2)
		{
			return !object.Equals(value1, value2);
		}
		
		static bool Equals(Constant value1, Constant value2) 
		{
			if (object.ReferenceEquals(value1, value2)) return true;
			if (object.ReferenceEquals(value1, null) || object.ReferenceEquals(value2, null)) return false;
			
			return value1.value == value2.value;
		}
	}
}

