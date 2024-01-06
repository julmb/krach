using System;
using System.Collections.Generic;
using Krach.Extensions;
using Krach.Basics;
using System.Linq;

namespace Wrappers.Casadi
{
	public static class Constraints
	{
		public static Constraint<ValueTerm> Create(ValueTerm value, Range<ValueTerm> range)
		{
			return new Constraint<ValueTerm>(value, Enumerable.Repeat(range, value.Dimension));
		}
		public static Constraint<ValueTerm> CreateError(ValueTerm value, ValueTerm targetValue, ValueTerm error)
		{
			return Create(value, new Range<ValueTerm>(Terms.Difference(targetValue, error), Terms.Sum(targetValue, error)));
		}
		public static Constraint<ValueTerm> CreateTarget(ValueTerm value, ValueTerm targetValue)
		{
			return CreateError(value, targetValue, Terms.Constant(0));
		}
		public static Constraint<ValueTerm> CreateZero(ValueTerm value)
		{
			return CreateTarget(value, Terms.Constant(0));
		}
		public static Constraint<ValueTerm> CreateEquality(ValueTerm value1, ValueTerm value2)
		{
			return CreateZero(Terms.Difference(value1, value2));
		}

		public static Constraint<ValueTerm> Merge(IEnumerable<Constraint<ValueTerm>> constraints)
		{
			return new Constraint<ValueTerm>
			(
				Terms.Vector
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
		public static Constraint<ValueTerm> Merge(params Constraint<ValueTerm>[] constraints)
		{
			return Merge((IEnumerable<Constraint<ValueTerm>>)constraints);
		}

		public static Constraint<FunctionTerm> Abstract(this Constraint<ValueTerm> constraint, IEnumerable<ValueTerm> variables)
		{
			return new Constraint<FunctionTerm>(constraint.Item.Abstract(variables), constraint.Ranges);
		}
		public static Constraint<FunctionTerm> Abstract(this Constraint<ValueTerm> constraint, params ValueTerm[] variables)
		{
			return constraint.Abstract((IEnumerable<ValueTerm>)variables);
		}
	}
}

