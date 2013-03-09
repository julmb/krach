using System;
using System.Collections.Generic;

namespace Krach.Terms.Functions
{
	public class Constant : BasicFunction, IEquatable<Constant>
	{
		readonly double value;
		
		public override int DomainDimension { get { return 0; } }
		public override int CodomainDimension { get { return 1; } }
		public double Value { get { return value; } }
		
		public Constant(double value)
		{
			this.value = value;
		}

		public override string ToString()
		{
			return value.ToString();
		}
		public override bool Equals(object obj)
		{
			return obj is Constant && Equals(this, (Constant)obj);
		}
		public override int GetHashCode()
		{
			return 0;
		}
		public bool Equals(Constant other)
		{
			return object.Equals(this, other);
		}
		public override IEnumerable<double> Evaluate(IEnumerable<double> values)
		{
			yield return value;
		}
		public override IEnumerable<Function> GetPartialDerivatives()
		{
			yield break;
		}
		
		public static bool operator ==(Constant constant1, Constant constant2)
		{
			return object.Equals(constant1, constant2);
		}
		public static bool operator !=(Constant constant1, Constant constant2)
		{
			return !object.Equals(constant1, constant2);
		}
		
		static bool Equals(Constant constant1, Constant constant2)
		{
			if (object.ReferenceEquals(constant1, constant2)) return true;
			if (object.ReferenceEquals(constant1, null) || object.ReferenceEquals(constant2, null)) return false;
			
			return constant1.value == constant2.value;
		}
	}
}

