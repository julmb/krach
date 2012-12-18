using System;

namespace Krach.Calculus
{
	public class Variable : Term, IEquatable<Variable>
	{
		readonly string name;

		public string Name { get { return name; } }

		public Variable(string name)
		{
			if (name == null) throw new ArgumentNullException("name");

			this.name = name;
		}

		public override double Evaluate()
		{
			throw new InvalidOperationException(string.Format("Cannot evaluate variable '{0}'.", name));
		}
		public override Term Substitute(Variable variable, Term term)
		{
			return variable == this ? term : this;
		}
		public override Term GetDerivative(Variable variable)
		{
			return new Constant(variable == this ? 1 : 0);
		}

		public override bool Equals(object obj)
		{
			return obj is Variable && this == (Variable)obj;
		}
		public override int GetHashCode()
		{
			return name.GetHashCode();
		}
		public bool Equals(Variable other)
		{
			return this == other;
		}

		public static bool operator ==(Variable variable1, Variable variable2)
		{
			if (object.ReferenceEquals(variable1, variable2)) return true;
			if (object.ReferenceEquals(variable1, null) || object.ReferenceEquals(variable2, null)) return false;
			
			return variable1.name == variable2.name;
		}
		public static bool operator !=(Variable variable1, Variable variable2)
		{
			if (object.ReferenceEquals(variable1, variable2)) return false;
			if (object.ReferenceEquals(variable1, null) || object.ReferenceEquals(variable2, null)) return true;
			
			return variable1.name != variable2.name;
		}
	}
}

