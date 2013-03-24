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
	public class Scaling : BinaryOperator, IEquatable<Scaling>
	{
		readonly int dimension;
		
		public int Dimension { get { return dimension; } }
		public override int DomainDimension { get { return 1 + dimension; } }
		public override int CodomainDimension { get { return dimension; } }
		
		public Scaling(int dimension)
		{
			if (dimension < 0) throw new ArgumentOutOfRangeException("dimension");
			
			this.dimension = dimension;
		}
		
		public override bool Equals(object obj)
		{
			return obj is Scaling && Equals(this, (Scaling)obj);
		}
		public override int GetHashCode()
		{
			return dimension.GetHashCode();
		}
		public bool Equals(Scaling other)
		{
			return object.Equals(this, other);
		}
		
		public override string GetText()
		{
			return "*";
		}
		public override IEnumerable<double> Evaluate(IEnumerable<double> values)
		{
			double factor = values.First();
			
			return
			(
				from value in values.Skip(1)
				select factor * value
			)
			.ToArray();
		}
		public override IEnumerable<IFunction> GetDerivatives()
		{			
			Variable c = new Variable(dimension, "c");
			Variable x = new Variable(dimension, "x");
			
			return Enumerables.Concatenate
			(
				Enumerables.Create(x.Abstract(c, x)),
				from indexX in Enumerable.Range(0, dimension)
				select Term.Vector
				(
					from index in Enumerable.Range(0, dimension)
					select index == indexX ? c : Term.Constant(0)
				)
				.Abstract(c, x)
			)
			.ToArray();
		}
		
		public static bool operator ==(Scaling function1, Scaling function2)
		{
			return object.Equals(function1, function2);
		}
		public static bool operator !=(Scaling function1, Scaling function2)
		{
			return !object.Equals(function1, function2);
		}
		
		static bool Equals(Scaling function1, Scaling function2) 
		{
			if (object.ReferenceEquals(function1, function2)) return true;
			if (object.ReferenceEquals(function1, null) || object.ReferenceEquals(function2, null)) return false;
			
			return function1.dimension == function2.dimension;
		}
	}
}
