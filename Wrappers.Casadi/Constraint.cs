using System;
using System.Collections.Generic;
using Krach.Basics;
using Krach.Extensions;

namespace Wrappers.Casadi
{
	public class Constraint<T>
	{        
		readonly T item;
		readonly IEnumerable<Range<ValueTerm>> ranges;

		public T Item { get { return item; } }
		public IEnumerable<Range<ValueTerm>> Ranges { get { return ranges; } }

		public Constraint(T item, IEnumerable<Range<ValueTerm>> ranges)
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

