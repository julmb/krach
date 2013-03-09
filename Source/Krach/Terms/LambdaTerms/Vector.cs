using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;

namespace Krach.Terms.LambdaTerms
{
	public class Vector : Value
	{
		readonly IEnumerable<Value> values;

		public override int Dimension { get { return values.Sum(value => value.Dimension); } }
		public IEnumerable<Value> Values { get { return values; } }
		
		public Vector(IEnumerable<Value> values)
		{
			if (values == null) throw new ArgumentNullException("values");
			
			this.values = values.ToArray();
		}
		
		public override string ToString()
		{
			return string.Format("({0})", values.ToStrings().Separate(" ").AggregateString());
		}
		public override bool Equals(object obj)
		{
			return obj is Vector && Equals(this, (Vector)obj);
		}
		public override int GetHashCode()
		{
			return 0;
		}
		public bool Equals(Vector other)
		{
			return object.Equals(this, other);
		}
		public override IEnumerable<Variable> GetFreeVariables()
		{
			return 
			(
				from value in values
				from variable in value.GetFreeVariables()
				select variable
			)
			.ToArray();
		}
		public override Value RenameVariable(Variable oldVariable, Variable newVariable)
		{
			return new Vector(values.Select(value => value.RenameVariable(oldVariable, newVariable)));
		}
		public override Value Substitute(Variable variable, Value term)
		{
			return new Vector(values.Select(value => value.Substitute(variable, term)));
		}
		public override IEnumerable<double> Evaluate()
		{
			return 
			(
				from value in values
				from result in value.Evaluate()
				select result
			)
			.ToArray();
		}
		public override IEnumerable<Value> GetPartialDerivatives(Variable variable)
		{
			return 
			(
				from value in values
				from derivative in value.GetPartialDerivatives(variable)
				select derivative
			)
			.ToArray();
		}
		
		public static bool operator ==(Vector vector1, Vector vector2)
		{
			return object.Equals(vector1, vector2);
		}
		public static bool operator !=(Vector vector1, Vector vector2)
		{
			return !object.Equals(vector1, vector2);
		}
		
		static bool Equals(Vector vector1, Vector vector2) 
		{
			if (object.ReferenceEquals(vector1, vector2)) return true;
			if (object.ReferenceEquals(vector1, null) || object.ReferenceEquals(vector2, null)) return false;
			
			return Enumerable.SequenceEqual(vector1.values, vector2.values);
		}
	}
}

