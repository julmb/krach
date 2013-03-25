using System;
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;
using Krach.Calculus.Terms.Combination;
using Krach.Calculus.Terms;

namespace Krach.Calculus.Terms.Basic
{
	public class Sum : BinaryOperator, IEquatable<Sum>
	{
		readonly int dimension;
		
		public int Dimension { get { return dimension; } }
		public override int DomainDimension { get { return 2 * dimension; } }
		public override int CodomainDimension { get { return dimension; } }
		
		public Sum(int dimension)
		{
			if (dimension < 0) throw new ArgumentOutOfRangeException("dimension");
			
			this.dimension = dimension;
		}
		
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
		
		public override string GetText()
		{
			return "+";
		}
		public override IEnumerable<double> Evaluate(IEnumerable<double> values)
		{
			return
			(
				from index in Enumerable.Range(0, dimension)
				let value1 = values.ElementAt(0 * dimension + index)
				let value2 = values.ElementAt(1 * dimension + index)
				select value1 + value2
			)
			.ToArray();
		}
		public override IEnumerable<FunctionTerm> GetDerivatives()
		{
			Variable x = new Variable(dimension, "x");
			Variable y = new Variable(dimension, "y");
			
			return Enumerables.Concatenate
			(
				from indexX in Enumerable.Range(0, dimension)
				select Term.Vector
				(
					from index in Enumerable.Range(0, dimension)
					select Term.Constant(index == indexX ? 1 : 0)
				)
				.Abstract(x, y),
				from indexY in Enumerable.Range(0, dimension)
				select Term.Vector
				(
					from index in Enumerable.Range(0, dimension)
					select Term.Constant(index == indexY ? 1 : 0)
				)
				.Abstract(x, y)
			)
			.ToArray();
		}
		
		public static bool operator ==(Sum function1, Sum function2)
		{
			return object.Equals(function1, function2);
		}
		public static bool operator !=(Sum function1, Sum function2)
		{
			return !object.Equals(function1, function2);
		}
		
		static bool Equals(Sum function1, Sum function2) 
		{
			if (object.ReferenceEquals(function1, function2)) return true;
			if (object.ReferenceEquals(function1, null) || object.ReferenceEquals(function2, null)) return false;
			
			return function1.dimension == function2.dimension;
		}
	}
}

