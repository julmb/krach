using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;
using Krach.Calculus.Abstract;
using Krach.Calculus.Terms.Combination;
using Krach.Calculus.Terms;
using Krach.Calculus.Basic;

namespace Krach.Calculus
{
	public class Exponentiation : Function, IEquatable<Exponentiation>
	{
		readonly double exponent;
			
		public double Exponent { get { return exponent; } }
		public override int DomainDimension { get { return 1; } }
		public override int CodomainDimension { get { return 1; } }

		public Exponentiation(double exponent) 
		{
			this.exponent = exponent;
		}

		public override bool Equals(object obj)
		{
			return obj is Exponentiation && Equals(this, (Exponentiation)obj);
		}
		public override int GetHashCode()
		{
			return exponent.GetHashCode();
		}
		public bool Equals(Exponentiation other)
		{
			return object.Equals(this, other);
		}
		
		public override string GetText()
		{
			return string.Format("^{0}", exponent);
		}
		public override bool HasCustomApplicationText(IValue parameter)
		{
			return true;
		}
		public override string GetCustomApplicationText(IValue parameter)
		{			
			if (exponent.ToString().IsSubSuperScriptCompatible())
				return string.Format("{0}{1}", parameter.GetText(), exponent.ToString().ToSuperscript());
			
			return string.Format("({0} ^ {1})", parameter.GetText(), exponent);
		}
		public override IEnumerable<double> Evaluate(IEnumerable<double> values)
		{
			yield return values.ElementAt(0).Exponentiate(exponent);
		}
		public override IEnumerable<IFunction> GetDerivatives()
		{	
			Variable x = new Variable(1, "x");
			
			yield return Term.Product(Term.Constant(exponent), x.Exponentiate(exponent - 1)).Abstract(x);
		}
		
		public static bool operator ==(Exponentiation function1, Exponentiation function2)
		{
			return object.Equals(function1, function2);
		}
		public static bool operator !=(Exponentiation function1, Exponentiation function2)
		{
			return !object.Equals(function1, function2);
		}
		
		static bool Equals(Exponentiation function1, Exponentiation function2) 
		{
			if (object.ReferenceEquals(function1, function2)) return true;
			if (object.ReferenceEquals(function1, null) || object.ReferenceEquals(function2, null)) return false;
			
			return function1.exponent == function2.exponent;
		}
	}
}

