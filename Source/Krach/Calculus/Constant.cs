using System;
using System.Collections.Generic;
using Krach.Calculus.Abstract;
using Krach.Calculus.Basic;

namespace Krach.Calculus
{
	public class Constant : Value, IEquatable<Constant>
	{
		readonly double value;
		
		public double Value { get { return value; } }
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
		
		public override string GetText()
		{
			return value.ToString();
		}
		public override IEnumerable<double> Evaluate()
		{
			yield return value;
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

