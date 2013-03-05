using System;
using System.Collections.Generic;
using System.Linq;

namespace Krach.Terms.LambdaTerms
{
	public class Variable : ValueTerm, IEquatable<Variable>
	{
		readonly string name;
		
		public string Name { get { return name; } }
		
		public Variable(string name)
		{
			if (name == null) throw new ArgumentNullException("name");

			this.name = name;
		}
		
		public override bool Equals(object obj)
		{
			return obj is Variable && Equals(this, (Variable)obj);
		}
		public override int GetHashCode()
		{
			return name.GetHashCode();
		}
		public bool Equals(Variable other)
		{
			return object.Equals(this, other);
		}
		public override string GetText()
		{
			return name;
		}
		public override IEnumerable<Variable> GetFreeVariables()
		{
			yield return this;
		}
		public override ValueTerm RenameVariable(Variable oldVariable, Variable newVariable)
		{
			if (this == oldVariable) return newVariable;
			
			return this;
		}
		public override ValueTerm Substitute(Variable variable, ValueTerm term)
		{
			return variable == this ? term : this;
		}
		public override double Evaluate()
		{
			throw new InvalidOperationException(string.Format("Cannot evaluate variable '{0}'.", name));
		}
		public override ValueTerm GetDerivative(Variable variable)
		{
			return new Constant(variable == this ? 1 : 0);
		}
		public Variable FindUnusedVariable(IEnumerable<Variable> usedVariables) 
		{
			Variable variable = this;
			
			while (usedVariables.Contains(variable)) variable = new Variable(variable.name + "'");
			
			return variable;
		}
		
		public static bool operator ==(Variable variable1, Variable variable2)
		{
			return object.Equals(variable1, variable2);
		}
		public static bool operator !=(Variable variable1, Variable variable2)
		{
			return !object.Equals(variable1, variable2);
		}
		
		static bool Equals(Variable variable1, Variable variable2) 
		{
			if (object.ReferenceEquals(variable1, variable2)) return true;
			if (object.ReferenceEquals(variable1, null) || object.ReferenceEquals(variable2, null)) return false;
			
			return variable1.name == variable2.name;
		}	
	}
}

