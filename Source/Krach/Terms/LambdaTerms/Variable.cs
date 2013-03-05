using System;

namespace Krach.Terms.LambdaTerms
{
	public class Variable : ValueTerm, IEquatable<Variable>
	{
		readonly string name;

		public Variable(string name)
		{
			if (name == null) throw new ArgumentNullException("name");

			this.name = name;
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
		public override ValueTerm Substitute(Variable variable, ValueTerm term)
		{
			return variable == this ? term : this;
		}
		public override string GetText()
		{
			return name;
		}
		public override double Evaluate()
		{
			throw new InvalidOperationException(string.Format("Cannot evaluate variable '{0}'.", name));
		}
		public override ValueTerm GetDerivative(Variable variable)
		{
			return new Constant(variable == this ? 1 : 0);
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

