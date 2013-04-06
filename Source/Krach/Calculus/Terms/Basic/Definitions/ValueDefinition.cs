using System;
using System.Collections.Generic;
using Krach.Extensions;
using Krach.Calculus.Terms.Notation;
using Krach.Calculus.Terms.Composite;
using System.Linq;
using Krach.Calculus.Terms.Basic;

namespace Krach.Calculus.Terms.Basic.Definitions
{
	public class ValueDefinition : BasicValueTerm, IEquatable<ValueDefinition>
	{
        readonly string name;
		readonly ValueTerm value;
        readonly Syntax syntax;

        public string Name { get { return name; } }
		public ValueTerm Value { get { return value; } }
        public override Syntax Syntax { get { return syntax; } }
		public override int Dimension { get { return value.Dimension; } }

        public ValueDefinition(string name, ValueTerm value, Syntax syntax)
		{
            if (name == null) throw new ArgumentNullException("name");
			if (value == null) throw new ArgumentNullException("value");
			if (value.GetFreeVariables().Any()) throw new ArgumentException("cannot create definition containing free variables.");
            if (syntax == null) throw new ArgumentNullException("syntax");

            this.name = name;
			this.value = value;
            this.syntax = syntax;
		}

		public override bool Equals(object obj)
		{
			return obj is ValueDefinition && Equals(this, (ValueDefinition)obj);
		}
		public override int GetHashCode()
		{
			return name.GetHashCode();
		}
		public bool Equals(ValueDefinition other)
		{
			return object.Equals(this, other);
		}

		public override IEnumerable<double> Evaluate()
		{
            return value.Evaluate();
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
			
			return value1.name == value2.name;
		}
	}
}

