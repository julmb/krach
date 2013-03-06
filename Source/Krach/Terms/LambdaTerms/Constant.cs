using System;
using System.Collections.Generic;

namespace Krach.Terms.LambdaTerms
{
	public class Constant : Value, IEquatable<Constant>
	{
		readonly double value;
		
		public double Value { get { return value; } }
		
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
		public override IEnumerable<Variable> GetFreeVariables()
		{
			yield break;
		}
		public override Value RenameVariable(Variable oldVariable, Variable newVariable)
		{
			return this;
		}
		public override Value Substitute(Variable variable, Value substitute) 
		{
			return this;
		}
		public override double Evaluate()
		{
			return value;
		}
		public override Value GetDerivative(Variable variable)
		{
			return Term.Constant(0);
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

