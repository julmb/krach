using System;
using Krach.Basics;
using Krach.Calculus;
using System.Collections.Generic;
using Krach.Calculus.Terms;
using Krach.Extensions;
using System.Linq;
using Krach.Calculus.Terms.Composite;
using Krach.Calculus.Abstract;

namespace Krach.Calculus.Explicit
{
	public class ExplicitConstraint<T> : IConstraint<T>
	{
		readonly T item;
		readonly IEnumerable<OrderedRange<double>> ranges;
		
		public T Item { get { return item; } }
		public IEnumerable<OrderedRange<double>> Ranges { get { return ranges; } }

		public ExplicitConstraint(T item, IEnumerable<OrderedRange<double>> ranges)
		{
			if (item == null) throw new ArgumentNullException("function");
			if (ranges == null) throw new ArgumentNullException("ranges");
			
			this.item = item;
			this.ranges = ranges;
		}
		
		public override string ToString()
		{
			return string.Format("{0}: [{1}]", item, ranges.ToStrings().Separate(", ").AggregateString());
		}
	}
}

