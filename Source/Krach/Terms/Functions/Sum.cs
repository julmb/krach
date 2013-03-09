using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;
using Krach.Terms.LambdaTerms;

namespace Krach.Terms.Functions
{
	public class Sum : BasicFunction
	{	
		public override int DomainDimension { get { return 2; } }
		public override int CodomainDimension { get { return 1; } }

		public override bool Equals(object obj)
		{
			return obj is Sum && Equals(this, (Sum)obj);
		}
		public override int GetHashCode()
		{
			return 0;
		}
		public bool Equals(Sum other)
		{
			return object.Equals(this, other);
		}
		
		public override string ToString()
		{
			return "+";
		}
		public override IEnumerable<double> Evaluate(IEnumerable<double> values)
		{
			yield return values.ElementAt(0) + values.ElementAt(1);
		}
		public override IEnumerable<Function> GetPartialDerivatives()
		{
			Variable x = new Variable(2, "x");
			
			yield return Term.Constant(1).Abstract(x);
			yield return Term.Constant(1).Abstract(x);
		}
		
		public static bool operator ==(Sum sum1, Sum sum2)
		{
			return object.Equals(sum1, sum2);
		}
		public static bool operator !=(Sum sum1, Sum sum2)
		{
			return !object.Equals(sum1, sum2);
		}
		
		static bool Equals(Sum sum1, Sum sum2)
		{
			if (object.ReferenceEquals(sum1, sum2)) return true;
			if (object.ReferenceEquals(sum1, null) || object.ReferenceEquals(sum2, null)) return false;
			
			return true;
		}
	}
}

