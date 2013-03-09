using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;

namespace Krach.Terms.LambdaTerms
{
	public class Variable : Value, IEquatable<Variable>
	{
		readonly int dimension;
		readonly string name;
		
		public override int Dimension { get { return dimension; } }
		public string Name { get { return name; } }
		
		public Variable(int dimension, string name)
		{
			if (dimension < 0) throw new ArgumentOutOfRangeException("dimension");
			if (name == null) throw new ArgumentNullException("name");
			
			this.dimension = dimension;
			this.name = name;
		}
		
		public override string ToString()
		{
			return name;
		}
		public override bool Equals(object obj)
		{
			return obj is Variable && Equals(this, (Variable)obj);
		}
		public override int GetHashCode()
		{
			return 0;
		}
		public bool Equals(Variable other)
		{
			return object.Equals(this, other);
		}
		public override IEnumerable<Variable> GetFreeVariables()
		{
			yield return this;
		}
		public override Value RenameVariable(Variable oldVariable, Variable newVariable)
		{
			if (this == oldVariable) return newVariable;
			
			return this;
		}
		public override Value Substitute(Variable variable, Value term)
		{
			return variable == this ? term : this;
		}
		public override IEnumerable<double> Evaluate()
		{
			throw new InvalidOperationException(string.Format("Cannot evaluate variable '{0}'.", name));
		}
		public override IEnumerable<Value> GetPartialDerivatives(Variable variable)
		{
			return 
			(
				from variableIndex in Enumerable.Range(0, variable.Dimension)
				select Term.Vector
				(
					from derivativeIndex in Enumerable.Range(0, dimension)
					select Term.Constant(variable == this && variableIndex == derivativeIndex ? 1 : 0)
				)
			)
			.ToArray();
		}
		public Variable FindUnusedVariable(IEnumerable<Variable> usedVariables) 
		{
			Variable variable = this;
			
			while (usedVariables.Contains(variable)) variable = new Variable(variable.Dimension, variable.name + "'");
			
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
			
			return variable1.dimension == variable2.dimension && variable1.name == variable2.name;
		}	
	}
}

