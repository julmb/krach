using System;
using Krach.Basics;
using Krach.Calculus;
using System.Collections.Generic;
using Krach.Calculus.Terms;
using Krach.Extensions;
using System.Linq;
using Krach.Calculus.Terms.Composite;
using Krach.Calculus.Abstract;
using Krach.Calculus.Explicit;

namespace Krach.Calculus.Terms.Constraints
{
	public static class Constraint
	{
        public static IConstraint<FunctionTerm> Abstract(this IConstraint<ValueTerm> constraint, IEnumerable<Variable> variables)
        {
			return new ExplicitConstraint<FunctionTerm>(constraint.Item.Abstract(variables), constraint.Ranges);
        }
        public static IConstraint<FunctionTerm> Abstract(this IConstraint<ValueTerm> constraint, params Variable[] variables)
        {
            return constraint.Abstract((IEnumerable<Variable>)variables);
        }
        public static IConstraint<ValueTerm> Apply(this IConstraint<FunctionTerm> constraint, IEnumerable<ValueTerm> parameters)
        {
			return new ExplicitConstraint<ValueTerm>(constraint.Item.Apply(parameters), constraint.Ranges);
        }
        public static IConstraint<ValueTerm> Apply(this IConstraint<FunctionTerm> constraint, params ValueTerm[] parameters)
        {
            return constraint.Apply((IEnumerable<ValueTerm>)parameters);
        }

		public static IConstraint<ValueTerm> CreateEmpty()
		{
			return new ExplicitConstraint<ValueTerm>
			(
				Term.Vector(),
				Enumerables.Create<OrderedRange<double>>()
			);
		}
		public static IConstraint<ValueTerm> CreateEquality(ValueTerm value1, ValueTerm value2)
		{
			int dimension = Items.Equal(value1.Dimension, value2.Dimension);

			return new ExplicitConstraint<ValueTerm>
			(
				Term.Difference(value1, value2),
				Enumerable.Repeat(new OrderedRange<double>(0), dimension)
			);
		}
		public static IConstraint<ValueTerm> Merge(IEnumerable<IConstraint<ValueTerm>> constraints)
		{
			return new ExplicitConstraint<ValueTerm>
			(
				Term.Vector
				(
					from constraint in constraints
					select constraint.Item
				),
				(
					from constraint in constraints
					from range in constraint.Ranges
					select range
				)
				.ToArray()
			);
		}
	}
}

