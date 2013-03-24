using System;
using System.Collections.Generic;
using Krach.Extensions;
using System.Linq;
using Krach.Calculus.Abstract;
using Krach.Calculus.Terms.Combination;
using Krach.Calculus.Terms;
using Krach.Calculus.Basic;

namespace Krach.Calculus
{
	public class Product : BinaryOperator, IEquatable<Product>
	{
		readonly int dimension;
		
		public int Dimension { get { return dimension; } }
		public override int DomainDimension { get { return 2 * dimension; } }
		public override int CodomainDimension { get { return 1; } }
		
		public Product(int dimension)
		{
			if (dimension < 0) throw new ArgumentOutOfRangeException("dimension");
			
			this.dimension = dimension;
		}
		
		public override bool Equals(object obj)
		{
			return obj is Product && Equals(this, (Product)obj);
		}
		public override int GetHashCode()
		{
			return dimension.GetHashCode();
		}
		public bool Equals(Product other)
		{
			return object.Equals(this, other);
		}
		
		public override string GetText()
		{
			return "∙";
		}
		public override IEnumerable<double> Evaluate(IEnumerable<double> values)
		{
			yield return
			(
				from index in Enumerable.Range(0, dimension)
				let value1 = values.ElementAt(0 * dimension + index)
				let value2 = values.ElementAt(1 * dimension + index)
				select value1 * value2
			)
			.Sum();
		}
		public override IEnumerable<IFunction> GetDerivatives()
		{
			Variable x = new Variable(dimension, "x");
			Variable y = new Variable(dimension, "y");
			
			return Enumerables.Concatenate
			(
				from indexX in Enumerable.Range(0, dimension)
				select y.Select(indexX).Abstract(x, y),
				from indexY in Enumerable.Range(0, dimension)
				select x.Select(indexY).Abstract(x, y)
			)
			.ToArray();
		}
		
		public static bool operator ==(Product function1, Product function2)
		{
			return object.Equals(function1, function2);
		}
		public static bool operator !=(Product function1, Product function2)
		{
			return !object.Equals(function1, function2);
		}
		
		static bool Equals(Product function1, Product function2) 
		{
			if (object.ReferenceEquals(function1, function2)) return true;
			if (object.ReferenceEquals(function1, null) || object.ReferenceEquals(function2, null)) return false;
			
			return function1.dimension == function2.dimension;
		}
	}
}
