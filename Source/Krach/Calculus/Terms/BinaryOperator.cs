using System;
using System.Collections.Generic;
using Krach.Calculus.Terms.Combination;
using System.Linq;
using Krach.Extensions;

namespace Krach.Calculus.Terms
{
	public abstract class BinaryOperator : BasicFunctionTerm, IEquatable<BinaryOperator>
	{		
		public override bool Equals(object obj)
		{
			throw new InvalidOperationException();
		}
		public override int GetHashCode()
		{
			throw new InvalidOperationException();
		}
		public bool Equals(BinaryOperator other)
		{
			return object.Equals(this, other);
		}

		public override bool HasCustomApplicationText(ValueTerm parameter)
		{
			IEnumerable<ValueTerm> parameters = parameter is Vector ? ((Vector)parameter).Terms : Enumerables.Create(parameter);
			
			return parameters.Count() == 2;
		}
		public override string GetCustomApplicationText(ValueTerm parameter)
		{
			IEnumerable<ValueTerm> parameters = parameter is Vector ? ((Vector)parameter).Terms : Enumerables.Create(parameter);
			
			return string.Format("({0} {1} {2})", parameters.ElementAt(0).GetText(), GetText(), parameters.ElementAt(1).GetText());
		}
		
		public static bool operator ==(BinaryOperator function1, BinaryOperator function2)
		{
			return object.Equals(function1, function2);
		}
		public static bool operator !=(BinaryOperator function1, BinaryOperator function2)
		{
			return !object.Equals(function1, function2);
		}
	}
}

