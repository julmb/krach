using System;
using System.Collections.Generic;
using Krach.Extensions;
using Krach.Calculus.Terms.Notation;

namespace Krach.Calculus.Terms.Basic.Definitions
{
	public class ValueDefinition : BasicValueTerm, IEquatable<ValueDefinition>
	{
		readonly ValueTerm value;
		readonly ValueSyntax valueSyntax;

		public ValueTerm Value { get { return value; } }
		public override ValueSyntax ValueSyntax { get { return valueSyntax; } }
		public override int Dimension { get { return value.Dimension; } }
		
		public ValueDefinition(ValueTerm value, ValueSyntax valueSyntax)
		{
			if (value == null) throw new ArgumentNullException("value");
			if (valueSyntax == null) throw new ArgumentNullException("valueSyntax");

			this.value = value;
			this.valueSyntax = valueSyntax;
		}

		public override bool Equals(object obj)
		{
			return obj is ValueDefinition && Equals(this, (ValueDefinition)obj);
		}
		public override int GetHashCode()
		{
			return value.GetHashCode();
		}
		public bool Equals(ValueDefinition other)
		{
			return object.Equals(this, other);
		}

		public override IEnumerable<double> Evaluate()
		{
			throw new InvalidOperationException("Cannot evaluate definition.");
		}
		
		public static bool operator ==(ValueDefinition value1, ValueDefinition value2)
		{
			return object.Equals(value1, value2);
		}
		public static bool operator !=(ValueDefinition value1, ValueDefinition value2)
		{
			return !object.Equals(value1, value2);
		}
		
		static bool Equals(ValueDefinition value1, ValueDefinition value2) 
		{
			if (object.ReferenceEquals(value1, value2)) return true;
			if (object.ReferenceEquals(value1, null) || object.ReferenceEquals(value2, null)) return false;
			
			return value1.value == value2.value;
		}
	}
}

